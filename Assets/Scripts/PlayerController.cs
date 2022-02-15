using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject player;

    private const float yClampAngle = 90f;
    private const float speed = 6f;

    private CharacterController controller;
    private float mouseSensitivity = 100f;
    private float yRotation = 0f;
    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        CalculateKeyActions();
        CalculateMovements();
        CalculateMouseActions();
    }

    void CalculateKeyActions()
    {
        bool IsPressedTab = Input.GetKey(KeyCode.Tab);
    }

    void CalculateMovements()
    {
        //walk calculating
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        Vector3 move = player.transform.right * axisX + player.transform.forward * axisZ;
        controller.Move(move * speed * Time.deltaTime);
    }

    void CalculateMouseActions()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -yClampAngle, yClampAngle); // lock camera rotation by Y
        camera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        player.transform.Rotate(Vector3.up * mouseX);

        MouseButtonsPressing();
    }

    void MouseButtonsPressing()
    {
        bool IsPressedLMB = Input.GetButtonUp("Fire1"); // detecting left button press
        bool IsPressedRMB = Input.GetButtonUp("Fire2"); // detecting right button press

    }
}
