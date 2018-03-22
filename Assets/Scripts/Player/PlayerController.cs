using UnityEngine;

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

    /// <summary>
    /// Mouse look sensitivity
    /// </summary>
    [SerializeField] private float lookSensitivity = 100f;

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

        CharacterController controller = GetComponent<CharacterController>();

        // Player movement
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // Player rotation (Pitch)
        Vector3 rotation = new Vector3(0f, Input.GetAxisRaw("Mouse X"), 0f) * lookSensitivity * Time.deltaTime;
        transform.Rotate(rotation);

        // Camera rotation (Yaw)
        if (cam)
        {
            Vector3 camRotation = new Vector3(Input.GetAxisRaw("Mouse Y"), 0f, 0f) * lookSensitivity * Time.deltaTime;
            cam.transform.Rotate(-camRotation);
        }
    }
}
