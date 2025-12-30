using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;   // NEW

public class TileHighlighter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap overlayTilemap;

    [Header("Tiles")]
    [SerializeField] private TileBase highlightTile;

    private Vector3Int _lastCell = new(int.MinValue, int.MinValue, 0);

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera == null || grid == null || overlayTilemap == null || highlightTile == null)
            return;

        // New Input System mouse position
        if (Mouse.current == null) return;
        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0f));
        mouseWorld.z = 0f;

        Vector3Int cell = grid.WorldToCell(mouseWorld);

        if (cell == _lastCell) return;

        // Clear old highlight
        if (_lastCell.x != int.MinValue)
            overlayTilemap.SetTile(_lastCell, null);

        // Set new highlight
        overlayTilemap.SetTile(cell, highlightTile);
        overlayTilemap.RefreshTile(cell);

        _lastCell = cell;
    }
}
