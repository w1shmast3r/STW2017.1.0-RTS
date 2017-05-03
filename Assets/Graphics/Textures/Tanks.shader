Shader "Custom/Tanks" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Emission("Emission", 2D) = "white" {}
		_Occlusion("Occlusion", 2D) = "white" {}
		_OcclusionStrength("Occlusion Strength", range(0,2)) = 1
		_GlowColor("Glow Color", Color) = (1,1,1,1)
		_GlowStrength("Glow Strength", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _Emission;
		sampler2D _Occlusion;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _GlowColor;
		float _OcclusionStrength;
		float _GlowStrength;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(c.rgb, c.rgb*_Color, c.a);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = tex2D(_Emission, IN.uv_MainTex)*_GlowColor*_GlowStrength;
			o.Occlusion = tex2D(_Occlusion, IN.uv_MainTex)/_OcclusionStrength;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
