using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICell
{
    int ID { get; set; }
    int Energy { get; set; }
    List<Dictionary<Vector3, int>> Genome { get; set; }
    int Lifespan { get; set; }

    void Life();
}
