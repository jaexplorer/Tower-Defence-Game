// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/WallRefract"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MainColor("MainColor", Color) = (0.3854887,0.7503951,0.8455882,0.341)
		_Emission("Emission", Float) = 0.5
		_BorderColor("BorderColor", Color) = (0.5220588,0.9208925,1,0.528)
		_BorderIntensity("BorderIntensity", Float) = 1
		_BorderThickness("BorderThickness", Float) = 1
		_BorderSharpness("BorderSharpness", Float) = 1
		_GradientTexture("GradientTexture", 2D) = "white" {}
		_GradientIntencity("GradientIntencity", Float) = 3
		_RefractionTexture("RefractionTexture", 2D) = "white" {}
		_RefractionAmount("RefractionAmount", Float) = 0
		_RefractionOffset("RefractionOffset", Float) = 0
		_RefractionTextureEmission("RefractionTextureEmission", Float) = 1
		_RefractionTextureScrollSpeed("RefractionTextureScrollSpeed", Float) = 0.2
		_RefractionTextureScale("RefractionTextureScale", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ "RefractionGrab0" }
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#pragma surface surf Standard alpha:fade keepalpha finalcolor:RefractionF  noshadow novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float eyeDepth;
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform float4 _BorderColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _BorderThickness;
		uniform float _BorderSharpness;
		uniform float _BorderIntensity;
		uniform float _Emission;
		uniform float _RefractionTextureEmission;
		uniform sampler2D _RefractionTexture;
		uniform float _RefractionTextureScale;
		uniform float _RefractionTextureScrollSpeed;
		uniform float _GradientIntencity;
		uniform sampler2D _GradientTexture;
		uniform sampler2D RefractionGrab0;
		uniform float _ChromaticAberration;
		uniform float _RefractionAmount;
		uniform float _RefractionOffset;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.0000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( RefractionGrab0, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( RefractionGrab0, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( RefractionGrab0, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			float3 vertexPos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 appendResult112 = float4( vertexPos.x , ( vertexPos.y + ( _RefractionTextureScrollSpeed * _Time.y ) ) , 0 , 0 );
			float4 tex2DNode98 = tex2D( _RefractionTexture,( _RefractionTextureScale * appendResult112 ).xy);
			float4 temp_cast_7 = _RefractionOffset;
				color.rgb = color.rgb + Refraction( i, o, ( ( tex2DNode98 * _RefractionAmount ) + temp_cast_7 ), _ChromaticAberration ) * ( 1 - color.a );
				color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float eyeDepth7 = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
			float temp_output_46_0 = ( clamp( ( ( 1.0 - ( eyeDepth7 - ( _BorderThickness + i.eyeDepth ) ) ) * _BorderSharpness ) , 0.0 , 1.0 ) * _BorderIntensity );
			float4 temp_output_30_0 = ( _MainColor + ( _BorderColor * temp_output_46_0 ) );
			o.Albedo = temp_output_30_0.rgb;
			float3 vertexPos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 appendResult112 = float4( vertexPos.x , ( vertexPos.y + ( _RefractionTextureScrollSpeed * _Time.y ) ) , 0 , 0 );
			float4 tex2DNode98 = tex2D( _RefractionTexture,( _RefractionTextureScale * appendResult112 ).xy);
			o.Emission = ( ( ( temp_output_30_0 * _Emission ) + ( temp_output_30_0 * ( _RefractionTextureEmission * tex2DNode98 ) ) ) + ( temp_output_30_0 * ( _GradientIntencity * tex2D( _GradientTexture,i.uv_texcoord) ) ) ).xyz;
			o.Alpha = ( _MainColor.a + ( _BorderColor.a * temp_output_46_0 ) );
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
0;92;1079;705;629.5429;365.7753;1.6;True;False
Node;AmplifyShaderEditor.RangedFloatNode;36;-1551.984,87.79752;Float;False;Property;_BorderThickness;BorderThickness;4;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SurfaceDepthNode;26;-1494.986,168.9977;Float;False;0;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;5;-1499.606,-104.8002;Float;False;False;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1270.685,35.29768;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;7;-1295.502,-77.80004;Float;False;0;0;FLOAT4;0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.TimeNode;110;-1420.038,746.1501;Float;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;113;-1515.133,658.1513;Float;False;Property;_RefractionTextureScrollSpeed;RefractionTextureScrollSpeed;12;0;0.2;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-1093.286,-16.10209;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-1168.836,751.4513;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;54;-1102.382,101.8974;Float;False;Property;_BorderSharpness;BorderSharpness;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;101;-1207.637,587.4511;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;34;-924.683,-90.80221;Float;False;0;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;43;-739.082,56.79736;Float;False;Constant;_Float2;Float 2;5;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;111;-1002.432,663.0511;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;-738.2822,134.4976;Float;False;Constant;_Float1;Float 1;5;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-736.6802,-41.10161;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;109;-978.1347,500.3512;Float;False;Property;_RefractionTextureScale;RefractionTextureScale;13;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;32;-553.0843,152.5974;Float;False;Property;_BorderIntensity;BorderIntensity;3;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;40;-550.6825,14.19753;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.AppendNode;112;-878.9318,587.6517;Float;False;FLOAT4;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-358.7808,32.89861;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-693.3353,522.7511;Float;False;0;FLOAT;0,0,0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.ColorNode;31;-526.8832,-240.4016;Float;False;Property;_BorderColor;BorderColor;2;0;0.5220588,0.9208925,1,0.528;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;100;-797.7338,281.1515;Float;True;Property;_RefractionTexture;RefractionTexture;8;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.TexCoordVertexDataNode;123;-453.2425,874.7744;Float;False;0;2;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;117;-410.3414,229.7745;Float;False;Property;_RefractionTextureEmission;RefractionTextureEmission;11;0;1;0;0;FLOAT
Node;AmplifyShaderEditor.ColorNode;28;-265.1856,-382.0012;Float;False;Property;_MainColor;MainColor;0;0;0.3854887,0.7503951,0.8455882,0.341;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;98;-500.7335,304.8511;Float;True;Property;_RefractionTextureI;RefractionTextureI;17;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;121;-391.5431,668.3246;Float;True;Property;_GradientTexture;GradientTexture;6;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-168.6846,-148.8026;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-9.886333,-168.4014;Float;False;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-135.7268,235.5518;Float;False;0;FLOAT;0,0,0,0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;120;-108.8424,679.524;Float;True;Property;_GradientTextureI;GradientTextureI;17;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;-132.9817,-24.80203;Float;False;Property;_Emission;Emission;1;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;125;-26.542,589.3746;Float;False;Property;_GradientIntencity;GradientIntencity;7;0;3;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;160.0582,43.97465;Float;False;0;COLOR;0.0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;105;-470.7356,527.5521;Float;False;Property;_RefractionAmount;RefractionAmount;9;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;260.5582,628.5748;Float;False;0;FLOAT;0.0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;155.8191,-65.80105;Float;False;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-136.0357,333.9526;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;264.6574,135.6751;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;116;312.9732,-17.44832;Float;False;0;COLOR;0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;107;-93.33583,463.2509;Float;False;Property;_RefractionOffset;RefractionOffset;10;0;0;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-148.485,72.79819;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;122;-77.74318,923.0748;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;103;163.7638,320.2505;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;124;451.6574,53.6743;Float;False;0;FLOAT4;0.0,0,0,0;False;1;FLOAT4;0.0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;129;-234.8435,1029.624;Float;False;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;49;166.2149,230.5983;Float;False;0;FLOAT;0.0;False;1;FLOAT;0;False;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;638.3007,-32.99999;Float;False;True;2;Float;ASEMaterialInspector;Standard;Custom/WallRefract;False;False;False;False;False;True;True;True;True;True;True;True;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;37;0;36;0
WireConnection;37;1;26;0
WireConnection;7;0;5;0
WireConnection;27;0;7;0
WireConnection;27;1;37;0
WireConnection;114;0;113;0
WireConnection;114;1;110;2
WireConnection;34;0;27;0
WireConnection;111;0;101;2
WireConnection;111;1;114;0
WireConnection;57;0;34;0
WireConnection;57;1;54;0
WireConnection;40;0;57;0
WireConnection;40;1;43;0
WireConnection;40;2;42;0
WireConnection;112;0;101;1
WireConnection;112;1;111;0
WireConnection;46;0;40;0
WireConnection;46;1;32;0
WireConnection;108;0;109;0
WireConnection;108;1;112;0
WireConnection;98;0;100;0
WireConnection;98;1;108;0
WireConnection;33;0;31;0
WireConnection;33;1;46;0
WireConnection;30;0;28;0
WireConnection;30;1;33;0
WireConnection;115;0;117;0
WireConnection;115;1;98;0
WireConnection;120;0;121;0
WireConnection;120;1;123;0
WireConnection;119;0;30;0
WireConnection;119;1;115;0
WireConnection;126;0;125;0
WireConnection;126;1;120;0
WireConnection;45;0;30;0
WireConnection;45;1;44;0
WireConnection;104;0;98;0
WireConnection;104;1;105;0
WireConnection;127;0;30;0
WireConnection;127;1;126;0
WireConnection;116;0;45;0
WireConnection;116;1;119;0
WireConnection;48;0;31;4
WireConnection;48;1;46;0
WireConnection;103;0;104;0
WireConnection;103;1;107;0
WireConnection;124;0;116;0
WireConnection;124;1;127;0
WireConnection;49;0;28;4
WireConnection;49;1;48;0
WireConnection;0;0;30;0
WireConnection;0;2;124;0
WireConnection;0;8;103;0
WireConnection;0;9;49;0
ASEEND*/
//CHKSM=238B60BFF8E5F85BDE236504230425182A1A4F79