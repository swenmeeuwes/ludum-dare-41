Shader "Camera/ColorFadeShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {} // Camera needs this
		_FadeColor ("Fade Color", Color) = (0, 0, 0, 1)
		_Fade ("Fade", Range(0, 1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _FadeColor;
			float _Fade;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 screenColor = tex2D(_MainTex, i.uv);
				return lerp(screenColor, _FadeColor, _Fade);
			}
			ENDCG
		}
	}
}
