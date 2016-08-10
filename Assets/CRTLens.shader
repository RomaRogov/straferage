Shader "Hidden/CRTLens"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				float xdist = (.5 - i.uv.x)*2.;
				float ydist = (.5 - i.uv.y)*2.;
				float dist = 1. - sqrt(xdist*xdist / 2. + ydist*ydist / 2.);

				float deEffect = 6.;

				float2 lensShift = i.uv + float2(xdist*dist / deEffect, ydist*dist / deEffect);
				float2 colorShift = lensShift - float2(.01 + .001 * sin(i.uv.y * 300. /*+ iGlobalTime  * 50.*/), 0.);
				
				return tex2D(_MainTex, lensShift) / (fixed4(1.,1.,1.,1.) - tex2D(_MainTex, colorShift)) * .3;
			}
			ENDCG
		}
	}
}
