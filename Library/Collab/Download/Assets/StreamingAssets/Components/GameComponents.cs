using System.Collections;
using UnityEngine;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique] public sealed class CurrentLevelIndexComponent : IComponent { public int value; }
[Game, Unique] public sealed class LevelDataListComponent : IComponent { public LevelData[] value; }
[Game, Unique] public sealed class LoadLevelIndexComponent : IComponent { public int value; }
[Game, Unique] public sealed class GameStateComponent : IComponent { public GameState value; }

public enum GameState
{
    Intro,
    ProfileMenu,
    MainMenu,
    Game,
    Building,
    Wave,
    PauseMenu,
    DebriefMenu,
    LevelMenu
}
