using System.Collections.Generic;
using UnityEngine;

public class SeedBehaviour : MonoBehaviour, ICell
{
    public int ID { get; set; }
    public int Energy { get; set; }
    public List<Dictionary<Vector3, int>> Genome { get; set; }
    public int Lifespan { get; set; }
    public GameObject ground;
    public GameObject sproutPrefab;

    void Start()
    {
        GenomeGenerator.RemoveId(ID);
    }

    public void Life()
    {
        if (Energy < 1)
        {
            Destroy(gameObject);
        }
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out var hitInfo))
        {
            if (hitInfo.distance > 0.125)
            {
                transform.position += new Vector3(0, -0.25f, 0);
            }
            else if (hitInfo.collider.gameObject.name.Equals(ground.name))
            {
                var newSprout = Instantiate(sproutPrefab, transform.position, Quaternion.identity);
                
                var newSproutCell = newSprout.GetComponent<SproutBehaviour>();
                
                newSproutCell.ID = GenomeGenerator.GetId();
                newSproutCell.Genome = Genome;
                newSproutCell.Energy = Energy;
                newSproutCell.Lifespan = 40;
                newSproutCell.activeGen = 0;

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        Energy--;
    }
}
