﻿Shader "Custom/Screen"
{
	SubShader{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}
		Pass {

			ColorMask 0

			Stencil
			{
				Ref 3
				Comp always
				Pass replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			struct appdata
			{
				float4 vertex : POSITION;
			};
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			half4 frag(v2f i) : SV_Target
			{
				return half4(1,1,1,0.5);
			}
			ENDCG
		}
	}
}