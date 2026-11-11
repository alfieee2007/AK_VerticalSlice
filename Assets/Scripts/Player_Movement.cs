
using UnityEngine;
using UnityEngine.InputSystem;

public class CapsuleMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;

    [Header("Camera")]
    public Transform cameraTransform;

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
                input.y += 1;

            if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
                input.y -= 1;

            if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
                input.x += 1;

            if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
                input.x -= 1;
        }

        if (cameraTransform == null)
            return;

        // Get camera directions
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Calculate camera-relative movement
        Vector3 movement =
        (forward * input.y + right * input.x).normalized;

        // Move player
        transform.position +=
        movement * moveSpeed * Time.deltaTime;

        // Rotate player towards movement
        if (movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
            Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
            );
        }
    }
}


