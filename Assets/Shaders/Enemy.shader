// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Enemy"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.84
		_TeleportEmission("TeleportEmission", Float) = 0
		_EmissionColor("EmissionColor", Color) = (1,1,1,1)
		_Texture0("Texture 0", 2D) = "white" {}
		_Color1("Color 1", Color) = (1,1,1,1)
		_BlinkEmission("BlinkEmission", Float) = 0
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Standard keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:outlineVertexDataFunc
		struct Input
		{
			fixed filler;
		};
		uniform fixed4 _ASEOutlineColor;
		uniform fixed _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz *= ( 1 + _ASEOutlineWidth);
		}
		void outlineSurf( Input i, inout SurfaceOutputStandard o ) { o.Emission = _ASEOutlineColor.rgb; o.Alpha = 1; }
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
		};

		uniform sampler2D _Texture0;
		uniform float4 _EmissionColor;
		uniform float _BlinkEmission;
		uniform float4 _Color1;
		uniform float _TeleportEmission;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = tex2D( _Texture0,i.texcoord_0).xyz;
			o.Emission = ( ( _EmissionColor * _BlinkEmission ) + ( _Color1 * _TeleportEmission ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1171;608;1076.595;115.1001;1;True;False
Node;AmplifyShaderEditor.ColorNode;33;-790.928,-29.89974;Float;False;Property;_Color1;Color 1;1;0;1,1,1,1;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;4;-568.201,-41.69997;Float;False;Property;_EmissionColor;EmissionColor;1;0;1,1,1,1;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;34;-794.9286,139.9001;Float;False;Property;_TeleportEmission;TeleportEmission;1;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-554.2016,127.1006;Float;False;Property;_BlinkEmission;BlinkEmission;1;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-531.7278,210.1004;Float;False;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-313.8014,-8.099869;Float;False;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-997.9005,-151.5997;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;3;-988.2991,-354.7995;Float;True;Property;_Texture0;Texture 0;1;0;Assets/Textures/Texture.psd;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.AppendNode;20;-1261.808,531.5023;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1239.599,426.201;Float;False;Property;_NoiseScale;NoiseScale;1;0;1.74;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-150.2951,110.1999;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;COLOR
Node;AmplifyShaderEditor.AppendNode;21;-364.1079,389.4016;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.ConditionalIfNode;9;2.799019,448.1008;Float;False;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1600.106,672.9016;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;17;-450.3004,575.8005;Float;False;Property;_NoiseAmplitude;NoiseAmplitude;1;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1429.503,559.6022;Float;False;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;2;-612.301,-308.3997;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-193.1002,444.8995;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1047.307,500.3016;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;12;-704,384;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;15;-1691.308,418.8004;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;11;-971.7993,263.3002;Float;True;Property;_Texture1;Texture 1;1;0;Assets/Textures/PerlinSeamless.jpg;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1389.006,808.501;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;26;-1863.414,771.2018;Float;False;Property;_TimeScale;TimeScale;6;0;2.63;0;0;FLOAT
Node;AmplifyShaderEditor.TimeNode;27;-1889.908,575.9007;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/Enemy;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;True;0.84;0,0,0,0;VertexScale;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;20;0;15;1
WireConnection;20;1;32;0
WireConnection;36;0;6;0
WireConnection;36;1;35;0
WireConnection;21;1;12;3
WireConnection;31;0;27;0
WireConnection;31;1;26;0
WireConnection;32;0;15;2
WireConnection;32;1;31;0
WireConnection;2;0;3;0
WireConnection;2;1;1;0
WireConnection;16;0;21;0
WireConnection;16;1;17;0
WireConnection;25;0;13;0
WireConnection;25;1;20;0
WireConnection;12;0;11;0
WireConnection;12;1;25;0
WireConnection;30;1;31;0
WireConnection;0;0;2;0
WireConnection;0;2;36;0
ASEEND*/
//CHKSM=3DC001D113BC8AA2F48ADB9BC853202FA1F21F8E