Shader "Custom/DissolveSurface" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness("Smoothness", Range(0,1)) = 0.5
		//_Metallic("Metallic", Range(0,1)) = 0.0

		_DissolveTexture("DissolveTexture (RGB)", 2D) = "white" {}
		_DissolveAmount("DissolveAmount", Range(0.0, 1.0)) = 0

		_BurnRamp("Burn Ramp (RGB)", 2D) = "white" {}
		_BurnSize("Burn Size", Range(0.0, 1.0)) = 0.15
		_BurnColor("Burn Color", Color) = (1,1,1,1)
		_EmissionAmount("Emission amount", float) = 2.0
}

SubShader{
	Tags { "RenderType" = "Opaque" }
	LOD 200
	Cull Off //Fast way to turn your material double-sided

	CGPROGRAM
	#pragma surface surf Lambert addshadow
	#pragma target 3.0


	//Dissolve properties

	fixed4 _Color;
	sampler2D _MainTex;
	sampler2D _DissolveTexture;
	sampler2D _BurnRamp;
	fixed4 _BurnColor;
	float _BurnSize;
	float _DissolveAmount;
	float _EmissionAmount;


	//half _Glossiness;
	//half _Metallic;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		// Handle Dissolve Effect Values
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		half test = tex2D(_DissolveTexture, IN.uv_MainTex).rgb - _DissolveAmount;
		clip(test);

		// Check Burn Rate
		if (test < _BurnSize && _DissolveAmount > 0) {
			o.Emission = tex2D(_BurnRamp, float2(test * (1 / _BurnSize), 0)) * _BurnColor * _EmissionAmount;
		}

		o.Albedo = c.rgb;
		o.Alpha = c.a;

	}
	
	ENDCG
	}
	FallBack "Diffuse"
}