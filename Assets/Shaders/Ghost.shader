// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Ghost"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Texture("Texture", 2D) = "white" {}
		_EmissionTexture("EmissionTexture", 2D) = "white" {}
		_EmissionTint("EmissionTint", Color) = (0,0,0,0)
		_EmissionFromTexture("EmissionFromTexture", Float) = 1
		_EmissionMult("Emission Mult", Float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha , OneMinusSrcAlpha OneMinusSrcAlpha
		BlendOp Add , Add
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha  noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
			float2 texcoord_1;
		};

		uniform sampler2D _Texture;
		uniform float _EmissionMult;
		uniform sampler2D _EmissionTexture;
		uniform float _EmissionFromTexture;
		uniform float4 _EmissionTint;
		uniform float _MaskClipValue = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Emission = ( ( ( tex2D( _Texture,i.texcoord_0) * _EmissionMult ) + ( tex2D( _EmissionTexture,i.texcoord_1) * _EmissionFromTexture ) ) * _EmissionTint ).rgb;
			o.Alpha = _EmissionTint.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1370;677;1394.41;535.0214;1.6;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;12;-957.8,214.9415;Float;True;Property;_EmissionTexture;EmissionTexture;1;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-978.1997,415.1418;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-623.6995,-135.9;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;2;-601.6998,-338.3001;Float;True;Property;_Texture;Texture;0;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.RangedFloatNode;15;-700.0099,427.3784;Float;False;Property;_EmissionFromTexture;EmissionFromTexture;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;-719.5684,220.9416;Float;True;Property;_qwet;qwet;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-337.6004,-93.09983;Float;False;Property;_EmissionMult;Emission Mult;4;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-367.2682,-303.9001;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-388.71,282.2781;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-11.39978,-218.9002;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.ColorNode;4;20.27793,216.9736;Float;False;Property;_EmissionTint;EmissionTint;2;0;0,0,0,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;16;154.5903,-21.52178;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;10;381.8835,416.0615;Float;False;Property;_Opacity;Opacity;5;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;312.5996,-8.499784;Float;False;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;403.921,433.9489;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;505.6002,-134.4999;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/Ghost;False;False;False;False;False;False;False;False;False;False;False;False;Back;1;0;False;0;0;Custom;0.5;True;False;0;True;Transparent;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;2;SrcAlpha;OneMinusSrcAlpha;1;OneMinusSrcAlpha;OneMinusSrcAlpha;Add;Add;0;False;0;0,0,0,0;VertexScale;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;11;0;12;0
WireConnection;11;1;13;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;17;0;11;0
WireConnection;17;1;15;0
WireConnection;6;0;1;0
WireConnection;6;1;9;0
WireConnection;16;0;6;0
WireConnection;16;1;17;0
WireConnection;8;0;16;0
WireConnection;8;1;4;0
WireConnection;0;2;8;0
WireConnection;0;9;4;4
ASEEND*/
//CHKSM=1B34BA1174C97CDB7FC22CEB23A5AF3C3F2ED6DD