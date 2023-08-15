using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBehaviour : MonoBehaviour, ICell
{
    public int id { get; set; }
    public int energy { get; set; }
    public List<Dictionary<Vector3, int>> genome { get; set; }
    public int lifespan { get; set; }

    void FixedUpdate()
    {
        lifespan--;
        if (lifespan < 1)
        {
            Destroy(gameObject);
        }

        if (lifespan == 1)
        {
            EnergyDistributor.LegacyTransfer(id, energy);
            energy = 0;
        }


        if (energy > 5)
        {
            energy += 100;
        }

        EnergyDistributor.EnergyDistribution(gameObject);

        if (energy < 1)
        {
            Destroy(gameObject);
        }

        energy--;
    }
}
