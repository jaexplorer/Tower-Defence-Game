using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Not implimented
/// </summary>

public class LevelButton : MonoBehaviour 
{	
	[SerializeField]
	private Transform _transform;
	[SerializeField]
	private Text _text;

	private int _index;
	private LevelMenu _levelMenu;

	public void Set (Vector2 position, string text, int index, LevelMenu levelMenu)
	{
		_transform.localPosition = position;
		_text.text = text;
		_index = index;
		_levelMenu = levelMenu;
	}

	public void OnClick ()
	{
		_levelMenu.OnLevelButtonClick(_index);
	}
}
