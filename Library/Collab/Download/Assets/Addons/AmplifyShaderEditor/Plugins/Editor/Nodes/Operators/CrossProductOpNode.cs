// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Cross", "Vector", "Cross product of two three-component vectors" )]
	public sealed class CrossProductOpNode : ParentNode
	{
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT3, false, "Lhs" );
			AddInputPort( WirePortDataType.FLOAT3, false, "Rhs" );
			AddOutputPort( WirePortDataType.FLOAT3, "Out" );
			m_useInternalPortData = true;
			m_previewShaderGUID = "65a9be5cc7037654db8e148d669f03ee";
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalVar )
		{
			string lhsStr = m_inputPorts[ 0 ].GeneratePortInstructions( ref dataCollector );
			string rhsStr = m_inputPorts[ 1 ].GeneratePortInstructions( ref dataCollector );

			string result = "cross( " + lhsStr + " , " + rhsStr + " )";
			return CreateOutputLocalVariable( 0, result, ref dataCollector );
		}

	}
}
