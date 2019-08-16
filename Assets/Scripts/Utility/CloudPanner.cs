using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates illusion of clouds moving
/// </summary>

public class CloudPanner : MonoBehaviour
{
    [SerializeField] private Transform _lightTransform;
    [SerializeField] private Vector3 _panningVector;
    Light _light;

    // Update is called once per frame
    void Update()
    {
        // _light.coo
        _lightTransform.Translate(_panningVector * Time.deltaTime);
    }
}
