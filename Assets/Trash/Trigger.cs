using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour 
{
	[SerializeField]
	private UnityEvent _onTriggerEnter;
	[SerializeField]
	private UnityEvent _onTriggerExit;

	private void OnTriggerEnter (Collider enteredCollider) 
	{
		_onTriggerEnter.Invoke();
	}
	
	private void OnTriggerExit (Collider enteredCollider) 
	{
		_onTriggerExit.Invoke();
	}
}
