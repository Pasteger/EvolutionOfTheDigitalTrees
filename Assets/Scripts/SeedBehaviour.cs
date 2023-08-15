using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehaviourt : MonoBehaviour, ICell
{
    public int id { get; set; }
    public int energy { get; set; }
    public List<Dictionary<Vector3, int>> genome { get; set; }
    public int lifespan { get; set; }
    public GameObject ground;
    public GameObject SproutPrefab;

    void Start()
    {
        GenomeGenerator.RemoveId(id);
    }

    void FixedUpdate()
    {
        if (energy < 1)
        {
            Destroy(gameObject);
        }
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hitInfo))
        {
            if (hitInfo.distance > 0.125)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
            }
            else if (hitInfo.collider.gameObject.name.Equals(ground.name))
            {
                GameObject newSprout = Instantiate(SproutPrefab, transform.position, new Quaternion(0, 0, 0, 0));
                newSprout.GetComponent<ICell>().id = GenomeGenerator.GetId();
                newSprout.GetComponent<ICell>().genome = genome;
                newSprout.GetComponent<ICell>().energy = energy;
                newSprout.GetComponent<ICell>().lifespan = 40;
                newSprout.GetComponent<SproutBehaviour>().activeGen = 0;

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        energy--;
    }
}
