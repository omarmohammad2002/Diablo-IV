using UnityEngine;

public class FreeFlyCamera : MonoBehaviour
{
    public float movementSpeed = 10f; // Speed of movement
    public float mouseSensitivity = 100f; // Mouse sensitivity
    private float rotationX = 0f; // Up-down rotation
    private float rotationY = 0f; // Left-right rotation

    void Update()
    {
        // Mouse rotation
        rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limit vertical rotation
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);

        // Movement
        float moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime; // A/D keys
        float moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;   // W/S keys
        float moveY = 0f;

        if (Input.GetKey(KeyCode.E)) moveY += movementSpeed * Time.deltaTime; // Up (E key)
        if (Input.GetKey(KeyCode.Q)) moveY -= movementSpeed * Time.deltaTime; // Down (Q key)

        transform.Translate(new Vector3(moveX, moveY, moveZ), Space.Self);
    }
}
