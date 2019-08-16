// using UnityEngine;
// using UnityEngine.Events;

// public class EventTest : MonoBehaviour 
// {
//     private UnityAction someListener;

//     void Awake ()
//     {
//         someListener = new UnityAction (SomeFunction);
//     }

//     void OnEnable ()
//     {
//         EventManager.Subscribe ("test", someListener);
//         EventManager.Subscribe ("Spawn", SomeOtherFunction);
//         EventManager.Subscribe ("Destroy", SomeThirdFunction);
//     }

//     void OnDisable ()
//     {
//         EventManager.Unsubscribe ("test", someListener);
//         EventManager.Unsubscribe ("Spawn", SomeOtherFunction);
//         EventManager.Unsubscribe ("Destroy", SomeThirdFunction);
//     }

//     void SomeFunction ()
//     {
//         Debug.Log ("Some Function was called!");
//     }
    
//     void SomeOtherFunction ()
//     {
//         Debug.Log ("Some Other Function was called!");
//     }
    
//     void SomeThirdFunction ()
//     {
//         Debug.Log ("Some Third Function was called!");
//     }
// }