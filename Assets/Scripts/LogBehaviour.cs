using System.Collections.Generic;
using UnityEngine;

public class LogBehaviour : ICell
{
    public LogBehaviour(LifeExecutor lifeExecutor, Vector3Int position, int lifespan, 
                        List<Dictionary<Vector3, int>> genome, int energy, int id)
    {
        Type = CellType.Log;
        LifeExecutor = lifeExecutor;
        Position = position;
        Lifespan = lifespan;
        Genome = genome;
        Energy = energy;
        ID = id;
        LifeExecutor.SetCell(position, Type);
    }
    
    public override void Life()
    {
        Lifespan--;
        switch (Lifespan)
        {
            case < 1:
                Death();
                break;
            case 1:
                LifeExecutor.EnergyDistributor.LegacyTransfer(LifeExecutor ,ID, Energy);
                Energy = 0;
                break;
        }

        if (Energy >= 5)
        {
            Energy += 100;
        }

        LifeExecutor.EnergyDistributor.EnergyDistribution(LifeExecutor,this);

        if (Energy < 1)
        {
            Death();
        }

        Energy--;
    }
}
