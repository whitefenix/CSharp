Shader "Custom/Water" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_FlowX ("FlowX", float) = 0.0
		_FlowY ("FlowY", float) = 0.0
		_Dist ("Dist", Range(0.0001,1)) = 0.01
		_Wave ("Wave", Range(0.001,1)) = 0.5
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

		struct Input {
			float2 uv_MainTex;
			float2 worldPos;
			float2 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _FlowX;
		float _FlowY;
		half _Wave;
		half _Dist;

		float random(float p) {
		  return frac(sin(p)*10000.);
		}

		float noise(float2 p) {
		  return random(p.x + p.y*10000.);
		}

		float2 sw(float2 p) {return float2( floor(p.x) , floor(p.y) );}
		float2 se(float2 p) {return float2( ceil(p.x)  , floor(p.y) );}
		float2 nw(float2 p) {return float2( floor(p.x) , ceil(p.y)  );}
		float2 ne(float2 p) {return float2( ceil(p.x)  , ceil(p.y)  );}

		float smoothNoise(float2 p) {
		  float2 inter = smoothstep(0., 1., frac(p));
		  float s = lerp(noise(sw(p)), noise(se(p)), inter.x);
		  float n = lerp(noise(nw(p)), noise(ne(p)), inter.x);
		  return lerp(s, n, inter.y);
		  return noise(nw(p));
		}

		float movingNoise(float2 p) {
		  float total = 0.0;
		  total += smoothNoise(p     - _Time.y * _Wave);
		  total += smoothNoise(p*2.  + _Time.y * _Wave) / 2.;
		  total += smoothNoise(p*4.  - _Time.y * _Wave) / 4.;
		  total += smoothNoise(p*8.  + _Time.y * _Wave) / 8.;
		  total += smoothNoise(p*16. - _Time.y * _Wave) / 16.;
		  total /= 1. + 1./2. + 1./4. + 1./8. + 1./16.;
		  return total;
		}

		float nestedNoise(float2 p) {
		  float x = movingNoise(p);
		  float y = movingNoise(p + 100.);
		  return movingNoise(p + float2(x, y));
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			IN.uv_MainTex.x += nestedNoise(IN.worldPos) * _Dist;
			IN.uv_MainTex.y += nestedNoise(IN.worldPos) * _Dist;

			IN.uv_MainTex.x += _FlowX * _Time.x;
			IN.uv_MainTex.y += _FlowY * _Time.x;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
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
