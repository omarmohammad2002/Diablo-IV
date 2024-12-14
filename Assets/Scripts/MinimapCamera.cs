using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;  // Reference to the player transform
    public Vector3 offset;    // Offset of the minimap camera from the player
    public float height = 10f; // Fixed height for the minimap camera (optional)

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned.");
        }
    }

    private void LateUpdate()
    {
        // Update the position of the minimap camera to follow the player's position with an offset
        if (player != null)
        {
            Vector3 newPosition = player.position + offset;
            newPosition.y = height;  // Keep the camera at a fixed height

            transform.position = newPosition;

            // Keep the camera rotation fixed
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Set rotation to top-down view
        }
    }
}
