// 2 of the components to the outlines are based on https://roystan.net/articles/outline-shader/
// smear effect is based off of https://github.com/cjacobwade/HelpfulScripts/tree/master/SmearEffect
Shader "Unlit/HalftoneObjects"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HalftonePattern ("Halftone Pattern", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        [IntRange] _Textured("Textured", Range(0,1)) = 1
        //[IntRange] _AmbientZ("Textured", Range(0,1)) = 1
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
        _RemapInputMin ("Remap input min value", Range(0, 1)) = 0
        _RemapInputMax ("Remap input max value", Range(0, 1)) = 1
        _RemapOutputMin ("Remap output min value", Range(0, 1)) = 0
        _RemapOutputMax ("Remap output max value", Range(0, 1)) = 1
        
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "LightMode" = "ForwardBase"
            "Queue" = "Geometry-1"
        }
        LOD 100
        Pass
        {
            ColorMask 0
			ZWrite Off

			Stencil // find a way to make this only run if normals are pointing up
			{
				Ref [_StencilID]
				Comp notEqual
				Pass Replace
			}
            
        }

        
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma multi_compile_fwdbase
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata // vertex shader inputs
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f // vertex shader outputs to fragment shader
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal: NORMAL;
                float3 viewDir : TEXCOORD1;
                float2 depth : TEXCOORD3;
                float4 screenSpace : TEXCOORD4;
                float4 posWorld : TEXCOORD5;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _HalftonePattern;
            float4 _HalftonePattern_ST;
            float4 _AmbientColor;
            float4 _Color;

            float _Textured;

            float _RemapInputMin;
            float _RemapInputMax;
            float _RemapOutputMin;
            float _RemapOutputMax;

            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;

            v2f vert (appdata v) // vertex shader
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = v.uv;
                o.posWorld = mul(unity_ObjectToWorld, v.pos);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.pos);
                o.screenSpace = ComputeScreenPos(o.pos);
                //o.screenSpace = TRANSFORM_TEX(o.screenSpace, _HalftonePattern);

                UNITY_TRANSFER_DEPTH(o.depth);

                TRANSFER_SHADOW(o)
                return o;
            }
            float map(float input, float inMin, float inMax, float outMin,  float outMax){
                float relativeValue = (input - inMin) / (inMax - inMin);
                return lerp(outMin, outMax, relativeValue);
            }
            float findEdgeNormal(float2 screenSpaceUV, sampler2D _CameraNormalsTexture) {
				float2 bottomLeftUV = screenSpaceUV-0.001;
				float2 topRightUV = screenSpaceUV+0.001;  
				float2 bottomRightUV = screenSpaceUV + float2(0.001, -0.001);
				float2 topLeftUV = screenSpaceUV + float2(-0.001, 0.001);
                
                float4 normal0 = tex2D(_CameraNormalsTexture, bottomLeftUV);
                float4 normal1 = tex2D(_CameraNormalsTexture, topRightUV);
                float4 normal2 = tex2D(_CameraNormalsTexture, bottomRightUV);
                float4 normal3 = tex2D(_CameraNormalsTexture, topLeftUV);
                float4 normalFiniteDifference0 = normal1 - normal0;
                float4 normalFiniteDifference1 = normal3 - normal2;
                float NormalThreshold = 1;
                float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
                edgeNormal = edgeNormal > NormalThreshold ? 1 : 0;
                return edgeNormal;
            }

            fixed4 frag (v2f i) : SV_Target // fragment shader
            {
                // normalize inputs from vertex shader
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                float3 lightDirection;
                float attenuation;
                if (_WorldSpaceLightPos0.w == 0.0) { //directional lighting
                    lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    attenuation = 1.0;
                } else { //point or spot
                    float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
                    attenuation = 1.0/length(vertexToLightSource);
                    lightDirection = normalize(vertexToLightSource);
                }

                // calculate lighting
                float NdotL = dot(lightDirection, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float4 light = smoothstep(0, 0.8,NdotL * shadow);

                // calculate rim
                float4 rimDot = 1-dot(viewDir, normal);
                //return rimDot;
                float rimIntensity = smoothstep(0.79, 0.80, rimDot * pow(NdotL, 0.1));
                // sample the texture
                float4 col = _Color;
                if (_Textured) {
                    col = tex2D(_MainTex, i.uv);
                }

                float2 screenSpaceUV = i.screenSpace.xy / i.screenSpace.w;
                float rimIntensity2 = 0;
                

                // depth based outlines
				float2 bottomLeftUV = screenSpaceUV-0.001;
				float2 topRightUV = screenSpaceUV+0.001;  
				float2 bottomRightUV = screenSpaceUV + float2(0.001, -0.001);
				float2 topLeftUV = screenSpaceUV + float2(-0.001, 0.001);

                float depth0 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, bottomLeftUV).r;
                float depth1 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, topRightUV).r;
                float depth2 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, bottomRightUV).r;
                float depth3 = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, topLeftUV).r;
                
                
                float depthFiniteDifference0 = depth1 - depth0;
                float depthFiniteDifference1 = depth3 - depth2;
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 50;
                float DepthThreshold = 1.5 * depth0;
                edgeDepth = edgeDepth > DepthThreshold ? 1 : 0;
                
                // camera normals based outlines
                // anti alias edgenormal through supersampling
                float edgeNormal;
                edgeNormal = (findEdgeNormal(screenSpaceUV, _CameraNormalsTexture)
                            + findEdgeNormal(screenSpaceUV+ float2(0.0005, 0), _CameraNormalsTexture)
                            + findEdgeNormal(screenSpaceUV+ float2(0, 0.0005), _CameraNormalsTexture)
                            + findEdgeNormal(screenSpaceUV+ float2(-0.0005, 0), _CameraNormalsTexture)
                            + findEdgeNormal(screenSpaceUV+ float2(0, -0.0005), _CameraNormalsTexture))/5;
                
                rimIntensity2 = smoothstep(DepthThreshold-0.01, DepthThreshold, edgeDepth);
                rimIntensity2 += edgeNormal;
                rimIntensity2 += smoothstep(0.79, 0.80, rimDot);
                rimIntensity2 = smoothstep(0, 0.5, rimIntensity2);
                
                float4 col2;
                if (smoothstep(0, 0.01, (NdotL * shadow)-0.2f) > 0) {
                    col2 = 1.8 * col;
                } else {
                    col2 = 0.2 * col;
                }
                //return rimIntensity2;

                // apply the halftone mask to the lighting
                screenSpaceUV = TRANSFORM_TEX(screenSpaceUV, _HalftonePattern);
                float halftoneValue = tex2D(_HalftonePattern, screenSpaceUV).r;
                halftoneValue = map(halftoneValue, _RemapInputMin, _RemapInputMax, _RemapOutputMin, _RemapOutputMax);

                // anti-aliasing
                float halftoneChange = fwidth(halftoneValue) * 0.5;
                light = smoothstep(halftoneValue - halftoneChange, halftoneValue + halftoneChange, light);
                float4 light2 = smoothstep(0, 0.7, normal.y + normal.x);
                light2 = smoothstep(halftoneValue - halftoneChange, halftoneValue + halftoneChange, light2);
                //return halftoneValue;
                float lightIntensity3 = smoothstep(0, 0.07, (NdotL * shadow)-0.8f);
                float highlight = smoothstep(halftoneValue - halftoneChange, halftoneValue + halftoneChange, lightIntensity3);

                light = (light + (1-light)*_AmbientColor + rimIntensity + highlight*0.2 /*+ specular*/);
                return (1-rimIntensity2) * (light+0.4) * col + rimIntensity2 * float4(0.05,0.05,0.05,1);
            }
            ENDCG
        }

        
 
        Pass
        {  
            Tags { "LightMode" = "ForwardAdd" } 
            Blend One One
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata // vertex shader inputs
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f // vertex shader outputs to fragment shader
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal: NORMAL;
                float3 viewDir : TEXCOORD1;
                float2 depth : TEXCOORD3;
                float4 screenSpace : TEXCOORD4;
                float4 posWorld : TEXCOORD5;
                float3 normalDir : TEXCOORD7;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _HalftonePattern;
            float4 _HalftonePattern_ST;
            float4 _AmbientColor;
            float4 _Color;

            float _Textured;

            float _RemapInputMin;
            float _RemapInputMax;
            float _RemapOutputMin;
            float _RemapOutputMax;

            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;

            v2f vert (appdata v) // vertex shader
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = v.uv;
                o.posWorld = mul(unity_ObjectToWorld, v.pos);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.pos);
                o.screenSpace = ComputeScreenPos(o.pos);
                //o.screenSpace = TRANSFORM_TEX(o.screenSpace, _HalftonePattern);

                UNITY_TRANSFER_DEPTH(o.depth);

                TRANSFER_SHADOW(o)
                
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                return o;
            }
            float map(float input, float inMin, float inMax, float outMin,  float outMax){
                float relativeValue = (input - inMin) / (inMax - inMin);
                return lerp(outMin, outMax, relativeValue);
            }
            
            fixed4 frag (v2f i) : SV_Target // fragment shader
            {
                // normalize inputs from vertex shader
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                float3 lightDirection;
                float attenuation;
                if (_WorldSpaceLightPos0.w == 0.0) { //directional lighting
                    lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    attenuation = 1.0;
                } else { //point or spot
                    float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
                    attenuation = 1.0/length(vertexToLightSource);
                    lightDirection = normalize(vertexToLightSource);
                }
                float NdotL = dot(lightDirection, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float4 light = smoothstep(0, 0.8,NdotL * shadow);


                // apply the halftone mask to the lighting
                float2 screenSpaceUV = i.screenSpace.xy / i.screenSpace.w;
                screenSpaceUV = TRANSFORM_TEX(screenSpaceUV, _HalftonePattern);
                float halftoneValue = tex2D(_HalftonePattern, screenSpaceUV).r;
                halftoneValue = map(halftoneValue, _RemapInputMin, _RemapInputMax, _RemapOutputMin, _RemapOutputMax);

                // anti-aliasing
                float halftoneChange = fwidth(halftoneValue) * 0.5;
                light = smoothstep(halftoneValue - halftoneChange, halftoneValue + halftoneChange, light);

                
                // sample the texture
                float4 col = _Color;
                if (_Textured) {
                    col = tex2D(_MainTex, i.uv);
                }

                return (light) * col;
            }
            ENDCG
        }

        
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        
    }
}
