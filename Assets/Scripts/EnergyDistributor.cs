using System.Linq;
using UnityEngine;

public class EnergyDistributor
{
    public void EnergyDistribution(LifeExecutor lifeExecutor, ICell cell)
    {
        var neighbours = lifeExecutor.CellsBehaviours.Where(
                cellBehaviour => cellBehaviour.Position == cell.Position + Vector3.up * ICell.CellScale || 
                                 cellBehaviour.Position == cell.Position + Vector3.down * ICell.CellScale || 
                                 cellBehaviour.Position == cell.Position + Vector3.right * ICell.CellScale || 
                                 cellBehaviour.Position == cell.Position + Vector3.left * ICell.CellScale)
            .ToList();
        
        var commonEnergy = cell.Energy + neighbours.Sum(neighbour => neighbour.Energy);

        var averageEnergy = commonEnergy / (neighbours.Count + 1);
        
        cell.Energy = averageEnergy;
        foreach (var n in neighbours)
        {
            n.Energy = averageEnergy;
        }
    }
    
    public void LegacyTransfer(LifeExecutor lifeExecutor, int id, int energy)
    {
        var sprouts = lifeExecutor.GetSprouts(id);
        foreach (var sprout in sprouts)
            sprout.Energy += energy / sprouts.Count;
    }
}
