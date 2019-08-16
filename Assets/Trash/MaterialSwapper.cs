// using UnityEngine;
// using System.Collections;

// public class MaterialSwapper : MonoBehaviour
// {
//     [SerializeField]
//     private MeshRenderer[] _meshRenderers;
//     [SerializeField]
//     private Material _highlightMaterial;

//     private Material[] _defaultMaterials;

//     private void Awake()
//     {
//         // for (int i = 0; i < _meshRenderers.Length; i++)
//         // {
//         //     _defaultMaterials[i] = _meshRenderers[i].sharedMaterial;
//         // }
//     }

//     public void SetMaterial(Material highlightMaterial)
//     {
//         // foreach (MeshRenderer m in _meshRenderers)
//         // {
//         //     m.material = highlightMaterial;
//         // }
//     }

//     public void RevertToOriginal()
//     {
//         // for (int i = 0; i < _meshRenderers.Length; i++)
//         // {
//         //     _meshRenderers[i].sharedMaterial = _defaultMaterials[i];
//         // }
//     }
// }
