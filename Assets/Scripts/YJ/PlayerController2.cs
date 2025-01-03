using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    public Transform cameraRig; // Camera Rig

    public Transform leftWheel; // Left Wheel
    public Transform rightWheel; // Right Wheel

    [SerializeField] private InputActionProperty leftControllerRotationAction; // Left Controller Rotation
    [SerializeField] private InputActionProperty rightControllerRotationAction; // Right Controller Rotation
    [SerializeField] private InputActionProperty leftControllerVelocityAction;  // Left Controller Velocity
    [SerializeField] private InputActionProperty rightControllerVelocityAction; // Right Controller Velocity
    [SerializeField] private InputActionProperty leftControllerTriggerAction;   // Left Trigger
    [SerializeField] private InputActionProperty rightControllerTriggerAction;  // Right Trigger
    [SerializeField] private InputActionProperty leftControllerGripAction;      // Left Grip
    [SerializeField] private InputActionProperty rightControllerGripAction;     // Right Grip

    [SerializeField] private float moveSpeed = 1.0f;          // Movement Speed
    [SerializeField] private float accelerationFactor = 2.0f; // Acceleration Factor
    [SerializeField] private float dampingFactor = 0.99f;     // Damping Factor (Inertia)
    [SerializeField] private float wheelRotationSpeed = 100f; // Wheel Rotation Speed

    private Vector3 velocity = Vector3.zero; // Current Velocity
    private Quaternion initialLeftControllerRotation;  // Initial Left Controller Rotation
    private Quaternion initialRightControllerRotation; // Initial Right Controller Rotation
    private bool isLeftRotating = false;  // Is Left Rotating
    private bool isRightRotating = false; // Is Right Rotating

    private void Update()
    {
        // Combine inputs from both controllers for movement
        bool isMoving = HandleMovement();

        bool leftRotated = false;
        bool rightRotated = false;

        if (!isMoving)
        {
            // Handle rotation for each controller
            leftRotated = HandleRotation(ref initialLeftControllerRotation, ref isLeftRotating, true);
            rightRotated = HandleRotation(ref initialRightControllerRotation, ref isRightRotating, false);
        }

        // Sync wheel rotation
        SyncWheelRotation(leftRotated, rightRotated);

        // Apply braking or damping
        ApplyBrakingOrDamping(isMoving);

        // Apply movement
        Vector3 newPosition = cameraRig.position + velocity * Time.deltaTime;

        // Prevent movement below the z-axis
        if (newPosition.z < 0)
        {
            newPosition.z = 0;
            velocity.z = Mathf.Max(0, velocity.z);
        }

        cameraRig.position = newPosition;
    }

    private bool HandleMovement()
    {
        // Get velocity from controllers
        Vector3 leftControllerVelocity = leftControllerVelocityAction.action.ReadValue<Vector3>();
        Vector3 rightControllerVelocity = rightControllerVelocityAction.action.ReadValue<Vector3>();

        // Get trigger states
        bool leftTriggerPressed = leftControllerTriggerAction.action.IsPressed();
        bool rightTriggerPressed = rightControllerTriggerAction.action.IsPressed();

        Vector3 combinedMovement = Vector3.zero;

        if (leftTriggerPressed)
        {
            combinedMovement += leftControllerVelocity;
        }

        if (rightTriggerPressed)
        {
            combinedMovement += rightControllerVelocity;
        }

        float acceleration = combinedMovement.magnitude * accelerationFactor;

        // Move backward if combined movement z is negative
        if (combinedMovement.z < -0.1f)
        {
            velocity += cameraRig.forward * (-combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true; // Moving
        }
        // Move forward if combined movement z is positive
        else if (combinedMovement.z > 0.1f)
        {
            velocity -= cameraRig.forward * (combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true; // Moving
        }

        return false; // Not moving
    }

    private bool HandleRotation(ref Quaternion initialRotation, ref bool isRotating, bool isRightDirectionOnly)
    {
        bool hasRotated = false;

        // Check grip button state
        bool gripPressed = isRightDirectionOnly
            ? rightControllerGripAction.action.IsPressed()
            : leftControllerGripAction.action.IsPressed();

        if (gripPressed && !isRotating)
        {
            initialRotation = isRightDirectionOnly
                ? rightControllerRotationAction.action.ReadValue<Quaternion>()
                : leftControllerRotationAction.action.ReadValue<Quaternion>();

            isRotating = true;
        }

        if (!gripPressed && isRotating)
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Quaternion currentRotation = isRightDirectionOnly
                ? rightControllerRotationAction.action.ReadValue<Quaternion>()
                : leftControllerRotationAction.action.ReadValue<Quaternion>();

            Quaternion rotationDelta = currentRotation * Quaternion.Inverse(initialRotation);

            float yawDelta = Mathf.DeltaAngle(0, rotationDelta.eulerAngles.y);

            if ((isRightDirectionOnly && yawDelta > 0) || (!isRightDirectionOnly && yawDelta < 0))
            {
                Vector3 pivot = isRightDirectionOnly ? leftWheel.position : rightWheel.position;
                cameraRig.RotateAround(pivot, Vector3.up, yawDelta);
                hasRotated = true;
            }

            initialRotation = currentRotation;
        }

        return hasRotated;
    }

    private void SyncWheelRotation(bool leftRotated, bool rightRotated)
    {
        float forwardSpeed = velocity.z;
        float rotationSpeed = forwardSpeed * wheelRotationSpeed;

        leftWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        rightWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        if (leftRotated)
        {
            rightWheel.Rotate(Vector3.right, wheelRotationSpeed * Time.deltaTime);
        }

        if (rightRotated)
        {
            leftWheel.Rotate(Vector3.right, wheelRotationSpeed * Time.deltaTime);
        }
    }

    private void ApplyBrakingOrDamping(bool isMoving)
    {
        if (isMoving) return;

        bool leftTriggerPressed = leftControllerTriggerAction.action.IsPressed();
        bool rightTriggerPressed = rightControllerTriggerAction.action.IsPressed();

        if ((leftTriggerPressed || rightTriggerPressed) && velocity.magnitude < 0.5f)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, 0.1f);
        }
        else
        {
            velocity *= dampingFactor;
        }
    }
}
