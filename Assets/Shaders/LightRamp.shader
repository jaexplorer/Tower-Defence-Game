// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LightRamp"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Texture3("Texture 3", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Texture3;

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float3 worldNormal = WorldNormalVector( i, float3(0,0,1) );
			float3 vertexNormal = mul( unity_WorldToObject, float4( worldNormal, 0 ) );
			o.Albedo = tex2D( _Texture3,vertexNormal.xy).xyz;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Lambert keepalpha 

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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
0;92;1117;667;1134.568;290.8701;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;103;-640,32;Float;False;194.64;351.8098;No shadows;2;99;65;
Node;AmplifyShaderEditor.NormalVertexDataNode;99;-624,80;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;70;-632.1,-167.5001;Float;True;Property;_Texture3;Texture 3;1;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.CommentaryNode;102;-640,416;Float;False;192.9501;191.2602;Has shadows;1;101;
Node;AmplifyShaderEditor.RangedFloatNode;97;453.9149,-585.0104;Float;False;Constant;_Float5;Float 5;2;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.SamplerNode;4;-377.3961,-749.6869;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;36;-1170.755,-703.4985;Float;False;0;FLOAT3;0,0,0;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;93;277.9138,-729.0098;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;453.9149,-681.0098;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;65;-624,224;Float;False;0;FLOAT3;0,0,0;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector2Node;101;-624,464;Float;False;Constant;_Vector3;Vector 3;3;0;0,0;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;96;261.9136,-617.0099;Float;False;Constant;_Float3;Float 3;2;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;98;648.514,-674.1098;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.NormalVertexDataNode;91;50.37607,-1065.259;Float;False;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;34;-914.7556,-655.4985;Float;False;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0;False;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-562.7547,-607.4985;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.TexturePropertyNode;88;626.3134,-913.0096;Float;True;Property;_Texture1;Texture 1;1;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.TexturePropertyNode;5;-663.5861,-833.3091;Float;True;Property;_Ramp;Ramp;1;0;None;False;white;Auto;SAMPLER2D
Node;AmplifyShaderEditor.SamplerNode;71;-384,80;Float;True;Property;_TextureSample3;Texture Sample 3;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;37;-1250.755,-543.4986;Float;False;0;FLOAT;0.0;False;FLOAT3
Node;AmplifyShaderEditor.Vector3Node;95;53.91101,-617.0099;Float;False;Constant;_Vector3;Vector 3;3;0;1,1,-1;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;92;53.61609,-778.3954;Float;False;Constant;_Vector2;Vector 2;3;0;1,1,-1;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;90;875.7131,-731.8098;Float;True;Property;_TextureSample2;Texture Sample 2;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;89;47.57893,-925.0156;Float;False;0;FLOAT3;0,0,0;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;-722.7554,-511.4986;Float;False;Constant;_Float1;Float 1;2;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;47;-930.7555,-543.4986;Float;False;Constant;_Float2;Float 2;2;0;0.5;0;0;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-722.7554,-623.4985;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;16,48;Float;False;True;2;Float;ASEMaterialInspector;Lambert;LightRamp;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;4;0;5;0
WireConnection;4;1;43;0
WireConnection;93;0;89;0
WireConnection;93;1;95;0
WireConnection;94;0;93;0
WireConnection;94;1;96;0
WireConnection;98;0;94;0
WireConnection;98;1;97;0
WireConnection;34;0;36;0
WireConnection;34;1;37;0
WireConnection;43;0;46;0
WireConnection;43;1;44;0
WireConnection;71;0;70;0
WireConnection;71;1;99;0
WireConnection;90;0;88;0
WireConnection;90;1;98;0
WireConnection;46;0;34;0
WireConnection;46;1;47;0
WireConnection;0;0;71;0
ASEEND*/
//CHKSM=EA6A74C9FA35460270D1A3A7FD4D29132802F3FD