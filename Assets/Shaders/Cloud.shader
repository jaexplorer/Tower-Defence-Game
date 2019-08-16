// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Cloud"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_FogClampValue("FogClampValue", Float) = 1
		_MainColor("MainColor", Color) = (0,0,0,0)
		_FogThickness("FogThickness", Range( 0 , 5)) = 1
		_Emission("Emission", Float) = 0
		_FogDensity("FogDensity", Range( 0 , 40)) = 1
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
		};

		uniform float4 _MainColor;
		uniform float _Emission;
		uniform sampler2D _CameraDepthTexture;
		uniform float _FogDensity;
		uniform float _FogThickness;
		uniform float _FogClampValue;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 temp_output_31_0 = _MainColor;
			o.Albedo = temp_output_31_0.rgb;
			o.Emission = ( _MainColor * _Emission ).rgb;
			float eyeDepth52 = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
			float temp_output_59_0 = clamp( ( ( ( eyeDepth52 - i.eyeDepth ) * _FogDensity ) + _FogThickness ) , 0.0 , _FogClampValue );
			o.Alpha = ( temp_output_59_0 * _MainColor.a );
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
563;453;922;498;764.5566;684.2547;1.9;True;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;-1064.537,-809.942;Float;False;False;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SurfaceDepthNode;49;-859.7161,-645.3439;Float;False;0;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;52;-860.4323,-782.9419;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-752.8137,-537.6447;Float;False;Property;_FogDensity;FogDensity;13;0;1;0;40;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-631.2162,-721.244;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-423.4115,-673.444;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;50;-600.9834,-429.9445;Float;False;Property;_FogThickness;FogThickness;8;0;1;0;5;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;92;-232.7558,-600.3582;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-237.2123,-434.5448;Float;False;Constant;_Float5;Float 5;5;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;58;-282.5127,-343.9437;Float;False;Property;_FogClampValue;FogClampValue;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.ColorNode;31;-65.62397,-180.722;Float;False;Property;_MainColor;MainColor;5;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;99;-12.65527,49.54257;Float;False;Property;_Emission;Emission;9;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;59;-30.71342,-495.5451;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;74;-185.232,272.9;Float;True;Property;_Mask;Mask;2;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-162.3553,509.8474;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.NoiseGeneratorNode;101;-648.9532,342.5422;Float;False;Simplex2D;0;FLOAT2;0,0;False;FLOAT
Node;AmplifyShaderEditor.SamplerNode;75;177.9244,280.7945;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;39.34521,226.0439;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;299.3462,-538.6576;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;102;-496.3535,226.5422;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SinTimeNode;84;1167.44,474.5462;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;60;-901.4149,41.8554;Float;False;Property;_NoiseIntencity;NoiseIntencity;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;350.4371,79.23677;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.AppendNode;106;-435.0538,-63.7572;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;387.3449,-56.05742;Float;False;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.AppendNode;105;-1318.454,269.6427;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-683.7234,528.2429;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;34;-1724.488,137.9311;Float;False;Property;_NoiseTiling;NoiseTiling;9;0;0.01930745;0;10;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;90;-528.4565,140.1415;Float;False;Property;_NoiseFlat;NoiseFlat;11;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-1356.986,-21.86917;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.WorldPosInputsNode;29;-1802.619,-34.35653;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-996.788,-189.3938;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-653.3847,-163.2696;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.AppendNode;97;-1549.852,-31.45649;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.TimeNode;6;-1783.948,287.0108;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;104;-1187.254,80.84278;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1272.745,-278.0894;Float;True;Property;_Noise;Noise;1;0;Assets/Addons/AmplifyShaderEditor/Examples/Community/Dissolve Burn/dissolve-guide.png;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1487.008,313.4546;Float;False;0;FLOAT;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;375.9492,-344.958;Float;False;0;FLOAT;0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;582.6468,-306.0555;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;487.9448,285.4472;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;8;-1779.448,457.4113;Float;False;Property;_NoiseTimeScale;NoiseTimeScale;4;0;0.02;0;0.1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;727.7989,-153.9001;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/Cloud;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;52;0;48;0
WireConnection;53;0;52;0
WireConnection;53;1;49;0
WireConnection;61;0;53;0
WireConnection;61;1;54;0
WireConnection;92;0;61;0
WireConnection;92;1;50;0
WireConnection;59;0;92;0
WireConnection;59;1;57;0
WireConnection;59;2;58;0
WireConnection;75;0;74;0
WireConnection;75;1;77;0
WireConnection;67;0;59;0
WireConnection;67;1;31;4
WireConnection;106;1;32;0
WireConnection;100;0;31;0
WireConnection;100;1;99;0
WireConnection;105;1;9;0
WireConnection;35;0;97;0
WireConnection;35;1;34;0
WireConnection;2;0;1;0
WireConnection;2;1;104;0
WireConnection;32;0;2;0
WireConnection;32;1;60;0
WireConnection;97;0;29;1
WireConnection;97;1;29;3
WireConnection;104;0;35;0
WireConnection;104;1;105;0
WireConnection;9;0;6;2
WireConnection;9;1;8;0
WireConnection;64;0;59;0
WireConnection;0;0;31;0
WireConnection;0;2;100;0
WireConnection;0;9;67;0
ASEEND*/
//CHKSM=CFB9249C08271C1FF91B6B1F12F42630850195F9