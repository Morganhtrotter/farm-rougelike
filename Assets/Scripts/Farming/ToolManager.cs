using UnityEngine;
using UnityEngine.InputSystem;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    [field: SerializeField] public ToolType ActiveTool { get; private set; } = ToolType.Hoe;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        // Temporary hotkeys (replace with UI later)
        if (Keyboard.current.digit1Key.wasPressedThisFrame) ActiveTool = ToolType.Hoe;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) ActiveTool = ToolType.Water;
    }
}
