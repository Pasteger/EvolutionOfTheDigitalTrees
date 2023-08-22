using System.Collections.Generic;
using UnityEngine;

public class SeedBehaviour : ICell
{
    public SeedBehaviour(LifeExecutor lifeExecutor, GameObject ground, Vector3Int position, int lifespan, 
        List<Dictionary<Vector3, int>> genome, int energy, int id)
    {
        Type = CellType.Seed;
        LifeExecutor = lifeExecutor;
        Position = position;
        Lifespan = lifespan;
        Genome = genome;
        Energy = energy;
        ID = id;
        Ground = ground;
        LifeExecutor.SetCell(position, Type);
        LifeExecutor.GenomeGenerator.RemoveId(ID);
    }

    public override void Life()
    {
        if (Energy < 1)
        {
            Death();
            return;
        }

        Fall();
    }

    private void Fall()
    {
        if (Physics.Raycast(new Ray(Position, Vector3.down), out var hitInfo))
        {
            if (hitInfo.distance > CellScale && LifeExecutor.CheckNeighbourCell(Position, Vector3.down))
            {
                LifeExecutor.SetCell(Position, CellType.Air);
                Position += new Vector3Int(0, -CellScale, 0);
                LifeExecutor.SetCell(Position, CellType.Seed);
            }
            else if (hitInfo.collider.gameObject.name.Equals(Ground.name))
            {
                Death();

                LifeExecutor.AddCellBehavior(new SproutBehaviour(
                    LifeExecutor, Ground, Position, 40, Genome, Energy, LifeExecutor.GenomeGenerator.GetId(), 0));
            }
            else
            {
                Death();
            }
        }
        Energy--;
    }
}
