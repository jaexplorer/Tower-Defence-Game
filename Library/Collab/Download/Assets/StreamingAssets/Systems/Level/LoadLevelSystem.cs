using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class LoadLevelSystem : ReactiveSystem<LevelEntity>
{
    public readonly LevelContext _context;

    protected LoadLevelSystem(Contexts contexts) : base(contexts.level)
    {
        _context = contexts.level;
    }

    protected override ICollector<LevelEntity> GetTrigger(IContext<LevelEntity> context)
    {
        // return context.CreateCollector(LevelMatcher);
        // return context.CreateCollector(LevelMatcher.Tile.Added());
        return null;
    }

    protected override bool Filter(LevelEntity entity)
    {
        return true;
    }

    protected override void Execute(List<LevelEntity> _entities)
    {

    }
}