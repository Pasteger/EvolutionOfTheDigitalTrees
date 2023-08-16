using System.Collections.Generic;
using UnityEngine;

public class LiveExecutor : MonoBehaviour
{
    private void FixedUpdate()
    {
        var cells = GameObject.FindGameObjectsWithTag("Cell");
        var sprouts = GameObject.FindGameObjectsWithTag("Sprout");
        var allCells = new List<GameObject>();
        
        allCells.AddRange(cells);
        allCells.AddRange(sprouts);
        
        foreach (var cell in allCells)
        {
            cell.GetComponent<ICell>().Life();
        }
    }
}
