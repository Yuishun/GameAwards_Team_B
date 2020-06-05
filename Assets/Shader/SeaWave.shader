// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32688,y:32327,varname:node_2865,prsc:2|diff-7851-OUT,spec-358-OUT,gloss-1813-OUT,normal-136-RGB,alpha-6514-OUT,voffset-2105-OUT;n:type:ShaderForge.SFN_Slider,id:358,x:32007,y:32072,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32007,y:32156,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8,max:1;n:type:ShaderForge.SFN_Tex2d,id:6928,x:31975,y:32972,ptovrint:False,ptlb:WaveHeightTex,ptin:_WaveHeightTex,varname:node_6928,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:441254493321ded4683c5393718854b1,ntxv:0,isnm:False|UVIN-1376-UVOUT;n:type:ShaderForge.SFN_Sin,id:2757,x:31563,y:32660,varname:node_2757,prsc:2|IN-4560-T;n:type:ShaderForge.SFN_Panner,id:1376,x:31745,y:32820,varname:node_1376,prsc:2,spu:0,spv:0.5|UVIN-2654-UVOUT,DIST-4286-OUT;n:type:ShaderForge.SFN_Time,id:4560,x:30953,y:32652,varname:node_4560,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:1597,x:32387,y:33094,prsc:2,pt:False;n:type:ShaderForge.SFN_RemapRange,id:4286,x:31763,y:32660,varname:node_4286,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-2757-OUT;n:type:ShaderForge.SFN_Multiply,id:5613,x:32377,y:32956,varname:node_5613,prsc:2|A-3986-OUT,B-1597-OUT;n:type:ShaderForge.SFN_Slider,id:7031,x:31685,y:32595,ptovrint:False,ptlb:WaveWidth,ptin:_WaveWidth,varname:node_7031,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:2;n:type:ShaderForge.SFN_Slider,id:197,x:32180,y:32894,ptovrint:False,ptlb:WaveHeight,ptin:_WaveHeight,varname:node_197,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:2;n:type:ShaderForge.SFN_Multiply,id:6961,x:32526,y:32835,varname:node_6961,prsc:2|A-197-OUT,B-5613-OUT;n:type:ShaderForge.SFN_Multiply,id:8515,x:32180,y:32742,varname:node_8515,prsc:2|A-7031-OUT,B-4286-OUT;n:type:ShaderForge.SFN_Append,id:9860,x:32343,y:32690,varname:node_9860,prsc:2|A-1762-OUT,B-1762-OUT,C-8515-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1762,x:32180,y:32690,ptovrint:False,ptlb:Wavedirection(xy),ptin:_Wavedirectionxy,varname:node_1762,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:2105,x:32526,y:32690,varname:node_2105,prsc:2|A-9860-OUT,B-6961-OUT;n:type:ShaderForge.SFN_Add,id:3986,x:32190,y:32972,varname:node_3986,prsc:2|A-6928-G,B-2241-OUT;n:type:ShaderForge.SFN_TexCoord,id:2654,x:31140,y:32718,varname:node_2654,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector2,id:6308,x:31149,y:32873,varname:node_6308,prsc:2,v1:0.5,v2:0.5;n:type:ShaderForge.SFN_Distance,id:5041,x:31319,y:32811,varname:node_5041,prsc:2|A-2654-UVOUT,B-6308-OUT;n:type:ShaderForge.SFN_Multiply,id:4874,x:31308,y:32953,varname:node_4874,prsc:2|A-5041-OUT,B-3565-OUT;n:type:ShaderForge.SFN_Vector1,id:3565,x:31140,y:32976,varname:node_3565,prsc:2,v1:5;n:type:ShaderForge.SFN_Slider,id:2994,x:30782,y:33129,ptovrint:False,ptlb:sircleSpeed,ptin:_sircleSpeed,varname:node_2994,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:2723,x:31109,y:33184,varname:node_2723,prsc:2|A-8480-OUT,B-2994-OUT;n:type:ShaderForge.SFN_Multiply,id:4170,x:31578,y:33024,varname:node_4170,prsc:2|A-5041-OUT,B-1497-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1497,x:31578,y:33162,ptovrint:False,ptlb:WaveTop,ptin:_WaveTop,varname:node_1497,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_OneMinus,id:5,x:31308,y:33081,varname:node_5,prsc:2|IN-4874-OUT;n:type:ShaderForge.SFN_Add,id:2497,x:31301,y:33217,varname:node_2497,prsc:2|A-5-OUT,B-2723-OUT;n:type:ShaderForge.SFN_Sin,id:9562,x:31508,y:33228,varname:node_9562,prsc:2|IN-2497-OUT;n:type:ShaderForge.SFN_Smoothstep,id:2129,x:31745,y:32960,varname:node_2129,prsc:2|A-8152-OUT,B-7137-OUT,V-4170-OUT;n:type:ShaderForge.SFN_Vector1,id:7137,x:31578,y:32976,varname:node_7137,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:8152,x:31578,y:32930,varname:node_8152,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Abs,id:3776,x:31693,y:33228,varname:node_3776,prsc:2|IN-9562-OUT;n:type:ShaderForge.SFN_OneMinus,id:743,x:31774,y:33092,varname:node_743,prsc:2|IN-2129-OUT;n:type:ShaderForge.SFN_Multiply,id:7123,x:32011,y:33126,varname:node_7123,prsc:2|A-743-OUT,B-4601-OUT;n:type:ShaderForge.SFN_Power,id:4601,x:31856,y:33217,varname:node_4601,prsc:2|VAL-3776-OUT,EXP-1348-OUT;n:type:ShaderForge.SFN_Vector1,id:1348,x:31693,y:33338,varname:node_1348,prsc:2,v1:20;n:type:ShaderForge.SFN_Panner,id:3471,x:31319,y:32534,varname:node_3471,prsc:2,spu:0,spv:0.1|UVIN-2654-UVOUT,DIST-4560-T;n:type:ShaderForge.SFN_Tex2d,id:136,x:31511,y:32499,ptovrint:False,ptlb:Wavenormal,ptin:_Wavenormal,varname:node_136,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:85031e7fb77282d4ca28d4dd251e3e0b,ntxv:3,isnm:False|UVIN-6509-OUT;n:type:ShaderForge.SFN_Color,id:2864,x:31516,y:32123,ptovrint:False,ptlb:color1,ptin:_color1,varname:node_2864,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.3,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:748,x:31516,y:32283,ptovrint:False,ptlb:color2,ptin:_color2,varname:node_748,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.6,c2:0.7,c3:1,c4:1;n:type:ShaderForge.SFN_Fresnel,id:1545,x:31889,y:32330,varname:node_1545,prsc:2|NRM-3460-OUT,EXP-5493-OUT;n:type:ShaderForge.SFN_Vector1,id:5493,x:31685,y:32456,varname:node_5493,prsc:2,v1:1;n:type:ShaderForge.SFN_NormalVector,id:3460,x:31685,y:32320,prsc:2,pt:False;n:type:ShaderForge.SFN_Lerp,id:7851,x:32085,y:32281,varname:node_7851,prsc:2|A-2864-RGB,B-748-RGB,T-1545-OUT;n:type:ShaderForge.SFN_Multiply,id:2241,x:32190,y:33104,varname:node_2241,prsc:2|A-7123-OUT,B-5687-OUT;n:type:ShaderForge.SFN_Vector1,id:5687,x:32011,y:33238,varname:node_5687,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Tex2d,id:7164,x:32286,y:33363,ptovrint:False,ptlb:AlphaTexture,ptin:_AlphaTexture,varname:node_7164,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e53bd3431cea0164ba470648f79f7a19,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9446,x:32492,y:33312,varname:node_9446,prsc:2|A-1248-OUT,B-7164-A;n:type:ShaderForge.SFN_Vector1,id:1248,x:32286,y:33292,varname:node_1248,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Frac,id:6509,x:31331,y:32386,varname:node_6509,prsc:2|IN-3471-UVOUT;n:type:ShaderForge.SFN_Subtract,id:8480,x:30954,y:32933,varname:node_8480,prsc:2|A-1176-OUT,B-4560-T;n:type:ShaderForge.SFN_Vector1,id:1176,x:30908,y:32873,varname:node_1176,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector2,id:5225,x:31182,y:33550,varname:node_5225,prsc:2,v1:0,v2:1;n:type:ShaderForge.SFN_TexCoord,id:6164,x:31182,y:33400,varname:node_6164,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Distance,id:1733,x:31391,y:33388,varname:node_1733,prsc:2|A-6164-UVOUT,B-5225-OUT;n:type:ShaderForge.SFN_Vector2,id:3469,x:31182,y:33708,varname:node_3469,prsc:2,v1:0.5,v2:1;n:type:ShaderForge.SFN_Distance,id:977,x:31391,y:33508,varname:node_977,prsc:2|A-6164-UVOUT,B-569-OUT;n:type:ShaderForge.SFN_Vector2,id:569,x:31182,y:33629,varname:node_569,prsc:2,v1:1,v2:1;n:type:ShaderForge.SFN_Slider,id:2249,x:31070,y:33877,ptovrint:False,ptlb:Horizon,ptin:_Horizon,varname:node_2249,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.9900173,max:1;n:type:ShaderForge.SFN_Time,id:7926,x:31647,y:32746,varname:node_7926,prsc:2;n:type:ShaderForge.SFN_Distance,id:6530,x:31391,y:33629,varname:node_6530,prsc:2|A-6164-UVOUT,B-3469-OUT;n:type:ShaderForge.SFN_Multiply,id:2548,x:31622,y:33420,varname:node_2548,prsc:2|A-1733-OUT,B-977-OUT,C-6530-OUT;n:type:ShaderForge.SFN_Vector2,id:9618,x:31182,y:33787,varname:node_9618,prsc:2,v1:0,v2:1;n:type:ShaderForge.SFN_Multiply,id:8750,x:31391,y:33802,varname:node_8750,prsc:2|A-9618-OUT,B-2249-OUT;n:type:ShaderForge.SFN_Add,id:7077,x:31568,y:33749,varname:node_7077,prsc:2|A-3469-OUT,B-8750-OUT;n:type:ShaderForge.SFN_Distance,id:6243,x:31731,y:33727,varname:node_6243,prsc:2|A-6164-UVOUT,B-7077-OUT;n:type:ShaderForge.SFN_Multiply,id:1605,x:31788,y:33493,varname:node_1605,prsc:2|A-2548-OUT,B-1733-OUT,C-977-OUT,D-6243-OUT;n:type:ShaderForge.SFN_Subtract,id:5385,x:32099,y:33574,varname:node_5385,prsc:2|A-2491-OUT,B-1605-OUT;n:type:ShaderForge.SFN_Vector1,id:2491,x:32095,y:33445,varname:node_2491,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:1212,x:32410,y:33546,varname:node_1212,prsc:2|A-5385-OUT,B-391-OUT;n:type:ShaderForge.SFN_Step,id:391,x:32328,y:33699,varname:node_391,prsc:2|A-2601-OUT,B-5385-OUT;n:type:ShaderForge.SFN_Multiply,id:6514,x:32621,y:33546,varname:node_6514,prsc:2|A-9446-OUT,B-1212-OUT,C-5385-OUT;n:type:ShaderForge.SFN_Slider,id:2601,x:31957,y:33722,ptovrint:False,ptlb:BlurryHorizon,ptin:_BlurryHorizon,varname:_Horizon_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.15,max:0.25;n:type:ShaderForge.SFN_Time,id:4057,x:31711,y:32810,varname:node_4057,prsc:2;proporder:358-1813-6928-7031-197-1762-2994-1497-136-2864-748-7164-2249-2601;pass:END;sub:END;*/

Shader "Shader Forge/SeaWave" {
    Properties {
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0.8
        _WaveHeightTex ("WaveHeightTex", 2D) = "white" {}
        _WaveWidth ("WaveWidth", Range(0, 2)) = 0
        _WaveHeight ("WaveHeight", Range(0, 2)) = 1
        _Wavedirectionxy ("Wavedirection(xy)", Float ) = 0
        _sircleSpeed ("sircleSpeed", Range(0, 1)) = 1
        _WaveTop ("WaveTop", Float ) = 1
        _Wavenormal ("Wavenormal", 2D) = "bump" {}
        _color1 ("color1", Color) = (0,0.3,0.5,1)
        _color2 ("color2", Color) = (0.6,0.7,1,1)
        _AlphaTexture ("AlphaTexture", 2D) = "white" {}
        _Horizon ("Horizon", Range(0, 1)) = 0.9900173
        _BlurryHorizon ("BlurryHorizon", Range(0, 0.25)) = 0.15
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _WaveHeightTex; uniform float4 _WaveHeightTex_ST;
            uniform float _WaveWidth;
            uniform float _WaveHeight;
            uniform float _Wavedirectionxy;
            uniform float _sircleSpeed;
            uniform float _WaveTop;
            uniform sampler2D _Wavenormal; uniform float4 _Wavenormal_ST;
            uniform float4 _color1;
            uniform float4 _color2;
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            uniform float _Horizon;
            uniform float _BlurryHorizon;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                UNITY_FOG_COORDS(7)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD8;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_4560 = _Time;
                float node_4286 = (sin(node_4560.g)*0.5+0.5);
                float2 node_1376 = (o.uv0+node_4286*float2(0,0.5));
                float4 _WaveHeightTex_var = tex2Dlod(_WaveHeightTex,float4(TRANSFORM_TEX(node_1376, _WaveHeightTex),0.0,0));
                float node_5041 = distance(o.uv0,float2(0.5,0.5));
                v.vertex.xyz += (float3(_Wavedirectionxy,_Wavedirectionxy,(_WaveWidth*node_4286))+(_WaveHeight*((_WaveHeightTex_var.g+(((1.0 - smoothstep( 0.7, 1.0, (node_5041*_WaveTop) ))*pow(abs(sin(((1.0 - (node_5041*5.0))+((1.0-node_4560.g)*_sircleSpeed)))),20.0))*0.5))*v.normal)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_4560 = _Time;
                float2 node_6509 = frac((i.uv0+node_4560.g*float2(0,0.1)));
                float4 _Wavenormal_var = tex2D(_Wavenormal,TRANSFORM_TEX(node_6509, _Wavenormal));
                float3 normalLocal = _Wavenormal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float3 diffuseColor = lerp(_color1.rgb,_color2.rgb,pow(1.0-max(0,dot(i.normalDir, viewDirection)),1.0)); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                float node_1733 = distance(i.uv0,float2(0,1));
                float node_977 = distance(i.uv0,float2(1,1));
                float2 node_3469 = float2(0.5,1);
                float node_5385 = (1.0-((node_1733*node_977*distance(i.uv0,node_3469))*node_1733*node_977*distance(i.uv0,(node_3469+(float2(0,1)*_Horizon)))));
                fixed4 finalRGBA = fixed4(finalColor,((0.5*_AlphaTexture_var.a)*(node_5385*step(_BlurryHorizon,node_5385))*node_5385));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _WaveHeightTex; uniform float4 _WaveHeightTex_ST;
            uniform float _WaveWidth;
            uniform float _WaveHeight;
            uniform float _Wavedirectionxy;
            uniform float _sircleSpeed;
            uniform float _WaveTop;
            uniform sampler2D _Wavenormal; uniform float4 _Wavenormal_ST;
            uniform float4 _color1;
            uniform float4 _color2;
            uniform sampler2D _AlphaTexture; uniform float4 _AlphaTexture_ST;
            uniform float _Horizon;
            uniform float _BlurryHorizon;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_4560 = _Time;
                float node_4286 = (sin(node_4560.g)*0.5+0.5);
                float2 node_1376 = (o.uv0+node_4286*float2(0,0.5));
                float4 _WaveHeightTex_var = tex2Dlod(_WaveHeightTex,float4(TRANSFORM_TEX(node_1376, _WaveHeightTex),0.0,0));
                float node_5041 = distance(o.uv0,float2(0.5,0.5));
                v.vertex.xyz += (float3(_Wavedirectionxy,_Wavedirectionxy,(_WaveWidth*node_4286))+(_WaveHeight*((_WaveHeightTex_var.g+(((1.0 - smoothstep( 0.7, 1.0, (node_5041*_WaveTop) ))*pow(abs(sin(((1.0 - (node_5041*5.0))+((1.0-node_4560.g)*_sircleSpeed)))),20.0))*0.5))*v.normal)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_4560 = _Time;
                float2 node_6509 = frac((i.uv0+node_4560.g*float2(0,0.1)));
                float4 _Wavenormal_var = tex2D(_Wavenormal,TRANSFORM_TEX(node_6509, _Wavenormal));
                float3 normalLocal = _Wavenormal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float3 diffuseColor = lerp(_color1.rgb,_color2.rgb,pow(1.0-max(0,dot(i.normalDir, viewDirection)),1.0)); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                float4 _AlphaTexture_var = tex2D(_AlphaTexture,TRANSFORM_TEX(i.uv0, _AlphaTexture));
                float node_1733 = distance(i.uv0,float2(0,1));
                float node_977 = distance(i.uv0,float2(1,1));
                float2 node_3469 = float2(0.5,1);
                float node_5385 = (1.0-((node_1733*node_977*distance(i.uv0,node_3469))*node_1733*node_977*distance(i.uv0,(node_3469+(float2(0,1)*_Horizon)))));
                fixed4 finalRGBA = fixed4(finalColor * ((0.5*_AlphaTexture_var.a)*(node_5385*step(_BlurryHorizon,node_5385))*node_5385),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform sampler2D _WaveHeightTex; uniform float4 _WaveHeightTex_ST;
            uniform float _WaveWidth;
            uniform float _WaveHeight;
            uniform float _Wavedirectionxy;
            uniform float _sircleSpeed;
            uniform float _WaveTop;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 posWorld : TEXCOORD4;
                float3 normalDir : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4560 = _Time;
                float node_4286 = (sin(node_4560.g)*0.5+0.5);
                float2 node_1376 = (o.uv0+node_4286*float2(0,0.5));
                float4 _WaveHeightTex_var = tex2Dlod(_WaveHeightTex,float4(TRANSFORM_TEX(node_1376, _WaveHeightTex),0.0,0));
                float node_5041 = distance(o.uv0,float2(0.5,0.5));
                v.vertex.xyz += (float3(_Wavedirectionxy,_Wavedirectionxy,(_WaveWidth*node_4286))+(_WaveHeight*((_WaveHeightTex_var.g+(((1.0 - smoothstep( 0.7, 1.0, (node_5041*_WaveTop) ))*pow(abs(sin(((1.0 - (node_5041*5.0))+((1.0-node_4560.g)*_sircleSpeed)))),20.0))*0.5))*v.normal)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _WaveHeightTex; uniform float4 _WaveHeightTex_ST;
            uniform float _WaveWidth;
            uniform float _WaveHeight;
            uniform float _Wavedirectionxy;
            uniform float _sircleSpeed;
            uniform float _WaveTop;
            uniform float4 _color1;
            uniform float4 _color2;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4560 = _Time;
                float node_4286 = (sin(node_4560.g)*0.5+0.5);
                float2 node_1376 = (o.uv0+node_4286*float2(0,0.5));
                float4 _WaveHeightTex_var = tex2Dlod(_WaveHeightTex,float4(TRANSFORM_TEX(node_1376, _WaveHeightTex),0.0,0));
                float node_5041 = distance(o.uv0,float2(0.5,0.5));
                v.vertex.xyz += (float3(_Wavedirectionxy,_Wavedirectionxy,(_WaveWidth*node_4286))+(_WaveHeight*((_WaveHeightTex_var.g+(((1.0 - smoothstep( 0.7, 1.0, (node_5041*_WaveTop) ))*pow(abs(sin(((1.0 - (node_5041*5.0))+((1.0-node_4560.g)*_sircleSpeed)))),20.0))*0.5))*v.normal)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float3 diffColor = lerp(_color1.rgb,_color2.rgb,pow(1.0-max(0,dot(i.normalDir, viewDirection)),1.0));
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
