// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Wall"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_BorderColor("BorderColor", Color) = (0.5220588,0.9208925,1,0.528)
		_MainColor("MainColor", Color) = (0.3854887,0.7503951,0.8455882,0.341)
		_BorderIntensity("BorderIntensity", Float) = 1
		_BorderThickness("BorderThickness", Float) = 1
		_WallEmission("WallEmission", Float) = 0.5
		_BorderSharpness("BorderSharpness", Float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha  noshadow novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float eyeDepth;
		};

		uniform float4 _MainColor;
		uniform float4 _BorderColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _BorderThickness;
		uniform float _BorderSharpness;
		uniform float _BorderIntensity;
		uniform float _WallEmission;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float eyeDepth7 = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
			float temp_output_46_0 = ( clamp( ( ( 1.0 - ( eyeDepth7 - ( _BorderThickness + i.eyeDepth ) ) ) * _BorderSharpness ) , 0.0 , 1.0 ) * _BorderIntensity );
			float4 temp_output_30_0 = ( _MainColor + ( _BorderColor * temp_output_46_0 ) );
			o.Albedo = temp_output_30_0.rgb;
			o.Emission = ( temp_output_30_0 * _WallEmission ).rgb;
			o.Alpha = ( _MainColor.a + ( _BorderColor.a * temp_output_46_0 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1157;685;1777.381;850.9998;2.2;True;True
Node;AmplifyShaderEditor.ScreenPosInputsNode;5;-1499.606,-104.8002;Float;False;False;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SurfaceDepthNode;26;-1494.986,168.9977;Float;False;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;36;-1551.984,87.79752;Float;False;Property;_BorderThickness;BorderThickness;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1270.685,35.29768;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;7;-1295.502,-77.80004;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-1093.286,-16.10209;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-1102.382,101.8974;Float;False;Property;_BorderSharpness;BorderSharpness;7;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;34;-924.683,-90.80221;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-736.6802,-41.10161;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;43;-739.082,56.79736;Float;False;Constant;_Float2;Float 2;5;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;-738.2822,134.4976;Float;False;Constant;_Float1;Float 1;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;40;-550.6825,14.19753;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;32;-553.0843,152.5974;Float;False;Property;_BorderIntensity;BorderIntensity;2;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-358.7808,32.89861;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.ColorNode;31;-464.1833,-177.7017;Float;False;Property;_BorderColor;BorderColor;0;0;0.5220588,0.9208925,1,0.528;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;28;-202.4857,-319.3013;Float;False;Property;_MainColor;MainColor;0;0;0.3854887,0.7503951,0.8455882,0.341;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-105.9847,-86.10262;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;30;137.3137,-107.0015;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;44;-96.58171,34.89796;Float;False;Property;_WallEmission;WallEmission;5;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-100.385,138.6982;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-563.2805,470.6978;Float;False;0;FLOAT;0;False;1;FLOAT;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;52;-1141.184,569.698;Float;True;Property;_Texture0;Texture 0;6;0;Assets/Textures/WallGradient.psd;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;237.2191,19.29895;Float;False;0;COLOR;0.0;False;1;FLOAT;0.0;False;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;49;136.6149,129.3984;Float;False;0;FLOAT;0.0;False;1;FLOAT;0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-1060.281,435.8983;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;59;-1494.482,372.2984;Float;False;Property;_BorderTextureTiling;BorderTextureTiling;8;0;-1.17;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;61;-1350.58,474.7982;Float;False;Property;_BorderTexturelPanning;BorderTexturelPanning;9;0;0.14;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;53;-908.1831,505.8966;Float;True;Property;_TextureSample0;Texture Sample 0;7;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1241.686,322.0977;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;398.9005,-71;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/Wall;False;False;False;False;False;True;True;True;True;True;True;True;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;37;0;36;0
WireConnection;37;1;26;0
WireConnection;7;0;5;0
WireConnection;27;0;7;0
WireConnection;27;1;37;0
WireConnection;34;0;27;0
WireConnection;57;0;34;0
WireConnection;57;1;54;0
WireConnection;40;0;57;0
WireConnection;40;1;43;0
WireConnection;40;2;42;0
WireConnection;46;0;40;0
WireConnection;46;1;32;0
WireConnection;33;0;31;0
WireConnection;33;1;46;0
WireConnection;30;0;28;0
WireConnection;30;1;33;0
WireConnection;48;0;31;4
WireConnection;48;1;46;0
WireConnection;58;1;53;3
WireConnection;45;0;30;0
WireConnection;45;1;44;0
WireConnection;49;0;28;4
WireConnection;49;1;48;0
WireConnection;64;0;55;0
WireConnection;64;1;61;0
WireConnection;53;0;52;0
WireConnection;53;1;64;0
WireConnection;55;1;59;0
WireConnection;0;0;30;0
WireConnection;0;2;45;0
WireConnection;0;9;49;0
ASEEND*/
//CHKSM=B036DA38B15775ADC075B4FD94487017C07199F2