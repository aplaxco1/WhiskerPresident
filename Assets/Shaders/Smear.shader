// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Smear"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
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
		Tags{ "RenderType" = "Opaque" }
		LOD 200


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
		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};
	
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _PrevPosition;
		fixed4 _Position;
		fixed3 _Rotation;
		fixed3 _PrevRotation;
	
		half _NoiseScale;
		half _NoiseHeight;
        float _Smearing;
	
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
	
		void vert(inout appdata_full v, out Input o)
		{
			
            if (!_Smearing) {
                return;
            }
			UNITY_INITIALIZE_OUTPUT(Input, o);
			fixed4 worldPos = mul(unity_ObjectToWorld, v.vertex);
	
			fixed3 worldOffset = _Position.xyz - _PrevPosition.xyz; // -5
			fixed3 localOffset = worldPos.xyz - _Position.xyz; // -5
			fixed3 rotationOffset = _Rotation - _PrevRotation; // -5
	
			// World offset should only be behind swing
			float dirDot = dot(normalize(worldOffset), normalize(mul(localOffset,rotationOffset)));
			fixed3 unitVec = fixed3(1, 1, 1) * _NoiseHeight;
			worldOffset = clamp(worldOffset, unitVec * -1, unitVec);
			//worldOffset *= -clamp(dirDot, -1, 0) * lerp(1, 0, step(length(worldOffset), 0));
	
			fixed3 smearOffset = -worldOffset.xyz * lerp(1, noise(worldPos * _NoiseScale), step(0, _NoiseScale));
			
			worldPos.xyz += smearOffset;
			
			v.vertex = mul(unity_WorldToObject, worldPos);
		}
	
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
	
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	

	FallBack "Diffuse"
}