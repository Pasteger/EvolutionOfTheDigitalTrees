using System.Collections.Generic;
using UnityEngine;

public class LogBehaviour : MonoBehaviour, ICell
{
    public int ID { get; set; }
    public int Energy { get; set; }
    public List<Dictionary<Vector3, int>> Genome { get; set; }
    public int Lifespan { get; set; }

    void FixedUpdate()
    {
        Lifespan--;
        switch (Lifespan)
        {
            case < 1:
                Destroy(gameObject);
                break;
            case 1:
                EnergyDistributor.LegacyTransfer(ID, Energy);
                Energy = 0;
                break;
        }

        if (Energy > 5)
        {
            Energy += 100;
        }

        EnergyDistributor.EnergyDistribution(this, transform.position);

        if (Energy < 1)
        {
            Destroy(gameObject);
        }

        Energy--;
    }
}
