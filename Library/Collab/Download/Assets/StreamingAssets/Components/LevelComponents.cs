using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Level, Unique] public sealed class TileGridComponent : IComponent { public int sizeX; public int sizeZ; }
// [Level, Unique] public sealed class LevelIndexComponent : IComponent { public TileVector value; }

[Level, CustomPrefixAttribute("has")] public sealed class SocketComponent : IComponent { }
[Level] public sealed class TileComponent : IComponent { }
[Level] public sealed class WalkableComponent : IComponent { }
[Level] public sealed class TilePositionComponent : IComponent {[EntityIndex] public TileVector value; }
[Level] public sealed class TileDataComponent : IComponent { public TileData value; }
