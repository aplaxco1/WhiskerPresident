Shader "Unlit/Torso"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 1
        [IntRange] _Highlighted("Highlighted", Range(0,1)) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
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
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata // vertex shader inputs
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f // vertex shader outputs to fragment shader
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            //float4 _MainTex_ST;

            v2f vert (appdata v) // vertex shader
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target // fragment shader
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

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
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AmbientColor;
            float4 _Color;
            float _Highlighted;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;

            v2f vert (appdata v) // vertex shader
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = v.uv;

                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.pos);
                o.screenSpace = ComputeScreenPos(o.pos);

                UNITY_TRANSFER_DEPTH(o.depth);

                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target // fragment shader
            {
                // normalize inputs from vertex shader
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                // calculate directional lighting
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
                float lightIntensity2 = smoothstep(0, 0.01, (NdotL * shadow)-0.2f); // adds 2-step gradient to shadow
                //float lightIntensity3 = smoothstep(0, 0.01, (NdotL * shadow)-0.8f); // adds highlight (looks ugly on president rn but will look great on things with normal maps)
                float4 light = ((lightIntensity + lightIntensity2)/2 /*+ lightIntensity3*/) * _LightColor0;

                // calculate rim
                float4 rimDot = 1-dot(viewDir, normal);
                float rimIntensity = smoothstep(0.79, 0.80, rimDot * pow(NdotL, 0.1));



                float2 screenSpaceUV = i.screenSpace.xy / i.screenSpace.w;
                //float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenSpaceUV));

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

                float4 normal0 = tex2D(_CameraNormalsTexture, bottomLeftUV);
                float4 normal1 = tex2D(_CameraNormalsTexture, topRightUV);
                float4 normal2 = tex2D(_CameraNormalsTexture, bottomRightUV);
                float4 normal3 = tex2D(_CameraNormalsTexture, topLeftUV);
                float4 normalFiniteDifference0 = normal1 - normal0;
                float4 normalFiniteDifference1 = normal3 - normal2;
                float NormalThreshold = 0.4;
                float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
                edgeNormal = edgeNormal > NormalThreshold ? 1 : 0;
                
                //return edgeNormal;
                //return normalFiniteDifference1;
                //UNITY_OUTPUT_DEPTH(i.depth);
                if (_Highlighted) {
                    rimIntensity = smoothstep(DepthThreshold-0.01, DepthThreshold, edgeDepth);
                    rimIntensity += edgeNormal;
                    rimIntensity += smoothstep(0.79, 0.80, rimDot);
                }

                // calculate specular
                /*
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);
                float specularIntensity = pow(NdotH * lightIntensity, 32 * 32);
                float4 specular = smoothstep(0.005, 0.01, specularIntensity);
                */

                // sample the texture
                float4 col = tex2D(_MainTex, i.uv) * _Color;
                return (light + _AmbientColor + rimIntensity /*+ specular*/) * col;
            }
            ENDCG
        }

        
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        
    }
}
