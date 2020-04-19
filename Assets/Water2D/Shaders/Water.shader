

Shader "Water/Water_Simple" {
Properties {    
    _MainTex ("Texture", 2D) = "white" { }    
   // _Color ("Main color", Color) = (1,1,1,1)
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

	_Stroke ("Stroke alpha", Range(0,1)) = 0.1
	//_StrokeColor ("Stroke color", Color) = (1,1,1,1)

	_RelativeRefractionIndex("Relative Refraction Index", Range(0.0, 1.0)) = 0.67
	[PowerSlider(5)]_Distance("Distance", Range(0.0, 100.0)) = 10.0

}
/// <summary>
/// Multiple metaball shader.

/// </summary>
SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	
	Cull Back
	ZWrite On
	ZTest LEqual
	ColorMask RGB

	GrabPass{ "_GrabTexture" }
    Pass {
    Blend SrcAlpha OneMinusSrcAlpha
	//Blend OneMinusDstColor One
	//Blend DstColor Zero
  // Blend One One // Additive
  // Blend One OneMinusSrcAlpha
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag	
	#include "UnityCG.cginc"	
	//float4 _Color;
	sampler2D _MainTex;	
	fixed _Cutoff;
	fixed _Stroke;
	//half4 _StrokeColor;
	float2 _screenPos;
	

	float4 _CameraDepthTexture_TexelSize;

	struct appdata {
		half4 vertex                : POSITION;
		half4 texcoord              : TEXCOORD0;
		half3 normal                : NORMAL;
	};
	sampler2D _GrabTexture;
	float _RelativeRefractionIndex;
	float _Distance;

	struct v2f {
	    float4  pos : SV_POSITION;
	    float2  uv : TEXCOORD0;
		half3	normal:TEXCOORD1;
		float3	worldpos:TEXCOORD2;
	};	



	float4 _MainTex_ST;		

	v2f vert (appdata v){
	    v2f o;
	    o.pos = UnityObjectToClipPos (v.vertex);
	    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
		o.worldpos = mul(unity_ObjectToWorld, v.vertex);
		o.normal = UnityObjectToWorldNormal(v.normal);

				
		
	    return o;
	};


	

	half4 frag (v2f i) : SV_Target{
		half4 texcol= tex2D(_MainTex, i.uv);
		//half4 finalColor = texcol;

	
		clip(texcol.a - _Cutoff);

		half3 color = texcol.rgb;

		if (texcol.a < _Stroke) {
			//texcol.r = lerp(color.r, 1, 0.3);//_StrokeColor;
			//texcol.g = lerp(color.g, 1, 0.3);
			//texcol.b = lerp(color.b, 1, 0.3);
			texcol.rgb = color + 0.2;			
			texcol.a = 0.7;
			//texcol *= _StrokeColor;
		} else {
			
			half3 viewDir = normalize(i.worldpos - _WorldSpaceCameraPos.xyz);
			// 屈折方向を求める
			half3 refractDir = refract(viewDir, i.normal, _RelativeRefractionIndex);
			// 屈折方向の先にある位置をサンプリング位置とする
			half3 samplingPos = i.worldpos + refractDir * _Distance;
			// サンプリング位置をプロジェクション変換
			half4 samplingScreenPos = mul(UNITY_MATRIX_VP, half4(samplingPos, 1.0));
			// ビューポート座標系に変換
			i.uv = (samplingScreenPos.xy / samplingScreenPos.w) * 0.5 + 0.5;
#if UNITY_UV_STARTS_AT_TOP
			i.uv.y = 1.0 - i.uv.y;
#endif
			texcol = tex2D(_GrabTexture, i.uv) * 0.5 + half4(color, 1);
			
		}
					
		
		
	 	return texcol;
	 	
	   
	}




	ENDCG

    }
}
Fallback "VertexLit"
} 