using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LifeExecutor : MonoBehaviour
{
    public GameObject ground;
    public GenomeGenerator GenomeGenerator;
    public EnergyDistributor EnergyDistributor;
    
    public readonly List<ICell> CellsBehaviours = new();
    private readonly List<ICell> _intermediateCellsBehavioursForAdd = new();
    private readonly List<ICell> _intermediateCellsBehavioursForRemove = new();
    
    private readonly CellType[] _cells = new CellType[MapWidth * MapHeight];
    
    private const int MapWidth = 500;
    private const int MapHeight = 500;
    
    private readonly List<Vector3> _vertices = new();
    private readonly List<Vector2> _uvs = new();
    private static int[] _triangles;
    
    private Mesh _mapMesh;
    
    private float _textureOffset;
    
    private static void InitTriangles()
    {
        _triangles = new int[65536 * 6 / 4];
        
        var verticesCount = 4;
        for (var i = 0; i < _triangles.Length; i += 6)
        {
            _triangles[i] = verticesCount - 4;
            _triangles[i+1] = verticesCount - 3;
            _triangles[i+2] = verticesCount - 2;
        
            _triangles[i+3] = verticesCount - 3;
            _triangles[i+4] = verticesCount - 1;
            _triangles[i+5] = verticesCount - 2;

            verticesCount += 4;
        }
    }
    
    private void Start()
    {
        GenomeGenerator = new GenomeGenerator();
        EnergyDistributor = new EnergyDistributor();
        
        InitTriangles();
        
        _textureOffset = 16f / GetComponent<MeshRenderer>().materials[0].mainTexture.width;
        
        _mapMesh = new Mesh();
        
        RegenerateMesh();


        GetComponent<MeshFilter>().mesh = _mapMesh;
        
        
        
        CreateFirstSeed();
        
    }

    private void RegenerateMesh()
    {
        _vertices.Clear();
        _uvs.Clear();
        
        for (var y = 0; y < MapHeight; y++)
        {
            for (var x = 0; x < MapWidth; x++)
            {
                GenerateCell(x, y);
            }
        }

        _mapMesh.triangles = Array.Empty<int>();
        _mapMesh.vertices = _vertices.ToArray();
        _mapMesh.uv = _uvs.ToArray();
        _mapMesh.SetTriangles(_triangles, 0, _vertices.Count * 6 / 4, 0, false);

        //_mapMesh.Optimize();
        _mapMesh.RecalculateNormals();

        var boundsSize = new Vector3(MapWidth, MapHeight) * ICell.CellScale;
        _mapMesh.bounds = new Bounds(boundsSize / 2, boundsSize);
        
        /*if (_mapMesh.vertices.Length == 0)
        {
            GetComponent<MeshCollider>().sharedMesh = null;
        }
        else
        {
            GetComponent<MeshCollider>().sharedMesh = _mapMesh;
        }*/
    }

    private void GenerateCell(int x, int y)
    {
        var index = GetIndexByPosition(x, y);
        
        if (_cells[index] == CellType.Air) return;
        
        var vectorXY = new Vector3(x, y, 0);
        
        _vertices.Add((new Vector3(0, 0, 0) + vectorXY) * ICell.CellScale);
        _vertices.Add((new Vector3(0, 1, 0) + vectorXY) * ICell.CellScale);
        _vertices.Add((new Vector3(1, 0, 0) + vectorXY) * ICell.CellScale);
        _vertices.Add((new Vector3(1, 1, 0) + vectorXY) * ICell.CellScale);
        
        var cellType = (float) _cells[index];
        
        _uvs.Add(new Vector2((cellType - 1) * _textureOffset, 0));
        _uvs.Add(new Vector2((cellType - 1) * _textureOffset, 1));
        _uvs.Add(new Vector2(cellType * _textureOffset, 0));
        _uvs.Add(new Vector2(cellType * _textureOffset, 1));
    }

    public void SetCell(Vector3Int position, CellType cellType)
    {
        var index = GetIndexByPosition(position.x, position.y);
        _cells[index] = cellType;
        RegenerateMesh();
    }

    public bool CheckNeighbourCell(Vector3Int position, Vector3 direction)
    {
        var index = GetIndexByPosition(position.x, position.y);
        if (direction == Vector3.up)
        {
            return _cells[index + MapWidth] == CellType.Air;
        }
        if (direction == Vector3.down)
        {
            return _cells[index - MapWidth] == CellType.Air;
        }
        if (direction == Vector3.right)
        {
            return _cells[index + 1] == CellType.Air;
        }
        if (direction == Vector3.left)
        {
            return _cells[index - 1] == CellType.Air;
        }

        return false;
    }
    
    private static int GetIndexByPosition(int x, int y)
    {
        return (x * MapWidth + y) / ICell.CellScale;
    }

    private void CreateFirstSeed()
    {
        CellsBehaviours.Add(new SeedBehaviour( this, ground, 
            new Vector3Int(MapWidth / 2, 0, 0), 50, 
            GenomeGenerator.GetGenome(), 50, GenomeGenerator.GetId()));
    }

    public ICell FindCellByPosition(Vector3 position)
    {
        return CellsBehaviours.FirstOrDefault(iCell => iCell.Position == position);
    }

    public void AddCellBehavior(ICell cell)
    {
        _intermediateCellsBehavioursForAdd.Add(cell);
    }

    public void RemoveCellBehavior(ICell cell)
    {
        if (CellsBehaviours.Contains(cell))
            _intermediateCellsBehavioursForRemove.Add(cell);
    }
    
    private void FixedUpdate()
    {
        //EditorApplication.isPaused = true;
        foreach (var log in CellsBehaviours.Where(behaviour => behaviour.Type == CellType.Log))
        {
            log.Life();
        }
        foreach (var sprout in CellsBehaviours.Where(behaviour => behaviour.Type == CellType.Sprout))
        {
            sprout.Life();
        }
        UpdateCellsCount();
        foreach (var seed in CellsBehaviours.Where(behaviour => behaviour.Type == CellType.Seed))
        {
            seed.Life();
        }
        UpdateCellsCount();
    }

    private void UpdateCellsCount()
    {
        CellsBehaviours.AddRange(_intermediateCellsBehavioursForAdd);
        foreach (var remoCell in _intermediateCellsBehavioursForRemove)
        {
            CellsBehaviours.Remove(remoCell);
        }
        
        _intermediateCellsBehavioursForAdd.Clear();
        _intermediateCellsBehavioursForRemove.Clear();
    }

    public List<ICell> GetSprouts(int id)
    {
        return CellsBehaviours.Where(iCell => iCell.Type == CellType.Sprout && iCell.ID == id).ToList();
    }
}
