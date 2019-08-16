// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/EnemyVertexColor"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.84
		_TeleportEmission("TeleportEmission", Float) = 0
		_EmissionColor("EmissionColor", Color) = (1,1,1,1)
		_BlinkEmission("BlinkEmission", Float) = 0
		_Color1("Color 1", Color) = (1,1,1,1)
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
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 vertexColor : COLOR;
		};

		uniform float4 _EmissionColor;
		uniform float _BlinkEmission;
		uniform float4 _Color1;
		uniform float _TeleportEmission;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = ( i.vertexColor * i.vertexColor ).rgb;
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
0;92;1198;701;907.3958;38.40045;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;34;-794.9286,139.9001;Float;False;Property;_TeleportEmission;TeleportEmission;1;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-554.2016,127.1006;Float;False;Property;_BlinkEmission;BlinkEmission;1;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.ColorNode;33;-790.928,-29.89974;Float;False;Property;_Color1;Color 1;1;0;1,1,1,1;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;4;-568.201,-41.69997;Float;False;Property;_EmissionColor;EmissionColor;1;0;1,1,1,1;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;37;-333.2965,-322.1991;Float;False;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-294.8014,31.90013;Float;False;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-531.7278,210.1004;Float;False;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.VertexColorNode;39;-331.4962,-154.7992;Float;False;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.AppendNode;21;-928.5085,746.4025;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;26;-2427.811,1128.204;Float;False;Property;_TimeScale;TimeScale;6;0;2.63;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2164.506,1029.903;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1611.708,857.303;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;11;-1536.201,620.3;Float;True;Property;_Texture1;Texture 1;1;0;Assets/Textures/PerlinSeamless.jpg;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.TimeNode;27;-2454.306,932.9022;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;43;-411.3958,301.5995;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TangentVertexDataNode;47;-199.3958,461.5995;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1953.407,1165.503;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;2;-663.6015,-369.4995;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BlendOpsNode;42;-545.6971,504.1993;Float;False;ColorBurn;True;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SamplerNode;12;-1268.401,741.0009;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1132.801,-168.6996;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-150.2951,110.1999;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;COLOR
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1115.599,-366.1995;Float;True;Property;_Texture0;Texture 0;1;0;Assets/Textures/Texture.psd;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.AppendNode;20;-1826.209,888.5038;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-757.5007,801.9006;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1014.701,932.8019;Float;False;Property;_NoiseAmplitude;NoiseAmplitude;1;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-104.6961,-194.5994;Float;False;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.PosVertexDataNode;15;-2255.707,775.8015;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1993.905,916.6036;Float;False;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-333.3958,549.5995;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-1804,783.202;Float;False;Property;_NoiseScale;NoiseScale;1;0;1.74;0;0;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;97.50002,-1.3;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/EnemyVertexColor;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;True;0.84;0,0,0,0;VertexScale;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;21;1;12;3
WireConnection;31;0;27;0
WireConnection;31;1;26;0
WireConnection;25;0;13;0
WireConnection;25;1;20;0
WireConnection;30;1;31;0
WireConnection;2;0;3;0
WireConnection;2;1;1;0
WireConnection;12;0;11;0
WireConnection;12;1;25;0
WireConnection;36;0;6;0
WireConnection;36;1;35;0
WireConnection;20;0;15;1
WireConnection;20;1;32;0
WireConnection;16;0;21;0
WireConnection;16;1;17;0
WireConnection;38;0;37;0
WireConnection;38;1;39;0
WireConnection;32;0;15;2
WireConnection;32;1;31;0
WireConnection;0;0;38;0
WireConnection;0;2;36;0
ASEEND*/
//CHKSM=E2F23FE1D16CEAA22937206AD8144B7D9E5F09C0