using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SproutBehaviour : ICell
{
    private readonly int _activeGen;

    public SproutBehaviour(LifeExecutor lifeExecutor, GameObject ground, Vector3Int position, int lifespan, 
        List<Dictionary<Vector3, int>> genome, int energy, int id, int activeGen)
    {
        Type = CellType.Sprout;
        LifeExecutor = lifeExecutor;
        Ground = ground;
        Position = position;
        Lifespan = lifespan;
        Genome = genome;
        Energy = energy;
        ID = id;
        _activeGen = activeGen;
        LifeExecutor.SetCell(position, Type);
    }
    
    public override void Life()
    {
        Lifespan--;
        LifeExecutor.EnergyDistributor.EnergyDistribution(LifeExecutor, this);

        if (Lifespan < 1)
        {
            Death();
            LifeExecutor.AddCellBehavior(new SeedBehaviour(LifeExecutor, Ground, Position, Lifespan, BetrayGenome(), Energy, ID));
        }
        else
        {
            var gen = Genome[_activeGen];
            if (Energy > 15)
            {
                var numberDescendants = 0;

                if (gen[Vector3.up] < 16 && LifeExecutor.CheckNeighbourCell(Position, Vector3.up))
                    numberDescendants++;
                if (gen[Vector3.down] < 16 && LifeExecutor.CheckNeighbourCell(Position, Vector3.down))
                    numberDescendants++;
                if (gen[Vector3.right] < 16 && LifeExecutor.CheckNeighbourCell(Position, Vector3.right))
                    numberDescendants++;
                if (gen[Vector3.left] < 16 && LifeExecutor.CheckNeighbourCell(Position, Vector3.left))
                    numberDescendants++;

                if (gen[Vector3.up] < 16  && LifeExecutor.CheckNeighbourCell(Position, Vector3.up))
                    Grow(Vector3.up, gen[Vector3.up], numberDescendants);
                if (gen[Vector3.down] < 16 && LifeExecutor.CheckNeighbourCell(Position, Vector3.down))
                    Grow(Vector3.down, gen[Vector3.down], numberDescendants);
                if (gen[Vector3.right] < 16 && LifeExecutor.CheckNeighbourCell(Position, Vector3.right))
                    Grow(Vector3.right, gen[Vector3.right], numberDescendants);
                if (gen[Vector3.left] < 16 && LifeExecutor.CheckNeighbourCell(Position, Vector3.left))
                    Grow(Vector3.left, gen[Vector3.left], numberDescendants);

                Death();
                LifeExecutor.AddCellBehavior(new LogBehaviour(LifeExecutor, Position, Lifespan, Genome, Energy, ID));
                return;
            }
        }

        if (Energy < 1)
        {
            Death();
        }

        Energy--;
    }

    private void Grow(Vector3 direction, int gen, int numberDescendants)
    {
        var newPosition = Position;

        if (direction.Equals(Vector3.up))
        {
            newPosition.y += CellScale;
        }
        else if (direction.Equals(Vector3.down))
        {
            newPosition.y -= CellScale;
        }
        else if (direction.Equals(Vector3.right))
        {
            newPosition.x += CellScale;
        }
        else
        {
            newPosition.x -= CellScale;
        }

        if (FindBlock(Position, direction)) return;
        
        var newEnergy = (Energy - 5) / numberDescendants;

        LifeExecutor.AddCellBehavior(new SproutBehaviour(LifeExecutor, Ground, newPosition, Lifespan, Genome, newEnergy, ID, gen));

        Energy -= newEnergy;
    }

    private bool FindBlock(Vector3 position, Vector3 direction)
    {
        if (Physics.Raycast(new Ray(position, direction), out var hit))
        {
            return hit.distance < CellScale;
        }

        return false;
    }

    private List<Dictionary<Vector3, int>> BetrayGenome()
    {
        List<Dictionary<Vector3, int>> theGenome = new();

        foreach (var gen in Genome)
        {
            Dictionary<Vector3, int> newGen = new();
            var mutatingGene = -1;

            if (Random.Range(0, 25) == 1)
            {
                mutatingGene = Random.Range(0, 4);
            }

            var upGenMutationValue = gen[Vector3.up];
            var downGenMutationValue = gen[Vector3.down];
            var rightGenMutationValue = gen[Vector3.right];
            var leftGenMutationValue = gen[Vector3.left];

            switch (mutatingGene)
            {
                case 0:
                    upGenMutationValue += Random.Range(0, 30);
                    if (upGenMutationValue > 29)
                        upGenMutationValue -= 29;
                    break;
                case 1:
                    downGenMutationValue += Random.Range(0, 30);
                    if (downGenMutationValue > 29)
                        downGenMutationValue -= 29;
                    break;
                case 2:
                    rightGenMutationValue += Random.Range(0, 30);
                    if (rightGenMutationValue > 29)
                        rightGenMutationValue -= 29;
                    break;
                case 3:
                    leftGenMutationValue += Random.Range(0, 30);
                    if (leftGenMutationValue > 29)
                        leftGenMutationValue -= 29;
                    break;
            }

            newGen.Add(Vector3.up, upGenMutationValue);
            newGen.Add(Vector3.down, downGenMutationValue);
            newGen.Add(Vector3.right, rightGenMutationValue);
            newGen.Add(Vector3.left, leftGenMutationValue);

            theGenome.Add(newGen);
        }

        return theGenome;
    }
}