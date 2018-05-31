/// ETML
/// Author: Gil Balsiger
/// Date: 20.04.2018
/// Summary: Handles player movements and rotation

using UnityEngine;
using UnityEngine.Networking;

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
    [SerializeField] private Camera fpsCam;

    [SerializeField] private Camera tpsCam;

    [SerializeField] private Camera weaponCam;

    private Camera activeCam;

    /// <summary>
    /// Final move direction vector
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        tpsCam.enabled = false;
        activeCam = fpsCam;
    }

    void Update()
    {
        Move();
        Rotate();
        CamRotate();

        // Camera switching
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (fpsCam.enabled)
            {
                tpsCam.enabled = true;
                fpsCam.enabled = false;
                activeCam = tpsCam;
                weaponCam.enabled = false;
            }
            else
            {
                tpsCam.enabled = false;
                fpsCam.enabled = true;
                activeCam = fpsCam;
                weaponCam.enabled = true;
            }
        }
    }

    /// <summary>
    /// Player movement on X and Z
    /// </summary>
    void Move()
    {
        CharacterController controller = GetComponent<CharacterController>();
        // Player movement
        if (controller.isGrounded)
        {
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                GetComponent<Animator>().SetBool("Run", true);
            else
                GetComponent<Animator>().SetBool("Run", false);

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump") && !GameMenu.isOn)
            {
                moveDirection.y = jumpSpeed;
                GetComponent<NetworkAnimator>().SetTrigger("Jump");
            }

            if (GameMenu.isOn)
                moveDirection = Vector3.zero;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Player rotation over Y
    /// </summary>
    void Rotate()
    {
        // Player rotation (Pitch)
        if (!GameMenu.isOn)
        {
            Vector3 rotation = new Vector3(0f, Input.GetAxisRaw("Mouse X"), 0f) * lookSensitivity * Time.deltaTime;
            transform.Rotate(rotation);
        }
    }

    /// <summary>
    /// Player camera rotation over X
    /// </summary>
    void CamRotate()
    {
        // Camera rotation (Yaw)
        if (activeCam && !GameMenu.isOn)
        {
            cameraRotationX = Input.GetAxisRaw("Mouse Y") * lookSensitivity * Time.deltaTime;
            // Set our rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply our rotation to the transform of our camera
            activeCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    public Camera GetActiveCam()
    {
        return activeCam;
    }

    public Camera GetTpsCam()
    {
        return tpsCam;
    }
}
