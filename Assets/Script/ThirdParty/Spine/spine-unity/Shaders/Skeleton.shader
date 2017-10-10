Shader "Spine/Skeleton" {
    Properties
    {
        _Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.1
        [NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Alpha("Alpha", Range(0,1)) = 1.0
        _Blender("Blender", Range(0, 1)) = 0.0
        _BlendMode("Blend Mode", Int) = 0
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Fog{ Mode Off }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex       : POSITION;
                float2 texcoord0    : TEXCOORD0;
                float4 vertexColor  : COLOR;
            };

            struct v2f
            {
                float4 pos          : SV_POSITION;
                float2 uv0          : TEXCOORD0;
                float4 vertexColor  : COLOR;
            };

            sampler2D _MainTex;
            fixed _Cutoff;
            fixed4 _Color;
            fixed _Alpha;
            fixed _Blender;
            fixed _BlendMode;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 rawColor = tex2D(_MainTex, i.uv0) * i.vertexColor;
                float alpha = rawColor.a * _Alpha;
                clip(alpha - _Cutoff);

                float gray = 0.299 * rawColor.r + 0.587 * rawColor.g + 0.114 * rawColor.b;
                fixed scalar = lerp(gray, 1, _BlendMode);
                fixed4 finalColor = lerp(rawColor, scalar * _Color, _Blender);
                finalColor.a = alpha;

                return finalColor;
            }
            ENDCG
        }
    }
}
