// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/RefractiveParticle"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MainColor("MainColor", Color) = (0,0,0,0)
		_Refraction("Refraction", Float) = 0
		_MainTexture("MainTexture", 2D) = "white" {}
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ "RefractionGrab1" }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 texcoord_0;
			float4 screenPos;
			float3 worldPos;
			float2 texcoord_1;
		};

		uniform float4 _MainColor;
		uniform sampler2D _MainTexture;
		uniform sampler2D RefractionGrab1;
		uniform float _ChromaticAberration;
		uniform float _Refraction;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.0000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( RefractionGrab1, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( RefractionGrab1, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( RefractionGrab1, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			float temp_output_88_0 = ( tex2D( _MainTexture,i.texcoord_1).a - 0.01 );
				color.rgb = color.rgb + Refraction( i, o, ( ( 0.0 * temp_output_88_0 ) * _Refraction ), _ChromaticAberration ) * ( 1 - color.a );
				color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			o.Albedo = _MainColor.rgb;
			float temp_output_88_0 = ( tex2D( _MainTexture,i.texcoord_0).a - 0.01 );
			o.Alpha = ( _MainColor.a * ( 0.0 * temp_output_88_0 ) );
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
		CGPROGRAM
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#pragma surface surf Standard alpha:fade keepalpha finalcolor:RefractionF vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1079;705;431.1487;568.1224;1.3;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;73;-790.3446,-249.7912;Float;True;Property;_MainTexture;MainTexture;13;0;Assets/Textures/Particles/FogParticle.png;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-795.3197,-37.97432;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;84;-417.6427,45.37881;Float;False;Constant;_Float0;Float 0;7;0;0.01;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;74;-504.7201,-185.7369;Float;True;Property;_TextureSample1;Texture Sample 1;14;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;88;-152.4406,-55.82238;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;86;-124.4412,-210.4216;Float;False;Constant;_Float1;Float 1;7;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;81;275.0598,79.67632;Float;False;Property;_Refraction;Refraction;6;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.ColorNode;31;124.9718,-354.8441;Float;False;Property;_MainColor;MainColor;3;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;131.4518,-172.0259;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;259.6597,-69.52511;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1137.311,-809.8432;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-1130.613,-711.9443;Float;False;Constant;_Float5;Float 5;5;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1334.778,-545.3203;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;50;-1816.285,-704.8439;Float;False;Property;_BorderThickness;BorderThickness;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-1374.418,-808.7436;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1551.818,-757.3438;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;-1780.739,-897.4416;Float;False;False;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;60;-938.2147,-617.7443;Float;False;Property;_BorderIntencity;BorderIntencity;2;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-1401.114,-687.5441;Float;False;Property;_BorderSharpness;BorderSharpness;7;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;559.2097,-479.1718;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;76;-404.4754,-738.6111;Float;False;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;52;-1576.635,-870.4415;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;83;176.757,457.3739;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;87;413.5602,439.1786;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;474.1597,-2.023578;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;583.6555,-351.4917;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;756.2546,-346.0916;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;389.0519,-178.4262;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;59;-934.8128,-755.5441;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SurfaceDepthNode;49;-1776.119,-623.6437;Float;False;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;58;-1129.813,-634.2442;Float;False;Constant;_Float7;Float 7;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;14.87209,-713.2556;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;79;-396.6441,-557.4896;Float;False;Constant;_EmissionAmount;EmissionAmount;14;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.ColorNode;47;741.1599,-554.0628;Float;False;Property;_Emission;Emission;7;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;709.1985,-156.7001;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/RefractiveParticle;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;1;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;74;0;73;0
WireConnection;74;1;75;0
WireConnection;88;0;74;4
WireConnection;88;1;84;0
WireConnection;89;0;86;0
WireConnection;89;1;88;0
WireConnection;82;1;88;0
WireConnection;56;0;53;0
WireConnection;56;1;54;0
WireConnection;61;0;59;0
WireConnection;61;1;60;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;51;0;50;0
WireConnection;51;1;49;0
WireConnection;52;0;48;0
WireConnection;80;0;82;0
WireConnection;80;1;81;0
WireConnection;90;0;31;4
WireConnection;90;1;89;0
WireConnection;59;0;56;0
WireConnection;59;1;57;0
WireConnection;59;2;58;0
WireConnection;64;0;76;0
WireConnection;64;1;79;0
WireConnection;0;0;31;0
WireConnection;0;8;80;0
WireConnection;0;9;90;0
ASEEND*/
//CHKSM=2121EAA715FB369A4064B329E3A203F97CCF777A