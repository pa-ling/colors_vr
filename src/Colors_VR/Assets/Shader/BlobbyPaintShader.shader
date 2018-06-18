Shader "Custom/Blobby Paint" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_PaintR("Paint R", Color) = (1, 0, 0, 1)
		_PaintG("Paint G", Color) = (0, 1, 0, 1)
		_PaintB("Paint B", Color) = (0, 0, 1, 1)
		_PaintA("Paint A", Color) = (0, 0, 0, 1)
		_Freq("Frequency", Float) = 5
		_Threshold("Threshold", Range(0,1)) = 0.3
		_Amp("Noise Amplitude", Range(0,1)) = 0.5
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert fullforwardshadows
		#pragma target 3.0

		#include "noise-simplex/noiseSimplex.cginc"

		sampler2D _MainTex;
		float4 _PaintR;
		float4 _PaintG;
		float4 _PaintB;
		float4 _PaintA;
		float _Freq;
		float _Threshold;
		float _Amp;


		struct Input {
			float2 uv_MainTex;
			float4 color : COLOR;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			float noise = 0;
			float highestChannelValue = 0;
			float highestChannelIndex = 4; //there are only 4 channels, so index 4 does not exist

			if (highestChannelValue < IN.color.r) {
				highestChannelValue = IN.color.r;
				highestChannelIndex = 0;
			}

			if (highestChannelValue < IN.color.g) {
				highestChannelValue = IN.color.g;
				highestChannelIndex = 1;
			}

			if (highestChannelValue < IN.color.b) {
				highestChannelValue = IN.color.b;
				highestChannelIndex = 2;
			}

			if (highestChannelValue < IN.color.a) {
				highestChannelValue = IN.color.a;
				highestChannelIndex = 3;
			}

			if (highestChannelValue > 0.01) {
				noise = snoise(IN.worldPos * _Freq) * _Amp + highestChannelValue;
			}

			if (highestChannelValue + noise > _Threshold && highestChannelIndex < 4) {
				if (highestChannelIndex == 0) {
					o.Albedo = _PaintR;
				} else if (highestChannelIndex == 1) {
					o.Albedo = _PaintG;
				} else if (highestChannelIndex == 2) {
					o.Albedo = _PaintB;
				} else if (highestChannelIndex == 3) {
					o.Albedo = _PaintA;
				}
			}
			else {
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
			}
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
