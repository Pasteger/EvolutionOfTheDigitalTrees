using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SproutBehaviour : MonoBehaviour, ICell
{
    public int id { get; set; }
    public int energy { get; set; }
    public List<Dictionary<Vector3, int>> genome { get; set; }
    public int lifespan { get; set; }

    public GameObject logPrefab;
    public GameObject SproutPrefab;
    public GameObject SeedPrefab;
    public int activeGen;

    void FixedUpdate()
    {
        lifespan--;
        EnergyDistributor.EnergyDistribution(gameObject);

        if (lifespan < 1)
        {
            GameObject seed = Instantiate(SeedPrefab, transform.position, new Quaternion(0, 0, 0, 0));
            seed.GetComponent<ICell>().genome = BetrayGenome();
            seed.GetComponent<ICell>().energy = energy;
            seed.GetComponent<ICell>().id = id;

            Destroy(gameObject);
        }
        else
        {
            if (energy > 15)
            {
                Dictionary<Vector3, int> gen = genome[activeGen];

                int numberDescendants = 0;

                if (gen[Vector3.up] < 16)
                    numberDescendants++;
                if (gen[Vector3.down] < 16)
                    numberDescendants++;
                if (gen[Vector3.right] < 16)
                    numberDescendants++;
                if (gen[Vector3.left] < 16)
                    numberDescendants++;

                if (gen[Vector3.up] < 16)
                    Grow(Vector3.up, gen[Vector3.up], numberDescendants);
                if (gen[Vector3.down] < 16)
                    Grow(Vector3.down, gen[Vector3.down], numberDescendants);
                if (gen[Vector3.right] < 16)
                    Grow(Vector3.right, gen[Vector3.right], numberDescendants);
                if (gen[Vector3.left] < 16)
                    Grow(Vector3.left, gen[Vector3.left], numberDescendants);

                GameObject log = Instantiate(logPrefab, transform.position, new Quaternion(0, 0, 0, 0));
                log.GetComponent<ICell>().genome = genome;
                log.GetComponent<ICell>().energy = energy;
                log.GetComponent<ICell>().lifespan = lifespan;

                Destroy(gameObject);
            }

        }

        if (energy < 1)
        {
            Destroy(gameObject);
        }

        energy--;
    }

    void Grow(Vector3 direction, int gen, int numberDescendants)
    {
        GameObject newSprout;
        Vector3 newPosition = gameObject.transform.position;

        if (direction.Equals(Vector3.up))
        {
            newPosition.y += 0.25f;
        }
        else if (direction.Equals(Vector3.down))
        {
            newPosition.y -= 0.25f;
        }
        else if (direction.Equals(Vector3.right))
        {
            newPosition.x += 0.25f;
        }
        else
        {
            newPosition.x -= 0.25f;
        }

        if (!FindBlock(transform.position, direction))
        {
            newSprout = Instantiate(SproutPrefab, newPosition, new Quaternion(0, 0, 0, 0));
            int newEnergy = (energy - 5) / numberDescendants;

            newSprout.GetComponent<ICell>().genome = genome;
            newSprout.GetComponent<ICell>().energy = newEnergy;
            newSprout.GetComponent<ICell>().lifespan = lifespan;
            newSprout.GetComponent<SproutBehaviour>().activeGen = gen;

            energy -= newEnergy;
        }
    }

    private bool FindBlock(Vector3 position, Vector3 direction)
    {
        Ray ray = new(position, direction);
        //Debug.DrawRay(position, direction, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.distance < 0.5;
        }
        return false;
    }

    private List<Dictionary<Vector3, int>> BetrayGenome()
    {
        List<Dictionary<Vector3, int>> theGemon = new();

        foreach (Dictionary<Vector3, int> gen in genome)
        {
            Dictionary<Vector3, int> newGen = new();
            int mutatingGene = -1;

            if (Random.Range(0, 25) == 1)
            {
                mutatingGene = Random.Range(0, 4);
            }

            int upGenMutationValue = gen[Vector3.up];
            int downGenMutationValue = gen[Vector3.down];
            int rightGenMutationValue = gen[Vector3.right];
            int leftGenMutationValue = gen[Vector3.left];

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

            theGemon.Add(newGen);
        }

        /*for (int i = 0; i < 16; i++)
        {
            Dictionary<Vector3, int> oldGen = genome[i];
            Dictionary<Vector3, int> youngGen = theGemon[i];

            if (oldGen[Vector3.up] != youngGen[Vector3.up])
            {
                Debug.Log(i + ": (1): " + oldGen[Vector3.up] + " " + youngGen[Vector3.up]);
            }
            if (oldGen[Vector3.down] != youngGen[Vector3.down])
            {
                Debug.Log(i + ": (2): " + oldGen[Vector3.down] + " " + youngGen[Vector3.down]);
            }
            if (oldGen[Vector3.right] != youngGen[Vector3.right])
            {
                Debug.Log(i + ": (3): " + oldGen[Vector3.right] + " " + youngGen[Vector3.right]);
            }
            if (oldGen[Vector3.left] != youngGen[Vector3.left])
            {
                Debug.Log(i + ": (4): " + oldGen[Vector3.left] + " " + youngGen[Vector3.left]);
            }
        }*/

        return theGemon;
    }
}
