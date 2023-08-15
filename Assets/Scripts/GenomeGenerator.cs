using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenomeGenerator : MonoBehaviour
{
    public GameObject seed;
    public List<Dictionary<Vector3, int>> genome = new();
    public static List<int> ids = new();

    private List<Dictionary<Vector3, int>> ManualFilling()
    {
        List<Dictionary<Vector3, int>> theGenome = new();

        string genomeStritg =
            "\n" +
            "00: 13, 29, 14, 12; \n" +
            "01: 29, 29, 29, 29; \n" +
            "02: 29, 09, 02, 29; \n" +
            "03: 29, 29, 29, 29; \n" +
            "04: 29, 29, 29, 29; \n" +
            "05: 29, 29, 29, 29; \n" +
            "06: 29, 29, 29, 29; \n" +
            "07: 29, 11, 29, 29; \n" +
            "08: 15, 29, 29, 29; \n" +
            "09: 29, 29, 00, 09; \n" +
            "10: 29, 29, 29, 29; \n" +
            "11: 06, 29, 02, 08; \n" +
            "12: 29, 07, 29, 29; \n" +
            "13: 08, 29, 29, 29; \n" +
            "14: 29, 29, 29, 29; \n" +
            "15: 29, 29, 09, 09; \n";

        for (int i = 0; i < 16; i++)
        {
            string line = genomeStritg.Substring(i * 21, 20);
            string genOne = line.Substring(5, 2);
            string genTwo = line.Substring(9, 2);
            string genThree = line.Substring(13, 2);
            string genFour = line.Substring(17, 2);

            Dictionary<Vector3, int> gen = new()
            {
                { Vector3.up, int.Parse(genOne) },
                { Vector3.down, int.Parse(genTwo) },
                { Vector3.right, int.Parse(genThree) },
                { Vector3.left, int.Parse(genFour) }
            };
            theGenome.Add(gen);
        }

        return theGenome;
    }

    private List<Dictionary<Vector3, int>> RandomFilling()
    {
        List<Dictionary<Vector3, int>> theGenome = new();
        for (int i = 0; i < 16; i++)
        {
            Dictionary<Vector3, int> gen = new()
            {
                { Vector3.up, Random.Range(0, 30) },
                { Vector3.down, Random.Range(0, 30) },
                { Vector3.right, Random.Range(0, 30) },
                { Vector3.left, Random.Range(0, 30) }
            };
            theGenome.Add(gen);
        }
        return theGenome;
    }

    private void Awake()
    {
        genome = ManualFilling();

        seed.GetComponent<ICell>().genome = genome;
        seed.GetComponent<ICell>().energy = 50;
    }

    public static int GetId()
    {
        int id;
        do
        {
            id = Random.Range(-2147483648, 2147483647);
        }
        while (ids.Contains(id));
        ids.Add(id);

        return id;
    }

    public static void RemoveId(int id)
    {
        if (ids.Contains(id))
        {
            ids.Remove(id);
        }
    }
}
