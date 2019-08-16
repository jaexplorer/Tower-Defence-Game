using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem[] _effects;

    public void Play()
    {
        for (int i = 0; i < _effects.Length; i++)
        {
            _effects[i].Play();
        }
    }
}
