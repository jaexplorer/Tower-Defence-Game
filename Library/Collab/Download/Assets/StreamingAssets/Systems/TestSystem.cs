using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class TestSystem : IInitializeSystem
{
    public readonly GameContext _context = Contexts.sharedInstance.game;

    public void Initialize()
    {

    }
}

public class TestRSystem : ReactiveSystem<GameEntity>
{
    public readonly GameContext _context = Contexts.sharedInstance.game;

    protected TestRSystem(Collector<GameEntity> collector) : base(collector)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return _context.GetGroup(GameMatcher.Position).CreateCollector(GroupEvent.Added);
    }

    public void Initialize()
    {

    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> _entities)
    {

    }
}

public interface PositionEntity : IEntity, IPosition { }
public partial class GameEntity : PositionEntity { }
public partial class LevelEntity : PositionEntity { }

public class MultiSystem : MultiReactiveSystem<PositionEntity, Contexts>
{
    public readonly LevelContext _context;

    protected MultiSystem(Contexts contexts) : base(contexts)
    {
        _context = contexts.level;
    }

    protected override ICollector[] GetTrigger(Contexts contexts)
    {
        return new ICollector[] {
            contexts.game.CreateCollector(GameMatcher.Position),
            contexts.level.CreateCollector(LevelMatcher.Position)
        };
    }

    protected override bool Filter(PositionEntity entity)
    {
        return true;
    }

    protected override void Execute(List<PositionEntity> _entities)
    {

    }
}