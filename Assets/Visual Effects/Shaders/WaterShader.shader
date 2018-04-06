Shader "timrodz/WaterShader" 
{
	Properties 
	{
		_MainTint ("Diffuse Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ScrollXSpeed ("X Scroll Speed", Range(0, 10)) = 2
		_ScrollYSpeed ("Y Scroll Speed", Range(0, 10)) = 2

		_Speed("Wave Speed", Range(0.1, 80)) = 5
		_Frequency("Wave frequency", Range(0, 5)) = 2
		_Amplitude("Wave Amplitude", Range(-1, 1)) = 1
	}
	SubShader 
	{
		Tags 
		{ "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		// #pragma surface surf Standard fullforwardshadows
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _MainTint;
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;

		// Moving water
		float _Speed;
		float _Frequency;
		float _Amplitude;
		float _OffsetVal;

		struct Input 
		{
			float2 uv_MainTex;
			float3 vertColor;
		};

		// half _Glossiness;
		// half _Metallic;
		// fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			float time = _Time * _Speed;

			float waveValueA = sin(time + v.vertex.x * _Frequency) * _Amplitude;

			v.vertex.xyz = float3(v.vertex.x, v.vertex.y + waveValueA, v.vertex.z);

			v.normal = normalize(float3(v.normal.x + waveValueA, v.normal.y, v.normal.z));

			//o.vertColor = float3(waveValueA,waveValueA,waveValueA); 
		}

		// void vert(inout appdata_full v, out Input o)
		// {
		// 	float time = _Time * _Speed;

		// 	float waveValueA = sin(time + v.vertex.x * _Frequency) * _Amplitude;

		// 	v.vertex.xyz = float3(v.vertex.x, v.vertex.y + waveValueA, v.vertex.z);

		// 	v.normal = normalize(float3(v.normal.x + waveValueA, v.normal.y, v.normal.z));
		// 	//v.normal = normalize(float3(v.normal.x + waveValueA, v.normal.y, v.normal.z));

		// 	o.vertColor = float3(waveValueA, waveValueA, waveValueA);
		// }

		void surf (Input IN, inout SurfaceOutput o) 
		{
			// // Albedo comes from a texture tinted by color
			// fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			// o.Albedo = c.rgb;
			// // Metallic and smoothness come from slider variables
			// o.Metallic = _Metallic;
			// o.Smoothness = _Glossiness;
			// o.Alpha = c.a;

			fixed2 scrolledUV = IN.uv_MainTex;

			fixed xScrollValue = _ScrollXSpeed * _Time;
			fixed yScrollValue = _ScrollYSpeed * _Time;

			scrolledUV += fixed2(xScrollValue, yScrollValue);

			half4 c = tex2D(_MainTex, scrolledUV);

			o.Albedo = c.rgb * _MainTint * 3;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
