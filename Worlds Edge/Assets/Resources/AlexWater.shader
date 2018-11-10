Shader "Lexdev/LowPoly/Water" 
{
	Properties
	{
		// Lighting
		_Color("Base Color", Color) = (0,0.5,0.7)
		_Opacity("Opacity", Range(0,1)) = 0.7
		_SpecularIntensity("Specular Intensity", Range(0,1)) = 0.6
		_SpecularSize("Specular Highlight Size", Range(0.01,10)) = 0.6
		// Waves
		_Length("Wave Length", Float) = 3.3
		_Stretch("Wave Stretch", Float) = 10
		_Speed("Wave Speed", Float) = 0.5
		_Height("Wave Height", Float) = 1
		//Other
		_Direction("Direction", Vector) = (-1,0,-1.65,0)
		[Toggle] _UseMath("Use math for noise generation", Float) = 0
		[NoScaleOffset] _NoiseTex("Noise Texture", 2D) = "white" {}
		_TexSize("TexSize", Float) = 64
		//Noise
		_NSpeed("Noise Speed", Float) = 1
		_NHeight("Noise Height", Float) = 0.2
		//Shore
		[Toggle] _ShoreBlend("Enable Shore", Float) = 0
		_ShoreColor("Shore Color", Color) = (1,1,1,1)
		_ShoreIntensity("Shore Intensity", Range(-1,1)) = 0
		_ShoreDistance("Shore Distance", Float) = 1
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
				#pragma target 5.0 
				#pragma vertex vert
				#pragma fragment frag
				#pragma shader_feature _SHOREBLEND_ON
				#pragma shader_feature _USEMATH_ON
				#pragma multi_compile_fog
				#include "UnityStandardUtils.cginc"
				#include "UnityLightingCommon.cginc"
				#include "noiseSimplex.cginc"
				//Object data
				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};
				//Vertex to fragment data
				struct v2f {
					float4 pos : SV_POSITION;
					fixed4 vertexLight : TEXCOORD1;
					UNITY_FOG_COORDS(0)
					#ifdef _SHOREBLEND_ON
					float4 screenPos : TEXCOORD3;
					#endif
				};
				//Variables
				half _NSpeed, _NHeight, _Opacity, _SpecularIntensity, _SpecularSize, _Speed, _Height, _Length, _Stretch;
				fixed4 _Color;
				half4 _Direction;
				//Noise map
				sampler2D _NoiseTex;
				half _TexSize;
				//Shore variables
				#ifdef _SHOREBLEND_ON
				sampler2D _CameraDepthTexture;
				half _ShoreIntensity, _ShoreDistance;
				fixed4 _ShoreColor;
				#endif

				//Return noise for specific vector2. Noise function or texture
				float noiseFunction(float2 uv) {
					#ifdef _USEMATH_ON
					return smoothstep(0, 1, tex2Dlod(_NoiseTex, float4(uv / _TexSize, 0, 0)).a) - 0.5;
					#else
					return smoothstep(0, 1, snoise(float4(uv / 3, 0, 0)) + 0.5) - 0.5;
					#endif
				}

				//Gerstner calculation based on position and phase
				void gerstner(inout float3 p, float phase) {
					float x = p.x*_Direction.x - p.z*_Direction.y;
					float z = p.z*_Direction.x + p.x*_Direction.y;
					float n = noiseFunction(float2(x / _Stretch, z / _Length + phase));
					p.y += _Height*n;
					p.xz -= n*_Direction.wz;
				}

				//Noise calculation for random vertex displacement
				float noise(float2 p, float phase) {
					float2 uv = float2(p.x, phase + p.y);
					#ifdef _USEMATH_ON
					return (snoise(float4(uv / 3, 0, 0)) - 0.5)*_NHeight * 0.7;
					#else
					return (tex2Dlod(_NoiseTex, float4(uv / _TexSize, 0, 0)).a - 0.5)*_NHeight;
					#endif
				}

				//Lighting calculation
				half4 lighting(half3 normal, half3 worldPos) {
					float3 lightDir = _WorldSpaceLightPos0.xyz; 
					half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));

					//Ambient light
					half3 ambient = max(0.0, ShadeSH9(half4(normal, 1.0)));

					//Fresnel calculation based on ambient light
					half dn = max(0.0, dot(worldViewDir, normal));
					half fresPower = 1 - dn;
					fresPower *= fresPower * fresPower;
					half3 fres = ambient * fresPower;

					//Primary lightsource (sun). Specular highlight
					half3 h = normalize(lightDir + worldViewDir);
					half nh = max(0.0, dot(normal, h));
					half specPower = pow(nh, 1 / _SpecularSize*128.0) * _SpecularIntensity;
					half3 spec = _LightColor0.rgb*specPower;

					//Final lighting output
					return fixed4(_Color*ambient + _LightColor0.rgb * (fres + spec), _Opacity*(1.0 + 0.2*fresPower + specPower));
				}

				v2f vert(appdata v) {
					v2f o;

					float4 pos0 = mul(unity_ObjectToWorld, v.vertex);

					//Get neighbours S
					float4 offs = float4(floor(v.uv),frac(v.uv)) * float4(1.0 / 10000.0, 1.0 / 10000.0, 10.0, 10.0) - 5.0;
					float4 p = v.vertex;
					p.xz -= offs.xz;
					float3 pos1 = mul(unity_ObjectToWorld, p).xyz;
					p.xz = v.vertex.xz - offs.yw;
					float3 pos2 = mul(unity_ObjectToWorld, p).xyz;

					//Calculate noise for all 3 vertices
					float phase = _Time[1] * _NSpeed;
					pos0.y += noise(pos0.xz, phase);
					pos1.y += noise(pos1.xz, phase);
					pos2.y += noise(pos2.xz, phase);

					//Calculate Gerstner for all three vertices
					phase = _Time[1] * _Speed;
					gerstner(pos0.xyz, phase);
					gerstner(pos1, phase);
					gerstner(pos2, phase);

					//Get normal of triangle
					half3 worldNormal = cross(pos1 - pos0.xyz, pos2 - pos0.xyz);
					worldNormal = normalize(worldNormal);
					//Calculate vertexIllumination for triangle
					o.vertexLight = lighting(worldNormal, (pos0.xyz + pos1 + pos2) / 3.0);

					o.pos = mul(UNITY_MATRIX_VP, pos0);

					#ifdef _SHOREBLEND_ON
					o.screenPos = ComputeNonStereoScreenPos(o.pos);
					o.screenPos.z = lerp(o.pos.w, mul(UNITY_MATRIX_V, pos0).z, unity_OrthoParams.w);
					#endif
					//Pass fog coordinates to pixel shader
					UNITY_TRANSFER_FOG(o,o.pos); 
					return o;
				}

				fixed4 frag(v2f i) : COLOR{
					fixed4 c = i.vertexLight;

					#ifdef _SHOREBLEND_ON
					float sceneZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
					float perpectiveZ = LinearEyeDepth(sceneZ);
					#if defined(UNITY_REVERSED_Z)
					sceneZ = 1 - sceneZ;
					#endif
					float orthoZ = sceneZ*(_ProjectionParams.y - _ProjectionParams.z) - _ProjectionParams.y;

					sceneZ = lerp(perpectiveZ, orthoZ, unity_OrthoParams.w);

					float diff = abs(sceneZ - i.screenPos.z) / _ShoreDistance;
					diff = smoothstep(_ShoreIntensity , 1 , diff);
					c = lerp(lerp(c, _ShoreColor, _ShoreColor.a), c, diff);
					#endif

					UNITY_APPLY_FOG(i.fogCoord, c);
					return c;
				}
			ENDCG
		}
	}
	Fallback "Legacy Shaders/VertexLit"
} 