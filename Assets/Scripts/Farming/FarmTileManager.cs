using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmTileManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundTilemap;

    [Header("Tiles")]
    [SerializeField] private TileBase grassTile;
    [SerializeField] private TileBase tilledTile;
    [SerializeField] private TileBase wateredTile;

    // Stores the farming state per cell position
    private readonly Dictionary<Vector3Int, FarmTileState> stateByCell = new();

    // Optional: restrict farming to only specific base tiles
    private bool IsFarmableBaseTile(TileBase baseTile)
    {
        // You can expand this later (e.g., only allow farming on grass)
        return baseTile == grassTile || baseTile == tilledTile || baseTile == wateredTile;
    }

    public void HandleTileClick(Vector3 worldPosition)
    {
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPosition);
        TileBase currentTile = groundTilemap.GetTile(cellPos);

        if (currentTile == null) return;
        if (!IsFarmableBaseTile(currentTile)) return;

        if (ToolManager.Instance == null)
        {
            Debug.LogError("ToolManager.Instance is null. Add ToolManager to a GameObject in the scene (e.g., Managers).");
            return;
        }

        ToolType tool = ToolManager.Instance.ActiveTool;

        FarmTileState currentState = stateByCell.TryGetValue(cellPos, out var s) ? s : FarmTileState.None;

        switch (tool)
        {
            case ToolType.Hoe:
                // Hoe turns None -> Tilled (and watered becomes tilled if you hoe again)
                SetState(cellPos, FarmTileState.Tilled);
                break;

            case ToolType.Water:
                // Water only works on tilled soil
                if (currentState == FarmTileState.Tilled)
                    SetState(cellPos, FarmTileState.Watered);
                break;
        }
    }

    private void SetState(Vector3Int cellPos, FarmTileState newState)
    {
        stateByCell[cellPos] = newState;

        switch (newState)
        {
            case FarmTileState.None:
                groundTilemap.SetTile(cellPos, grassTile);
                break;

            case FarmTileState.Tilled:
                groundTilemap.SetTile(cellPos, tilledTile);
                break;

            case FarmTileState.Watered:
                groundTilemap.SetTile(cellPos, wateredTile);
                break;
        }
    }

    // Utility for later: reset watered tiles at end of day
    public void ResetWateredTiles()
    {
        // copy keys to avoid modifying collection during iteration
        var keys = new List<Vector3Int>(stateByCell.Keys);

        foreach (var cell in keys)
        {
            if (stateByCell[cell] == FarmTileState.Watered)
                SetState(cell, FarmTileState.Tilled);
        }
    }
}
