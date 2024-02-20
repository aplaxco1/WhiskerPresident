Shader "Unlit/Torso"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "gray" {}
        
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
        }
        LOD 100

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
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AmbientColor;

            v2f vert (appdata v) // vertex shader
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = v.uv;

                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.pos);

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

                // calculate specular
                /*
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);
                float specularIntensity = pow(NdotH * lightIntensity, 32 * 32);
                float4 specular = smoothstep(0.005, 0.01, specularIntensity);
                */

                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                return (light + _AmbientColor + rimIntensity /*+ specular*/) * col;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
