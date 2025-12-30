using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerClickToMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private FarmTileManager farmTileManager;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool hasTarget;

    private Camera mainCam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        targetPosition = rb.position;

        if (farmTileManager == null)
            farmTileManager = FindFirstObjectByType<FarmTileManager>();

        if (farmTileManager == null)
            Debug.LogError("No FarmTileManager found in scene. Add one and/or assign it on PlayerClickToMove.");
    }

    private void Update()
    {
        // Left click sets a new target
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            targetPosition = new Vector2(mouseWorld.x, mouseWorld.y);
            hasTarget = true;
            
            if (farmTileManager != null)
                farmTileManager.HandleTileClick(targetPosition);
        }
    }

    private void FixedUpdate()
    {
        if (!hasTarget) return;

        Vector2 currentPos = rb.position;
        Vector2 toTarget = targetPosition - currentPos;
        float distance = toTarget.magnitude;

        if (distance <= stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            hasTarget = false;
            return;
        }

        Vector2 moveDir = toTarget.normalized;
        rb.linearVelocity = moveDir * moveSpeed;
    }
}
