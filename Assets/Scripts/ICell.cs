using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICell
{
    int id { get; set; }
    int energy { get; set; }
    List<Dictionary<Vector3, int>> genome { get; set; }
    int lifespan { get; set; }
}
