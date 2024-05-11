// 2 of the components to the outlines are based on https://roystan.net/articles/outline-shader/
// smear effect is based off of https://github.com/cjacobwade/HelpfulScripts/tree/master/SmearEffect
Shader "Unlit/HalftoneSmear"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HalftonePattern ("Halftone Pattern", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
        
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 1
        [IntRange] _Highlighted("Highlighted", Range(0,1)) = 1
        [IntRange] _Smearing("Smearing", Range(0,1)) = 1

		_Position("Position", Vector) = (0, 0, 0, 0)
		_PrevPosition("Prev Position", Vector) = (0, 0, 0, 0)
		_Rotation("Rotation", Vector) = (0, 0, 0)
		_PrevRotation("Prev Rotation", Vector) = (0, 0, 0)

		_NoiseScale("Noise Scale", Float) = 15
		_NoiseHeight("Noise Height", Float) = 1.3
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
            sampler2D _HalftonePattern;
            float4 _HalftonePattern_ST;
            float4 _AmbientColor;
            float4 _Color;

            float _Highlighted;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;

            float _Smearing;
            fixed4 _PrevPosition;
            fixed4 _Position;
            fixed3 _Rotation;
            fixed3 _PrevRotation;
            half _NoiseScale;
            half _NoiseHeight;

            
	
            float hash(float n)
            {
                return frac(sin(n)*43758.5453);
            }
        
            float noise(float3 x)
            {
                // The noise function returns a value in the range -1.0f -> 1.0f
        
                float3 p = floor(x);
                float3 f = frac(x);
        
                f = f*f*(3.0 - 2.0*f);
                float n = p.x + p.y*57.0 + 113.0*p.z;
        
                return lerp(lerp(lerp(hash(n + 0.0), hash(n + 1.0), f.x),
                    lerp(hash(n + 57.0), hash(n + 58.0), f.x), f.y),
                    lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
                        lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
            }

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
                if (!_Smearing) {
                    return o;
                }
                fixed4 worldPos = mul(unity_ObjectToWorld, v.pos);
        
                fixed3 worldOffset = _Position.xyz - _PrevPosition.xyz; // -5
                fixed3 rotationOffset = _Rotation - _PrevRotation;
                float angle = length(rotationOffset);
                if (angle == 0) { // im so mad.
                    fixed3 smearOffset = -worldOffset;
                    smearOffset *= lerp(1, noise(worldPos * _NoiseScale), step(0, _NoiseScale));
                    
                    worldPos.xyz += smearOffset;
                    
                    o.pos = UnityObjectToClipPos(mul(unity_WorldToObject, worldPos));
                    return o;
                }
                fixed3 localOffset = worldPos.xyz - _Position.xyz; // -5
                float radius = length(localOffset);
                float n = 2 * radius * sin(angle/2*3.14159/180);
                fixed3 angularVector = normalize(rotationOffset)*n;
        
                // World offset should only be behind swing
                float dirDot = dot(normalize(worldOffset), localOffset);
                fixed3 unitVec = fixed3(1, 1, 1) * _NoiseHeight;
                worldOffset = clamp(worldOffset, unitVec * -1, unitVec);
                worldOffset *= -clamp(dirDot, -1, 0) * lerp(1, 0, step(length(worldOffset), 0));
                
                // angular offset should be behind swing as well. but how?
                float dirDot2 = dot(normalize(angularVector), localOffset);
                angularVector = clamp(angularVector, unitVec * -1, unitVec);
                angularVector *= -(clamp(dirDot2, -1, 0)) * lerp(1, 0, step(length(angularVector), 0)) * 7;
        
                fixed3 smearOffset = -angularVector-worldOffset;
                smearOffset *= lerp(1, noise(worldPos * _NoiseScale), step(0, _NoiseScale));
                
                worldPos.xyz += smearOffset;
                
                o.pos = UnityObjectToClipPos(mul(unity_WorldToObject, worldPos));
                return o;
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

                // calculate directional lighting
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float shadow = SHADOW_ATTENUATION(i);
                float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
                float lightIntensity2 = smoothstep(0, 0.01, (NdotL * shadow)-0.2f); // adds 2-step gradient to shadow
                float lightIntensity3 = smoothstep(0, 0.01, (NdotL * shadow)-0.8f); // adds highlight (looks ugly on president rn but will look great on things with normal maps)
                float4 light = ((lightIntensity + lightIntensity2)/2 + lightIntensity3*0.1) * _LightColor0;
                light = NdotL*shadow;

                // calculate rim
                float4 rimDot = 1-dot(viewDir, normal);
                float rimIntensity = smoothstep(0.79, 0.80, rimDot * pow(NdotL, 0.1));

                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);// * _Color;
                /*
                if (!_Highlighted) {
                    return (light + _AmbientColor + rimIntensity) * col;
                }
                */



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
                
                float rimIntensity2 = smoothstep(DepthThreshold-0.01, DepthThreshold, edgeDepth);
                //rimIntensity2 += edgeNormal;
                rimIntensity2 += smoothstep(0.79, 0.80, rimDot);
                rimIntensity2 = smoothstep(0.3, 0.80, rimIntensity2);
                // calculate specular
                /*
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);
                float specularIntensity = pow(NdotH * lightIntensity, 32 * 32);
                float4 specular = smoothstep(0.005, 0.01, specularIntensity);
                */
                float4 col2;
                if (smoothstep(0, 0.01, (NdotL * shadow)-0.2f) > 0) {
                    col2 = 1.8 * col;
                } else {
                    col2 = 0.2 * col;
                }

                screenSpaceUV = TRANSFORM_TEX(screenSpaceUV, _HalftonePattern);
                
                float halftoneValue = tex2D(_HalftonePattern, screenSpaceUV).r;
                //halftoneValue = map(halftoneValue, _RemapInputMin, _RemapInputMax, _RemapOutputMin, _RemapOutputMax);

                // anti-aliasing
                float halftoneChange = fwidth(halftoneValue) * 0.6;
                light = smoothstep(halftoneValue - halftoneChange, halftoneValue + halftoneChange, light+0.28);
                //light = step(halftoneValue, light+0.28);
                //return halftoneValue;
                lightIntensity3 = smoothstep(0, 0.2, (NdotL * shadow)-0.8f);
                float highlight = smoothstep(halftoneValue - halftoneChange, halftoneValue + halftoneChange, lightIntensity3);

                light = (light + (1-light)*_AmbientColor + rimIntensity + highlight*0.2/*+ specular*/);

                return (1-rimIntensity2) * (light+0.4) * col + rimIntensity2 * col2;
            }
            ENDCG
        }

        
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        
    }
}
