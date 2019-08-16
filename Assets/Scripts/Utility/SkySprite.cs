using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Scales sky sprite to fill the screen
public class SkySprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Camera _camera;

    void Start()
    {
        float scale = transform.localScale.x * ((Screen.width / Screen.height) / (_renderer.sprite.texture.width / _renderer.sprite.texture.height)); // Still wrong
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
