Shader "Custom/VFX/CubeParticleShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_EmissionTex("Emission", 2D) = "transparant" {}
		_Color("Border Color", Color) = (1,0,0,1)
		_Emission("Emission Color", Color) = (1,0,0,1)
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent"
			"Ignore Projector" = "True"}
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _EmissionTex;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_EmissionTex;
			};

			fixed4 _Color;
			fixed4 _Emission;

			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
				UNITY_INSTANCING_BUFFER_END(Props)

				void surf(Input IN, inout SurfaceOutputStandard o)
				{
				// Albedo comes from a texture tinted by color

				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 e = tex2D(_EmissionTex, IN.uv_EmissionTex) * _Emission;

				if (any(c.rgb > fixed3(0.95, 0.95, 0.95))) {                     //if the texture color is from white
					c.rgb = _Color.rgb;
				}

				//then color it by using the _Color property
				o.Albedo = c.rgb; //Apply the result to the surface shader
				o.Emission = e.rgb * e.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
