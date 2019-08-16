using UnityEngine;
using Entitas;

class GameController : MonoBehaviour
{
    private Systems _systems;
    private Systems _fixedSystems;
    private GameStateEntity time;

    private void Start()
    {
        var contexts = Contexts.sharedInstance;
        // contexts.S();
        _systems = CreateSystems(contexts);
        _fixedSystems = CreateFixedSystems(contexts);
        _systems.Initialize();

        // time = Contexts.sharedInstance.gameState.CreateEntity();
    }

    private void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }

    private void FixedUpdate()
    {
        // _fixedSystems.Execute();
        // _fixedSystems.Cleanup();

        // Contexts.sharedInstance.gameState.ReplaceFrameCount(Time.frameCount);
    }

    private void OnDestroy()
    {
        _systems.TearDown();
        _fixedSystems.TearDown();
    }

    private Systems CreateSystems(Contexts contexts)
    {
        return new Feature("Systems")
            // .Add(new InputSystem(contexts))
            // .Add(new InitiateSystem(contexts))
            // .Add(new LoadTileSystem(contexts))/

            ;
    }

    private Systems CreateFixedSystems(Contexts contexts)
    {
        return new Feature("FixedSystems")
            ;
    }
}