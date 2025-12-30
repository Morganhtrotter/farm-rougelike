using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private readonly Dictionary<Vector2Int, TileData> _tiles = new();

    public TileData GetOrCreate(Vector2Int cell)
    {
        if (!_tiles.TryGetValue(cell, out var data))
        {
            data = new TileData();
            _tiles[cell] = data;
        }
        return data;
    }

    public bool TryGet(Vector2Int cell, out TileData data) => _tiles.TryGetValue(cell, out data);

    public void Clear() => _tiles.Clear();
}
