
using UnityEngine;
using UnityEngine.InputSystem;

public class CapsuleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Vector2 moveInput;

    void Update()
    {
        // Read keyboard input
        moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
                moveInput.y += 1;

            if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
                moveInput.y -= 1;

            if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
                moveInput.x += 1;

            if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
                moveInput.x -= 1;
        }

        // Move the Capsule
        Vector3 movement = new Vector3(
        moveInput.x,
        0f,
        moveInput.y
        ).normalized;

        transform.Translate(
        movement * moveSpeed * Time.deltaTime,
        Space.World
        );
    }
}

