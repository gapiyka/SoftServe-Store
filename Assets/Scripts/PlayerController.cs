using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject player;
    [SerializeField] private MenuController menu;

    private const float yClampAngle = 90f;
    private const float speed = 6f;
    private const float gravity = -6.67f;

    private CharacterController controller;
    private float mouseSensitivity = 100f;
    private float yRotation = 0f;
    private bool IsOpenedMenu = false;
    private AimType aimType;
    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!menu.IsMenuOpened)
        {
            CalculateKeyActions();
            CalculateMovements();
            CalculateMouseActions();
        }
    }

    void CalculateKeyActions()
    {
        bool IsPressedTab = Input.GetKeyDown(KeyCode.Tab);
        bool IsPressedSpace = Input.GetKeyDown(KeyCode.Space);

        if (IsPressedSpace) menu.OpenCart();
        if (IsPressedTab) menu.SwitchHints();
    }

    void CalculateMovements()
    {
        //walk calculating
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        Vector3 move = player.transform.right * axisX + player.transform.forward * axisZ;
        controller.Move(move * speed * Time.deltaTime);
        //adding gravity
        Vector3 velocity = new Vector3(0, gravity, 0);
        controller.Move(velocity * Time.deltaTime);
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
        bool IsPressedLMB = Input.GetButtonDown("Fire1"); // detecting left button press
        Transform aimObject = CalculateRaycastHit();
        if (IsPressedLMB)
        {
            if (aimType == AimType.Furniture) menu.OpenObjectMenu(aimObject);
            if (aimType == AimType.Register) menu.OpenCart();
        }
    }

    Transform CalculateRaycastHit()
    {
        const string furnitureTag = "Furniture";
        const string registerTag = "Register";
        const int castDistance = 4;
        RaycastHit hit;
        // point at center of screen
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, camera.nearClipPlane);
        Vector3 cameraCenter =
            camera.ScreenToWorldPoint(screenCenter);

        //check where looking camera
        if (Physics.Raycast(cameraCenter, camera.gameObject.transform.forward, out hit, castDistance))
        {
            if (hit.transform.CompareTag(furnitureTag)) aimType = AimType.Furniture;
            else if (hit.transform.CompareTag(registerTag)) aimType = AimType.Register;
            else aimType = AimType.Default;
        }
        else aimType = AimType.Default;
        menu.ChangeAimColor(aimType);

        return hit.transform;
    }
}
