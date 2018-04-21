Shader "ScreenTransition/Simple"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {} // Used by the camera
		_TransitionTexture("Transition Texture", 2D) = "white" {}
		_TransitionColor("Transition Color", Color) = (0, 0, 0, 1)
		_Cutoff("Cutoff", Range(0, 1)) = 0
		_Opacity("Opacity", Range(0,1)) = 1
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Tags 
		{ 
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _TransitionTexture;
			float4 _TransitionColor;
			float4 _TransitionTexture_ST;
			float _Cutoff;
			float _Opacity;

			float brightness (float4 color)
			{
				return color.x * 0.3 + color.y * 0.59 + color.z * 0.11;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _TransitionTexture);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 cameraColor = tex2D(_MainTex, i.uv);
				float4 transitionColor = tex2D(_TransitionTexture, i.uv);				
				if (_Cutoff > 0.01 && brightness(transitionColor) <= _Cutoff)
					cameraColor = lerp(cameraColor, _TransitionColor, _Opacity);

				return cameraColor;
			}
			ENDCG
		}
	}
}
