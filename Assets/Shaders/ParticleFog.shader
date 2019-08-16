// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fog"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Texture0("Texture 0", 2D) = "white" {}
		_TimeScale("Time Scale", Float) = 0.02
		_FogIntencity("FogIntencity", Float) = 1
		_MainColor("MainColor", Color) = (0,0,0,0)
		_BorderThickness("BorderThickness", Float) = 1
		_EffectScale("Effect Scale", Float) = 0.1
		_tiling("tiling", Range( 0 , 0.1)) = 0.01930745
		_BorderSharpness("BorderSharpness", Float) = 1
		_Emission("Emission", Color) = (0,0,0,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha  noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float eyeDepth;
		};

		uniform float4 _MainColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _BorderThickness;
		uniform float _BorderSharpness;
		uniform float _FogIntencity;
		uniform float4 _Emission;
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
			float temp_output_61_0 = ( clamp( ( ( eyeDepth52 - ( _BorderThickness + i.eyeDepth ) ) * _BorderSharpness ) , 0.0 , 1.0 ) * _FogIntencity );
			float4 temp_output_64_0 = ( _MainColor * temp_output_61_0 );
			o.Albedo = ( _MainColor + temp_output_64_0 ).rgb;
			o.Emission = ( _Emission + temp_output_64_0 ).rgb;
			o.Alpha = ( _MainColor.a * temp_output_61_0 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1173;655;1215.251;979.1556;1.6;True;True
Node;AmplifyShaderEditor.RangedFloatNode;50;-1275.883,-609.6443;Float;False;Property;_BorderThickness;BorderThickness;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SurfaceDepthNode;49;-1235.717,-528.4441;Float;False;0;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;-1240.337,-802.242;Float;False;False;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;29;-1459.219,295.9434;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TimeNode;6;-1460.849,11.21041;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;8;-1449.949,199.211;Float;False;Property;_TimeScale;Time Scale;2;0;0.02;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1011.416,-662.1442;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;52;-1036.233,-775.2419;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.AppendNode;43;-1169.246,270.81;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1176.708,114.4543;Float;False;0;FLOAT;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-860.7125,-592.3445;Float;False;Property;_BorderSharpness;BorderSharpness;7;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-834.0168,-713.544;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-596.9106,-714.6436;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;58;-589.4127,-539.0446;Float;False;Constant;_Float7;Float 7;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;34;-1233.087,436.5308;Float;False;Property;_tiling;tiling;4;0;0.01930745;0;0.1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;-590.2123,-616.7447;Float;False;Constant;_Float5;Float 5;5;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1000.224,216.4428;Float;False;0;FLOAT;0.0;False;1;FLOAT4;0;False;FLOAT4
Node;AmplifyShaderEditor.TexturePropertyNode;1;-915.2452,-15.68959;Float;True;Property;_Texture0;Texture 0;0;0;Assets/Addons/AmplifyShaderEditor/Examples/Community/Dissolve Burn/dissolve-guide.png;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-840.2863,315.1307;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.ClampOpNode;59;-394.4133,-660.3445;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;60;-399.1152,-521.2447;Float;False;Property;_FogIntencity;FogIntencity;2;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-599.288,97.00613;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-179.0115,-640.2434;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;33;-502.3855,335.2309;Float;False;Property;_EffectScale;Effect Scale;3;0;0.1;0;0;FLOAT
Node;AmplifyShaderEditor.ColorNode;31;-145.824,-402.6223;Float;False;Property;_MainColor;MainColor;3;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;47;-172.2947,-217.7246;Float;False;Property;_Emission;Emission;7;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;294.8494,-456.0579;Float;False;0;COLOR;0;False;1;FLOAT;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-232.5846,146.3304;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.ColorNode;62;349.6859,-929.1443;Float;False;Property;_BorderColor;BorderColor;0;0;0.5220588,0.9208925,1,0.528;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;66;600.8128,-758.321;Float;False;Property;_Float11;Float 11;5;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;-691.2433,-316.6899;Float;False;Property;_ZTransparancyOffset;ZTransparancyOffset;6;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;65;520.1058,-275.9678;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;70;505.9687,-81.73103;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;69;253.1561,22.78618;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;284.8372,-207.563;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-304.2862,-58.06901;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;72;109.0482,-30.35529;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-441.5435,-180.6899;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;30;-570.7183,-53.75691;Float;False;Property;_YTransparancy;YTransparancy;3;0;0.1;0;0;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;36;-689.6865,-233.5686;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;40;-686.7856,590.1318;Float;False;FLOAT;0;FLOAT;0.0;False;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;71;-704.9508,-909.853;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;42;-26.84526,503.6105;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.GrabScreenPosition;41;-272.944,464.8105;Float;False;True;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.AppendNode;39;6.814272,151.231;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.OneMinusNode;55;-564.7138,-914.8442;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;712.5985,-156.7001;Float;False;True;2;Float;ASEMaterialInspector;Standard;Fog;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;51;0;50;0
WireConnection;51;1;49;0
WireConnection;52;0;48;0
WireConnection;43;0;29;1
WireConnection;43;1;29;3
WireConnection;9;0;6;2
WireConnection;9;1;8;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;56;0;53;0
WireConnection;56;1;54;0
WireConnection;14;0;9;0
WireConnection;14;1;43;0
WireConnection;35;0;14;0
WireConnection;35;1;34;0
WireConnection;59;0;56;0
WireConnection;59;1;57;0
WireConnection;59;2;58;0
WireConnection;2;0;1;0
WireConnection;2;1;35;0
WireConnection;61;0;59;0
WireConnection;61;1;60;0
WireConnection;64;0;31;0
WireConnection;64;1;61;0
WireConnection;32;0;2;0
WireConnection;32;1;33;0
WireConnection;65;0;31;0
WireConnection;65;1;64;0
WireConnection;70;0;47;0
WireConnection;70;1;64;0
WireConnection;67;0;31;4
WireConnection;67;1;61;0
WireConnection;38;0;46;0
WireConnection;38;1;30;0
WireConnection;46;0;44;0
WireConnection;46;1;36;2
WireConnection;42;0;41;0
WireConnection;39;1;32;0
WireConnection;0;0;65;0
WireConnection;0;2;70;0
WireConnection;0;9;67;0
WireConnection;0;11;39;0
ASEEND*/
//CHKSM=887478F358E065412CF45DDF18C9E155E1909CCC