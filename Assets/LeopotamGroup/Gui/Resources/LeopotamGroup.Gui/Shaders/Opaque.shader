//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

Shader "LeopotamGroup/Gui/Opaque" {
    Properties {
        _MainTex ("Color (RGB)", 2D) = "black" {}
    }

    SubShader {
        Tags { "Queue" = "Geometry" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }

        LOD 100
        Fog { Mode Off }
        Lighting Off
        Cull Off
        ColorMask RGB

        Pass {
            CGPROGRAM
            //#include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            //#pragma glsl_no_auto_normalization
            #pragma vertex vert
            #pragma fragment frag
     
            sampler2D _MainTex;
            sampler2D _AlphaTex;

            float4 _MainTex_ST;

            struct app2v {
                float4 vertex : POSITION;
                fixed2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };
     
            struct v2f {
                float4 pos : SV_POSITION;
                fixed2 uv : TEXCOORD0;
                fixed4 color : TEXCOORD1;
            };

            v2f vert(app2v v) {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : COLOR {
                return tex2D (_MainTex, i.uv) * i.color;
            }
            ENDCG
        }
    }
    FallBack Off
}