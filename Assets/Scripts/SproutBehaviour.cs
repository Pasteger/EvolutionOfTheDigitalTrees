using System.Collections.Generic;
using UnityEngine;

public class SproutBehaviour : MonoBehaviour, ICell
{
    public int ID { get; set; }
    public int Energy { get; set; }
    public List<Dictionary<Vector3, int>> Genome { get; set; }
    public int Lifespan { get; set; }

    public GameObject logPrefab;
    public GameObject sproutPrefab;
    public GameObject seedPrefab;
    public int activeGen;

    void FixedUpdate()
    {
        Lifespan--;
        EnergyDistributor.EnergyDistribution(this, transform.position);

        if (Lifespan < 1)
        {
            var seed = Instantiate(seedPrefab, transform.position, Quaternion.identity);

            var seedCell = seed.GetComponent<ICell>();
            seedCell.Genome = BetrayGenome();
            seedCell.Energy = Energy;
            seedCell.ID = ID;

            Destroy(gameObject);
        }
        else
        {
            var gen = Genome[activeGen];
            if (Energy > 15)
            {
                var numberDescendants = 0;

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

                var log = Instantiate(logPrefab, transform.position, Quaternion.identity);

                var logCell = log.GetComponent<ICell>();
                
                logCell.Genome = Genome;
                logCell.Energy = Energy;
                logCell.Lifespan = Lifespan;

                Destroy(gameObject);
            }
        }

        if (Energy < 1)
        {
            Destroy(gameObject);
        }

        Energy--;
    }

    private void Grow(Vector3 direction, int gen, int numberDescendants)
    {
        var newPosition = gameObject.transform.position;

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

        if (FindBlock(transform.position, direction)) return;
        
        var newSprout = Instantiate(sproutPrefab, newPosition, Quaternion.identity);
        var newEnergy = (Energy - 5) / numberDescendants;

        var newSproutCell = newSprout.GetComponent<SproutBehaviour>();
        
        newSproutCell.Genome = Genome;
        newSproutCell.Energy = newEnergy;
        newSproutCell.Lifespan = Lifespan;
        newSproutCell.activeGen = gen;

        Energy -= newEnergy;
    }

    private bool FindBlock(Vector3 position, Vector3 direction)
    {
        if (Physics.Raycast(new Ray(position, direction), out var hit))
        {
            return hit.distance < 0.5;
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
