// using UnityEngine;
// using System.Collections;
// using DG.Tweening;

// public class MeshAnimator : MonoBehaviour 
// {
// 	// Mesh filter to be animated.
// 	[SerializeField]
// 	private MeshFilter _meshFilter;
// 	// Meshes the animation consists of.
// 	[SerializeField]
// 	private  Mesh[] _meshes;
// 	// Frame rate in frames per second.
// 	[SerializeField]
// 	private int _frameRate = 15;
// 	[SerializeField]
// 	public bool _playOnEnable = true; 

// 	private WaitForSeconds _frameInterval;
// 	private int _currentFrame;
// 	private bool _playing = false;

// 	private void OnEnable ()
// 	{
// 		if (_playOnEnable)
// 		{
// 			Play();
// 		}
// 	}

// 	public void Play ()
// 	{
// 		_meshFilter.transform.DOLocalJump(new Vector3(0f, 0.4f, 0f), 0.1f, 2000, 1000f);
// 		if (_frameInterval == null)
// 		{
// 			_frameInterval = new WaitForSeconds(1f/_frameRate);
// 		}
// 		_playing = true;
// 		StartCoroutine(AnimationCoroutine());
// 	}

// 	private IEnumerator AnimationCoroutine ()
// 	{	
// 		while (_playing)
// 		{
// 			_currentFrame++;
// 			if (_currentFrame == _meshes.Length)
// 			{
// 				_currentFrame = 0;
// 			}
// 			_meshFilter.mesh = _meshes[_currentFrame];
// 			//print(1/_frameRate);
// 			//yield return new WaitForSeconds(1f/_frameRate);

// 			yield return _frameInterval;
// 		}
// 	}
// }