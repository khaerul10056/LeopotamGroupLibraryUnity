//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

Shader "LeopotamGroup/FX/ScreenFade" {
	Properties {
	}

	SubShader {
		Tags { "RenderType" = "Transparent" "Queue"="Overlay" "LightMode"="ForwardBase" "IgnoreProjector" = "True" }
		LOD 100
		Lighting Off
		ZWrite Off
		ZTest Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityCG.cginc"

		struct v2f {
			float4 pos : SV_POSITION;
			fixed4 color : TEXCOORD0;
		};


		v2f vert (appdata_full v) {
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.color = v.color;
			return o;
		}
		ENDCG

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest		
			fixed4 frag (v2f i) : SV_Target {
				return i.color;
			}
			ENDCG 
		}
	}
	Fallback Off
}