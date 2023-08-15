using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class EnergyDistributor
{
    public static void EnergyDistribution(ICell cell, Vector3 position)
    {
        var neighbours = new List<ICell>();
        var commonEnergy = cell.Energy;

        for (var j = 0; j < 4; j++)
        {
            var direction = j switch
            {
                1 => Vector3.down,
                2 => Vector3.right,
                3 => Vector3.left,
                _ => Vector3.up
            };

            var neighbour = FindNeighbour(cell, position, direction);
            if (ReferenceEquals(neighbour, null)) continue;
            commonEnergy += neighbour.Energy;
            neighbours.Add(neighbour);
        }
        var averageEnergy = commonEnergy / (neighbours.Count + 1);
        cell.Energy = averageEnergy;
        foreach (var n in neighbours)
        {
            n.Energy = averageEnergy;
        }
    }

    private static ICell FindNeighbour(ICell cell, Vector3 position, Vector3 direction)
    {
        if (!Physics.Raycast(new Ray(position, direction), out var hit)) return null;
        
        if (!(hit.distance < 0.25)) return null;
        
        var neighbour = hit.collider.gameObject;
        try
        {
            var neighbourCell = neighbour.GetComponent<ICell>();
            if (neighbourCell.ID == cell.ID &&
                neighbourCell.Energy < cell.Energy)
            {
                return neighbourCell;
            }
        }
        catch
        {
            return null;
        }
        return null;
    }

    public static void LegacyTransfer(int id, int energy)
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Sprout");
        
        //Подсказка для самого себя
        /*foreach (var sprout in gameObjects)
        {
            if (sprout.GetComponent<ICell>().id == id)
            {
                sprouts.Add(sprout);
            }
        }*/
        var sprouts = gameObjects.Where(
            sprout => sprout.GetComponent<ICell>().ID == id).ToList();

        foreach (var sprout in sprouts)
        {
            sprout.GetComponent<ICell>().Energy += energy / sprouts.Count;
        }
    }
}
