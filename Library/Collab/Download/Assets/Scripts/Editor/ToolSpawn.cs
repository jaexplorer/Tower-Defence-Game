using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ToolSpawn : LevelEditorTool
{
	private List<Spawner> _spawners;
	private int _currentSpawnerIndex;
	private bool _assigning;

	public override void OnEnable ()
	{
		_name = "Spawn"; 
		//Tile[] tiles = _level.GetTiles();
		//for (int i = 0; i < tiles.Length; i++)
		//{
		//	Tile tile = tiles[i];
		//	if (tile.GetData() != null && tile.GetData().IsSpawner())
		//	{
		//		_spawners.Add(tile.GetSpawner());
		//	}
		//}

		//List<SpawnerData> spawnerData = _level.spawnerData;
		//foreach (SpawnerData sd in spawnerData)
		//{
		//	if (sd.assigned)
		//	{
		//		Tile spawnerTile = _level.GetTile(sd.position);
		//		if (spawnerTile.data == null || !spawnerTile.data.IsSpawner())
		//		{
		//			sd.assigned = true;
		//		}
		//	}
		//}
	}

	//public override void OnEnable ()
	//{
	//	_name = "Spawn";
	//}

	//public override void DrawGUI ()
	//{
	//	// Drawing spawner buttons.
	//	List<SpawnerData> spawnerData = _level.spawnerData;
	//	for (int i = 0; i < spawnerData.Count; i++)
	//	{
	//		if (i == _currentSpawnerIndex)
	//		{
	//			GUI.backgroundColor = new Color(1f, 0.8f, 0f);
	//		}
	//		if (spawnerData[i].assigned == false)
	//		{
	//			GUI.backgroundColor = Color.red;
	//		}
	//		if (GUI.Button(new Rect(10, 40 + 25 * i, 200, 25), "Spawner " + i))
	//		{
	//			// Left click selects spawner.
	//			if (Event.current.button == 0)
	//			{
	//				_currentSpawnerIndex = i;
	//			}
	//			// Right click removes spawner.
	//			if (Event.current.button == 1)
	//			{
	//				_level.RemoveSpawner(i);
	//			}
	//		}
	//		GUI.backgroundColor = Color.white;
	//	}

	//	// Drawing "Add Spawner" button.
	//	if (GUI.Button(new Rect(10, _levelEditor.position.height - 40, 200, 25), "Add Spawner"))
	//	{
	//		_level.AddSpawner();
	//	}
	//}

	//public override void DrawHandles ()
	//{
	//	// Drawing spawner labels.
	//	List<SpawnerData> spawnerData = _level.spawnerData;
	//	for (int i = 0; i < spawnerData.Count; i++)
	//	{
	//		if (spawnerData[i].assigned)
	//		{
	//			MapPosition position = spawnerData[i].position;
	//			Tile tile = _level.GetTile(position);
	//			if (tile != null && tile.data != null && tile.data.IsSpawner())
	//			{
	//				Handles.Label(_levelEditor.WorldToScreenPoint(position.ToVector3()), "" + i);
	//			}
	//		}
	//	}

	//	// Drawing brush rectangle.
	//	Tile mouseTile = _levelEditor.GetMouseTile();
	//	if (mouseTile != null && mouseTile.data != null && mouseTile.data.IsSpawner())
	//	{
	//		DrawBrush(0, Color.green);
	//	}
	//	else
	//	{
	//		DrawBrush(0, Color.white);
	//	}
	//}

	//public override void HandleInput (Event e)
	//{
	//	// Assigning and selecting spawners.
	//	Tile mouseTile = _levelEditor.GetMouseTile();
	//	if (mouseTile != null)
	//	{
	//		// Finding spawner in the mouse tile.
	//		List<SpawnerData> spawnerData = _level.spawnerData;
	//		SpawnerData tileSpawnerData = null;
	//		for (int i = 0; i < spawnerData.Count; i++)
	//		{
	//			if (spawnerData[i].assigned && spawnerData[i].position == mouseTile.position)
	//			{
	//				tileSpawnerData = spawnerData[i];
	//			}
	//		}

	//		// Interacting with spawner in mouse tile.
	//		if (mouseTile != null && mouseTile.data != null && mouseTile.data.IsSpawner())
	//		{
	//			// Assign.
	//			if (e.type == EventType.MouseDown && e.button == 0)
	//			{
	//				if (tileSpawnerData == null)
	//				{
	//					spawnerData[_currentSpawnerIndex].position = _levelEditor.GetMouseMapPosition();
	//					spawnerData[_currentSpawnerIndex].assigned = true;
 //                   }
	//			}
	//			// Unassign.
	//			else if (e.type == EventType.MouseDown && e.button == 1)
	//			{
	//				tileSpawnerData.assigned = false;
	//			}
	//		}
	//	}
	//}
}
