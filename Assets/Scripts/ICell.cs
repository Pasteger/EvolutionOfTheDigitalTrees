using System.Collections.Generic;
using UnityEngine;

public abstract class ICell
{
    public const int CellScale = 1;
    public int ID { get; set; }
    public int Energy { get; set; }
    public Vector3Int Position { get; set; }
    public CellType Type { get; set; }

    protected List<Dictionary<Vector3, int>> Genome { get; set; }
    protected int Lifespan { get; set; }
    protected LifeExecutor LifeExecutor { get; set; }   
    protected GameObject Ground { get; set; }
    
    public virtual void Life() {}
    protected void Death()
    {
        LifeExecutor.SetCell(Position, CellType.Air);
        LifeExecutor.RemoveCellBehavior(this);
    }
}
