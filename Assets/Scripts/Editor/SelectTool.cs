using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ToolSelect : LevelEditorTool
{
	//private List<TileData> _tileData = new List<TileData>(256);
	//private TileData _currentTileData;
	//private int _brushExtraSize = 0;
	
	override public void OnEnable ()
	{
		_name = "Select";
	}
}