// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/GlowingPrticleBlended"
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
			float4 appendResult17 = float4( i.vertexColor.r , i.vertexColor.g , i.vertexColor.b , 0 );
			o.Albedo = appendResult17.xyz;
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float temp_output_12_0 = ( tex2D( _MainTexture,uv_MainTexture).a - 0.1 );
			o.Emission = ( _TintColor * ( _EmissionAmount * ( appendResult17 * temp_output_12_0 ) ) ).xyz;
			o.Alpha = ( ( i.vertexColor.a * temp_output_12_0 ) * _TintColor.a );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1157;655;1420.853;1033.738;2.276489;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;9;-917.4532,-355.2885;Float;True;Property;_MainTexture;MainTexture;4;0;Assets/Textures/Particles/Star.png;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SamplerNode;7;-653.1812,-355.2015;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;14;-558.8491,-545.7769;Float;False;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-594.6495,-148.9765;Float;False;Constant;_Float0;Float 0;4;0;0.1;0;0;FLOAT
Node;AmplifyShaderEditor.AppendNode;17;-284.8484,-484.7769;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;12;-323.1496,-153.4756;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;3;-290.9379,-566.1471;Float;False;Property;_EmissionAmount;EmissionAmount;2;0;2;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-93.89918,-287.0001;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.ColorNode;1;-266.6002,-744.2993;Float;False;Property;_TintColor;TintColor;0;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;95.17832,-243.4552;Float;False;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-102.9466,-163.0293;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;179.151,-401.3768;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.ColorNode;2;-768.5003,274.6999;Float;False;Property;_EmissionColor;EmissionColor;0;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-420.2596,285.9573;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;149.6536,-82.27711;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;341.4001,-273.1998;Float;False;True;2;Float;ASEMaterialInspector;Lambert;Custom/GlowingPrticleBlended;False;False;False;False;True;True;True;True;True;True;True;True;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;7;0;9;0
WireConnection;17;0;14;1
WireConnection;17;1;14;2
WireConnection;17;2;14;3
WireConnection;12;0;7;4
WireConnection;12;1;13;0
WireConnection;8;0;17;0
WireConnection;8;1;12;0
WireConnection;11;0;3;0
WireConnection;11;1;8;0
WireConnection;10;0;14;4
WireConnection;10;1;12;0
WireConnection;15;0;1;0
WireConnection;15;1;11;0
WireConnection;4;0;2;0
WireConnection;18;0;10;0
WireConnection;18;1;1;4
WireConnection;0;0;17;0
WireConnection;0;2;15;0
WireConnection;0;9;18;0
ASEEND*/
//CHKSM=C1957B86A089A7A606A523E8F4A1ABF132CC5D5D