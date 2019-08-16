// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/GlowingPrticle"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TintColor("TintColor", Color) = (0,0,0,0)
		_EmissionAmount("EmissionAmount", Float) = 2
		_MainTexture("MainTexture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert alpha:fade keepalpha  noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float4 _TintColor;
		uniform float _EmissionAmount;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 temp_output_20_0 = ( i.vertexColor * i.vertexColor );
			o.Albedo = ( _TintColor * temp_output_20_0 ).rgb;
			o.Emission = ( _EmissionAmount * temp_output_20_0 ).rgb;
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			o.Alpha = ( ( i.vertexColor.a * ( tex2D( _MainTexture,uv_MainTexture).a - 0.1 ) ) * _TintColor.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1095;664;999.5563;881.3781;1.676489;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;9;-917.4532,-355.2885;Float;True;Property;_MainTexture;MainTexture;4;0;Assets/Textures/Particles/Star.png;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.RangedFloatNode;13;-572.908,-143.8235;Float;False;Constant;_Float0;Float 0;4;0;0.1;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;7;-653.1812,-355.2015;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-323.1496,-153.4756;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;14;-531.4608,-551.7411;Float;False;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;19;-530.1063,-710.2529;Float;False;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-107.9761,-161.3528;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;3;48.80674,-702.5598;Float;False;Property;_EmissionAmount;EmissionAmount;2;0;2;0;0;FLOAT
Node;AmplifyShaderEditor.ColorNode;1;-331.8247,-716.6283;Float;False;Property;_TintColor;TintColor;0;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-187.8352,-409.9663;Float;False;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;316.051,-271.779;Float;False;0;FLOAT;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;332.9642,-760.1301;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-420.2596,285.9573;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.ColorNode;2;-768.5003,274.6999;Float;False;Property;_EmissionColor;EmissionColor;0;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;136.2417,-130.8953;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;111.4791,-420.3254;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;576.0783,-315.1119;Float;False;True;2;Float;ASEMaterialInspector;Lambert;Custom/GlowingPrticle;False;False;False;False;True;True;True;True;True;True;True;True;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;7;0;9;0
WireConnection;12;0;7;4
WireConnection;12;1;13;0
WireConnection;10;0;14;4
WireConnection;10;1;12;0
WireConnection;20;0;19;0
WireConnection;20;1;14;0
WireConnection;11;0;3;0
WireConnection;11;1;20;0
WireConnection;4;0;2;0
WireConnection;18;0;10;0
WireConnection;18;1;1;4
WireConnection;8;0;1;0
WireConnection;8;1;20;0
WireConnection;0;0;8;0
WireConnection;0;2;11;0
WireConnection;0;9;18;0
ASEEND*/
//CHKSM=E6268B669D405962CB9A43562E43D7FF3EC6A976