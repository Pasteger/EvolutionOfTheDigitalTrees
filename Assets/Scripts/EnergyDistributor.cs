using System.Collections.Generic;
using UnityEngine;

class EnergyDistributor
{
    public static void EnergyDistribution(GameObject cell)
    {
        List<GameObject> neighbours = new List<GameObject>();
        int commonEnergy = cell.GetComponent<ICell>().energy;

        for (int j = 0; j < 4; j++)
        {
            Vector3 direction = Vector3.up;
            switch (j)
            {
                case 1: direction = Vector3.down; break;
                case 2: direction = Vector3.right; break;
                case 3: direction = Vector3.left; break;
            }

            GameObject neighbour = FindNeighbour(cell, direction);
            if (neighbour != null)
            {
                commonEnergy += neighbour.GetComponent<ICell>().energy;
                neighbours.Add(neighbour);
            }
        }
        int averageEnergy = commonEnergy / (neighbours.Count + 1);
        cell.GetComponent<ICell>().energy = averageEnergy;
        foreach (GameObject n in neighbours)
        {
            n.GetComponent<ICell>().energy = averageEnergy;
        }
    }

    private static GameObject FindNeighbour(GameObject cell, Vector3 direction)
    {

        Ray ray = new Ray(cell.transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < 0.25)
            {
                GameObject neighbour = hit.collider.gameObject;
                try
                {
                    if (neighbour.GetComponent<ICell>().id == cell.GetComponent<ICell>().id &&
                        neighbour.GetComponent<ICell>().energy < cell.GetComponent<ICell>().energy)
                    {
                        return neighbour;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        return null;
    }

    public static void LegacyTransfer(int id, int energy)
    {
        List<GameObject> sprouts = new();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Sprout");
        
        foreach (GameObject sprout in gameObjects)
        {
            if (sprout.GetComponent<ICell>().id == id)
            {
                sprouts.Add(sprout);
            }
        }

        foreach (GameObject sprout in sprouts)
        {
            sprout.GetComponent<ICell>().energy += energy / sprouts.Count;
        }
    }
}
