// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/FogParticle"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_BorderIntencity("BorderIntencity", Float) = 1
		_BorderThickness("BorderThickness", Float) = 1
		_BorderSharpness("BorderSharpness", Float) = 1
		_MainTexture("MainTexture", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 vertexColor : COLOR;
			float4 screenPos;
			float eyeDepth;
			float2 texcoord_0;
		};

		uniform sampler2D _CameraDepthTexture;
		uniform float _BorderThickness;
		uniform float _BorderSharpness;
		uniform float _BorderIntencity;
		uniform sampler2D _MainTexture;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 temp_output_76_0 = i.vertexColor;
			o.Albedo = temp_output_76_0.rgb;
			o.Emission = ( i.vertexColor * 0.0 ).rgb;
			float eyeDepth52 = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
			o.Alpha = ( i.vertexColor.a * ( ( clamp( ( ( eyeDepth52 - ( _BorderThickness + i.eyeDepth ) ) * _BorderSharpness ) , 0.0 , 1.0 ) * _BorderIntencity ) * tex2D( _MainTexture,i.texcoord_0).a ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha vertex:vertexDataFunc 

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
0;92;1157;655;1849.466;1436.318;3.122504;True;True
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;-1240.337,-802.242;Float;False;False;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SurfaceDepthNode;49;-1235.717,-528.4441;Float;False;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;50;-1275.883,-609.6443;Float;False;Property;_BorderThickness;BorderThickness;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1011.416,-662.1442;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;52;-1036.233,-775.2419;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-834.0168,-713.544;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-860.7125,-592.3445;Float;False;Property;_BorderSharpness;BorderSharpness;7;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;58;-589.4127,-539.0446;Float;False;Constant;_Float7;Float 7;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-596.9106,-714.6436;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-590.2123,-616.7447;Float;False;Constant;_Float5;Float 5;5;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;59;-394.4133,-660.3445;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;60;-397.8152,-522.5447;Float;False;Property;_BorderIntencity;BorderIntencity;2;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-486.0206,-116.2743;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;73;-479.4455,-328.0911;Float;True;Property;_MainTexture;MainTexture;13;0;Assets/Textures/Particles/FogParticle.png;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SamplerNode;74;-195.4196,-312.0369;Float;True;Property;_TextureSample1;Texture Sample 1;14;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-85.67548,-444.4207;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;79;-111.8444,-538.3913;Float;False;Constant;_EmissionAmount;EmissionAmount;14;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;256.2094,-205.4713;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;76;93.62501,-762.6116;Float;False;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;672.856,-414.8911;Float;False;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.ColorNode;31;478.5719,-615.8442;Float;False;Property;_MainColor;MainColor;3;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;251.3721,-323.2546;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;522.7556,-57.69143;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ColorNode;47;-167.0403,-742.1639;Float;False;Property;_Emission;Emission;7;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;712.5985,-156.7001;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/FogParticle;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;51;0;50;0
WireConnection;51;1;49;0
WireConnection;52;0;48;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;56;0;53;0
WireConnection;56;1;54;0
WireConnection;59;0;56;0
WireConnection;59;1;57;0
WireConnection;59;2;58;0
WireConnection;74;0;73;0
WireConnection;74;1;75;0
WireConnection;61;0;59;0
WireConnection;61;1;60;0
WireConnection;67;0;61;0
WireConnection;67;1;74;4
WireConnection;78;0;31;0
WireConnection;64;0;76;0
WireConnection;64;1;79;0
WireConnection;77;0;76;4
WireConnection;77;1;67;0
WireConnection;0;0;76;0
WireConnection;0;2;64;0
WireConnection;0;9;77;0
ASEEND*/
//CHKSM=4CDFAED512408FD6496F849132559AF15508F7B8