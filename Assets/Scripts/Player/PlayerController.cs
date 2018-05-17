using UnityEngine;

/// <summary>
/// Handles player movements and rotation
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Walking speed
    /// </summary>
    [SerializeField] private float speed = 6.0F;

    /// <summary>
    /// Jump speed
    /// </summary>
    [SerializeField] private float jumpSpeed = 8.0F;

    /// <summary>
    /// Falling speed (fake gravity)
    /// </summary>
    [SerializeField] private float gravity = 20.0F;

    [SerializeField] private float cameraRotationLimit = 85f;

    /// <summary>
    /// Mouse look sensitivity
    /// </summary>
    [SerializeField] private float lookSensitivity = 50f;

    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    /// <summary>
    /// Player camera to rotate
    /// </summary>
    [SerializeField] private Camera cam;

    /// <summary>
    /// Final move direction vector
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        Move();
        Rotate();
        CamRotate();
    }

    void Move()
    {
        CharacterController controller = GetComponent<CharacterController>();

        // Player movement
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

            if(GameMenu.isOn)
                moveDirection = Vector3.zero;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void Rotate()
    {
        // Player rotation (Pitch)
        if (!GameMenu.isOn)
        {
            Vector3 rotation = new Vector3(0f, Input.GetAxisRaw("Mouse X"), 0f) * lookSensitivity * Time.deltaTime;
            transform.Rotate(rotation);
        }
    }

    void CamRotate()
    {
        // Camera rotation (Yaw)
        if (cam && !GameMenu.isOn)
        {
            cameraRotationX = Input.GetAxisRaw("Mouse Y") * lookSensitivity * Time.deltaTime;
            // Set our rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply our rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }
}
