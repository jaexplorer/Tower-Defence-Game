// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
using UnityEngine;

namespace AmplifyShaderEditor
{
	[System.Serializable]
	[NodeAttributes( "[Deprecated] World Reflection", "Surface Standard Inputs", "World reflection vector", null, KeyCode.None, true, true, "World Reflection", typeof( WorldReflectionVector ) )]
	public sealed class WorldReflInputsNode : SurfaceShaderINParentNode
	{
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			m_currentInput = AvailableSurfaceInputs.WORLD_REFL;
			InitialSetup();
		}
	}
}
