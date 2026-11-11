using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 4f;
    public float height = 1.6f;
    public float shoulderOffset = 0.6f;

    [Header("Mouse Settings")]
    public float sensitivity = 0.15f;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    [Header("Smoothing")]
    public float smoothSpeed = 12f;

    [Header("Camera Collision")]
    public float collisionRadius = 0.2f;
    public float collisionPadding = 0.1f;

    private float yaw;
    private float pitch = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Read mouse movement
        if (Mouse.current != null)
        {
            Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * sensitivity;
            pitch -= mouseDelta.y * sensitivity;

            pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
            );
        }

        // Calculate camera rotation
        Quaternion rotation =
        Quaternion.Euler(pitch, yaw, 0f);

        // Calculate camera pivot position
        Vector3 pivot = target.position +
        Vector3.up * height;

        // Calculate desired camera position
        Vector3 shoulderPosition =
        rotation * Vector3.right * shoulderOffset;

        Vector3 cameraPosition =
        pivot +
        shoulderPosition -
        rotation * Vector3.forward * distance;

        // Prevent camera going through walls
        Vector3 direction =
        cameraPosition - pivot;

        float actualDistance = distance;

        if (Physics.SphereCast(
        pivot + shoulderPosition,
        collisionRadius,
        rotation * Vector3.forward,
        out RaycastHit hit,
        distance,
        Physics.DefaultRaycastLayers,
        QueryTriggerInteraction.Ignore))
        {
            actualDistance = Mathf.Max(
            0.1f,
            hit.distance - collisionPadding
            );
        }

        cameraPosition =
        pivot +
        shoulderPosition -
        rotation * Vector3.forward * actualDistance;

        // Smooth camera movement
        transform.position = Vector3.Lerp(
        transform.position,
        cameraPosition,
        smoothSpeed * Time.deltaTime
        );

        // Look towards the player
        Quaternion lookRotation =
        Quaternion.LookRotation(
        pivot - transform.position
        );

        transform.rotation = Quaternion.Slerp(
        transform.rotation,
        lookRotation,
        smoothSpeed * Time.deltaTime
        );
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}