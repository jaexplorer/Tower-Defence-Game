using System.Collections;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Level] public sealed class PositionComponent : IComponent { public Vector3 value; }
[Game, Level] public sealed class PrefabComponent : IComponent { public GameObject value; }
[Game, Level] public sealed class GameObjectComponent : IComponent { public GameObject value; }
[Game, Level] public sealed class TransformComponent : IComponent { public GameObject value; }