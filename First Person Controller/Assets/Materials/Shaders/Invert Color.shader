// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "GrabPassInvert"
{
	Properties
	{
		_MainTex("Alpha (A) only", 2D) = "white" {}
	}
	SubShader
	{
		
		// Draw ourselves after all opaque geometry
		Tags { "Queue" = "Overlay" "Rendertype" = "Transparent" "IgnoreProjector" = "True"}

		// Grab the screen behind the object into _BackgroundTexture
		GrabPass
		{
			"_BackgroundTexture"
		}

		// Render the object with the texture generated above, and invert the colors
		Pass
		{
		Zwrite Off
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 grabPos : TEXCOORD0;
				float4 uv : TEXCOORD1;
				float4 pos : SV_POSITION;
				float4 pos1 : POSITION1;
			};

			v2f vert(appdata_base v) {
				v2f o;
				// use UnityObjectToClipPos from UnityCG.cginc to calculate 
				// the clip-space of the vertex
				o.pos = UnityObjectToClipPos(v.vertex);
				// use ComputeGrabScreenPos function from UnityCG.cginc
				// to get the correct texture coordinate
				o.grabPos = ComputeGrabScreenPos(o.pos);
				o.uv = float4(v.texcoord.xy, 0, 0);
				o.pos1 = UnityObjectToClipPos(v.vertex);
				return o;
			}

			sampler2D _BackgroundTexture;
			sampler2D _MainTex;
			half4 frag(v2f i) : SV_Target
			{
				half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
				bgcolor.a = 1 - tex2D(_MainTex, i.uv);
				return 1 - bgcolor;
			}
			ENDCG
		}

	}
}