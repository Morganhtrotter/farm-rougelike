using UnityEngine;

public class WorldGridModel : MonoBehaviour
{
    public GridData Data { get; private set; } = new GridData();

    public TileData GetTile(Vector2Int cell)
    {
        return Data.GetOrCreate(cell);
    }
}
