//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

Shader "LeopotamGroup/LazyGui/Font" {
    Properties {
        _MainTex ("Texture (RGB)", 2D) = "black" {}
        _ClipData ("clip-data", Vector) = (0,0,0,0)
        _ClipTrans ("clip-trans", Vector) = (0,0,0,0)
    }

    SubShader {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }

        LOD 100
        Fog { Mode Off }
        AlphaTest Off
        Lighting Off
        ZWrite Off
        Cull Off
        ColorMask RGB
        Blend SrcAlpha OneMinusSrcAlpha
        Offset -1, -1

        Pass {
            CGPROGRAM
            //#include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile __ LGUI_CLIP_RANGE
            //#pragma glsl_no_auto_normalization
            #pragma vertex vert
            #pragma fragment frag
     
            sampler2D _MainTex;
            float4 _MainTex_ST;

            #ifdef LGUI_CLIP_RANGE
            float4 _ClipData;
            float4 _ClipTrans;
            #endif

            struct app2v
			{
				float4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
     
            struct v2f {
                float4 pos : SV_POSITION;
                fixed2 uv : TEXCOORD0;
                fixed4 color : TEXCOORD1;

                #ifdef LGUI_CLIP_RANGE
                half2 clipPos : TEXCOORD2;
                #endif
            };

            v2f vert(app2v v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
                o.color = v.color;

                #ifdef LGUI_CLIP_RANGE
                o.clipPos = (mul(_Object2World, v.vertex).xy - _ClipTrans.xy) * _ClipData.xy;
                #endif

                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                fixed4 c = tex2D (_MainTex, i.uv).aaaa * i.color;

                #ifdef LGUI_CLIP_RANGE
                half2 soft = _ClipData.zw - abs(i.clipPos);
                c.a *= min(soft.x, soft.y);
                #endif

                return c;
            }
            ENDCG
        }
    }
    FallBack Off
}