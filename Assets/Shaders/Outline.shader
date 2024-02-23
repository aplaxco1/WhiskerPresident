// Simplest Shader that takes texture input

Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags
        { 
            "RenderType"="Transparent"
            "Queue" = "Transparent"
        }
        LOD 100

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
                float3 normal : NORMAL;
            };

            struct v2f // vertex shader outputs to fragment shader
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenSpace : TEXCOORD1;
                float3 normal :TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            sampler2D _MainTex;
            //Texture2D _MainTex;
            //SamplerState sampler_MainTex;
            float4 _MainTex_TexelSize;
            float _Scale;

            //float4 _MainTex_ST;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;

            v2f vert (appdata v) // vertex shader
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                o.screenSpace = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target // fragment shader
            {
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
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 10;

                return edgeDepth;
                
                //return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenSpaceUV);
                //return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
