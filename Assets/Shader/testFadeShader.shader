Shader "Unlit/testFadeShader"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Alpha("Time", Range(0, 1)) = 0
		_SubTex("SubTex", 2D) = "white" {}
	}
		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}
			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Fog{ Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			
				struct appdata_t
				{
					float4 vertex   : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					half2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;
				fixed _Alpha;
				sampler2D _MainTex;
				uniform sampler2D _SubTex; uniform float4 _SubTex_ST;

				// 頂点シェーダーの基本
				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
	#ifdef UNITY_HALF_TEXEL_OFFSET
					OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1,1);
	#endif
					return OUT;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					half alphamain = tex2D(_MainTex, IN.texcoord).a;
					half alphasub = tex2D(_SubTex, IN.texcoord).a;
					alphamain = saturate(alphamain - (_Alpha * 2 - 1));
					alphasub  = saturate(alphasub  - (_Alpha * 2 - 1));
					half alpha = (alphamain + alphasub);
					return fixed4(_Color.r, _Color.g, _Color.b, alpha);
				}
				ENDCG
			}
		}

			FallBack "UI/Default"
}