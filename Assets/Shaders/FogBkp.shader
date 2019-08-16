// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fog"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Texture0("Texture 0", 2D) = "white" {}
		_Color2("Color 2", Color) = (0.5220588,0.9208925,1,0.528)
		_TimeScale("Time Scale", Float) = 0.02
		_Float9("Float 9", Float) = 1
		_Float1("Float 1", Float) = 1
		_Color0("Color 0", Color) = (0,0,0,0)
		_YTransparancy("YTransparancy", Float) = 0.1
		_EffectScale("Effect Scale", Float) = 0.1
		_tiling("tiling", Range( 0 , 0.1)) = 0.01930745
		_ZTransparancyOffset("ZTransparancyOffset", Float) = 0
		_Float3("Float 3", Float) = 1
		_Emission("Emission", Color) = (0,0,0,0)
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
			float4 screenPos;
			float eyeDepth;
			float3 worldPos;
		};

		uniform float4 _Color0;
		uniform float4 _Color2;
		uniform sampler2D _CameraDepthTexture;
		uniform float _Float1;
		uniform float _Float3;
		uniform float _Float9;
		uniform float4 _Emission;
		uniform float _ZTransparancyOffset;
		uniform float _YTransparancy;
		uniform sampler2D _Texture0;
		uniform float _TimeScale;
		uniform float _tiling;
		uniform float _EffectScale;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
			float3 worldPosition = mul(unity_ObjectToWorld, v.vertex);
			float4 appendResult43 = float4( worldPosition.x , worldPosition.z , 0 , 0 );
			float4 appendResult39 = float4( 0 , ( tex2Dlod( _Texture0,( ( ( _Time.y * _TimeScale ) * appendResult43 ) * _tiling )) * _EffectScale ).x , 0 , 0 );
			v.vertex.xyz += appendResult39.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float eyeDepth52 = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
			float temp_output_61_0 = ( clamp( ( ( 1.0 - ( eyeDepth52 - ( _Float1 + i.eyeDepth ) ) ) * _Float3 ) , 0.0 , 1.0 ) * _Float9 );
			float4 temp_output_64_0 = ( _Color2 * temp_output_61_0 );
			o.Albedo = ( _Color0 + temp_output_64_0 ).rgb;
			o.Emission = ( _Emission + temp_output_64_0 ).rgb;
			o.Alpha = ( ( ( _ZTransparancyOffset + i.worldPos.y ) * _YTransparancy ) + ( _Color2.a * temp_output_61_0 ) );
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
				Input customInputData;
				vertexDataFunc( v, customInputData );
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
0;92;1157;655;1515.86;1205.138;3.030118;True;True
Node;AmplifyShaderEditor.SurfaceDepthNode;49;-1346.116,-504.5443;Float;False;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;50;-1403.114,-585.7445;Float;False;Property;_Float1;Float 1;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;-1350.736,-778.3422;Float;False;False;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;52;-1146.632,-751.342;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1121.815,-638.2444;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;8;-1449.949,199.211;Float;False;Property;_TimeScale;Time Scale;2;0;0.02;0;0;FLOAT
Node;AmplifyShaderEditor.TimeNode;6;-1460.849,11.21041;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;29;-1459.219,295.9434;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-944.4167,-689.6442;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.AppendNode;43;-1169.246,270.81;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1176.708,114.4543;Float;False;0;FLOAT;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-953.5127,-571.6447;Float;False;Property;_Float3;Float 3;7;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;55;-775.8132,-764.3442;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1000.224,216.4428;Float;False;0;FLOAT;0.0;False;1;FLOAT4;0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;34;-1233.087,436.5308;Float;False;Property;_tiling;tiling;4;0;0.01930745;0;0.1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;58;-589.4127,-539.0446;Float;False;Constant;_Float7;Float 7;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-590.2123,-616.7447;Float;False;Constant;_Float5;Float 5;5;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-587.8106,-714.6436;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;-691.2433,-316.6899;Float;False;Property;_ZTransparancyOffset;ZTransparancyOffset;6;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;1;-915.2452,-15.68959;Float;True;Property;_Texture0;Texture 0;0;0;Assets/Addons/AmplifyShaderEditor/Examples/Community/Dissolve Burn/dissolve-guide.png;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-840.2863,315.1307;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;60;-404.2152,-520.9448;Float;False;Property;_Float9;Float 9;2;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;59;-401.8134,-659.3445;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;36;-689.6865,-233.5686;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;30;-570.7183,-53.75691;Float;False;Property;_YTransparancy;YTransparancy;3;0;0.1;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-599.288,97.00613;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;33;-502.3855,335.2309;Float;False;Property;_EffectScale;Effect Scale;3;0;0.1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-441.5435,-180.6899;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ColorNode;62;-315.3139,-851.2437;Float;False;Property;_Color2;Color 2;0;0;0.5220588,0.9208925,1,0.528;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-209.9115,-640.6434;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.ColorNode;47;-58.29473,-427.3248;Float;False;Property;_Emission;Emission;7;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-304.2862,-58.06901;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ColorNode;31;-54.62401,-248.4222;Float;False;Property;_Color0;Color 0;3;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;286.9371,-438.163;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;317.5494,-577.7579;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-232.5846,146.3304;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;521.9686,-238.5311;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;65;523.7057,-87.96777;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;69;508.1564,57.88617;Float;False;0;FLOAT;0.0;False;1;FLOAT;0;False;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;42;-26.84526,503.6105;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;66;49.55746,-952.6079;Float;False;Property;_Float11;Float 11;5;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.GrabScreenPosition;41;-272.944,464.8105;Float;False;True;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;40;-686.7856,590.1318;Float;False;FLOAT;0;FLOAT;0.0;False;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.AppendNode;39;-43.78573,146.831;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;712.5985,-156.7001;Float;False;True;2;Float;ASEMaterialInspector;Standard;Fog;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;52;0;48;0
WireConnection;51;0;50;0
WireConnection;51;1;49;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;43;0;29;1
WireConnection;43;1;29;3
WireConnection;9;0;6;2
WireConnection;9;1;8;0
WireConnection;55;0;53;0
WireConnection;14;0;9;0
WireConnection;14;1;43;0
WireConnection;56;0;55;0
WireConnection;56;1;54;0
WireConnection;35;0;14;0
WireConnection;35;1;34;0
WireConnection;59;0;56;0
WireConnection;59;1;57;0
WireConnection;59;2;58;0
WireConnection;2;0;1;0
WireConnection;2;1;35;0
WireConnection;46;0;44;0
WireConnection;46;1;36;2
WireConnection;61;0;59;0
WireConnection;61;1;60;0
WireConnection;38;0;46;0
WireConnection;38;1;30;0
WireConnection;67;0;62;4
WireConnection;67;1;61;0
WireConnection;64;0;62;0
WireConnection;64;1;61;0
WireConnection;32;0;2;0
WireConnection;32;1;33;0
WireConnection;70;0;47;0
WireConnection;70;1;64;0
WireConnection;65;0;31;0
WireConnection;65;1;64;0
WireConnection;69;0;38;0
WireConnection;69;1;67;0
WireConnection;42;0;41;0
WireConnection;39;1;32;0
WireConnection;0;0;65;0
WireConnection;0;2;70;0
WireConnection;0;9;69;0
WireConnection;0;11;39;0
ASEEND*/
//CHKSM=85099C4FDC9F785DB799F7DD1C134DFF875AAE09