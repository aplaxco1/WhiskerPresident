Shader "Unlit/CameraShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 viewNormal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                o.viewNormal = mul((float3x3)UNITY_MATRIX_MV, v.normal);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return float4(normalize(i.viewNormal), 0);
            }
            ENDCG
        }
    }
}
