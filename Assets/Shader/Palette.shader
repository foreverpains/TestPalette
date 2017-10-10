Shader "Unlit/Palette"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MinRadius ("MinRadius", Float) = 0.3
		_MaxRadius ("MaxRadius", Float) = 0.5
		_RotateOffset ("RotateOffset", Range(0.0, 360.0)) = 0.0
		_RadiusOffset ("RadiusOffset", Range(-0.5, 5.5)) = 2.5
		_Center ("Center", Vector) = (0.5, 0.5, 0, 0)
		_RimWidth ("Rim Width", Range(0,0.2)) = 0
		_RimColor ("Rim Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		//选取Alpha混合方式
        Blend SrcAlpha OneMinusSrcAlpha

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
			float4 _MainTex_ST;
			float _MinRadius;
			float _MaxRadius;
			float _RotateOffset;
			float _RadiusOffset;
			float2 _Center;
			fixed4 _OutColor;
			float _RimWidth;
			fixed4 _RimColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float r = distance(i.uv, _Center);
				clip(r - _MinRadius);
				clip(_MaxRadius - r);
				float2 vec = i.uv - _Center;
				float rotate = degrees(atan2(vec.y, vec.x)) + _RotateOffset;
				float ra = ((r - _MinRadius) * 2.0f / (_MaxRadius - _MinRadius) + _RadiusOffset) / 7.0f;
				clip(ra);
				clip(1.0f - ra);
				float2 uv = float2(rotate / 360.0f, 1.0f - ra);
				// sample the texture

				


				fixed4 col = tex2D(_MainTex, uv);

				////多重采样
				//col =(col+ tex2D(_MainTex, uv + float2(0,_RimWidth)) + tex2D(_MainTex, uv + float2(0,-_RimWidth)))/3;

				////描边
				//if(r<_MinRadius + _RimWidth|| r >_MaxRadius - _RimWidth)
				//{
				//	col = col*_RimColor;
				//}

				//alpha 渐变
				//if(r<_MinRadius + _RimWidth)
				//{
				//	col.a = smoothstep(0,_RimWidth,(r - _MinRadius )) + 0.4;
				//}
				//if(r >_MaxRadius - _RimWidth)
				//{
				//	col.a = smoothstep(0,_RimWidth,(_MaxRadius - r))+ 0.4;
				//}				

				return col;
			}
			ENDCG
		}
	}
}
