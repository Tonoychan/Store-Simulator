using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Public References")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference lookAction;
    public Transform holdPoint;
    
    public CharacterController controller;
    
    public LayerMask whatIsStock;
    public LayerMask whatIsShelf;
    
    [Header("Private References")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private StockObject heldPickUpObject;
    
    
    [Header("Value Factor Controls")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float verticalAngle;
    [SerializeField]
    private float gravSpeedMultiplier;
    [SerializeField]
    private float interactionRange;
    [SerializeField]
    private float throwForce;
    
    [Header("Value For Debug")]
    [SerializeField]
    private float ySpeed;
    [SerializeField]
    private float horizontalRotation;
    [SerializeField]
    private float verticalRotation;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.Instance.updatePricePanel)
        {
            if (UIController.Instance.updatePricePanel.activeSelf)
            {
                return;
            }
        }

        //Looking-Controls
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        
        //Vertical Sideways Looking
        verticalRotation -= lookInput.y * Time.deltaTime * rotationSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalAngle, verticalAngle);
        
        //Horizontal Looking via Camera
        if (_camera != null)
            _camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        horizontalRotation += lookInput.x * Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
        
        //Movement
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 verticalMove = transform.forward * moveInput.y;
        Vector3 horizontalMove = transform.right * moveInput.x;
        Vector3 moveAmount = (horizontalMove + verticalMove).normalized;
        
        moveAmount *= (moveSpeed * Time.deltaTime);
        if (controller.isGrounded)
        {
            //Jumping
            ySpeed = 0;
            if (jumpAction.action.WasPressedThisFrame())
            {
                ySpeed =  jumpForce;
            }
        }
        ySpeed += (Physics.gravity.y * Time.deltaTime * gravSpeedMultiplier);
        moveAmount.y = ySpeed;
        //Final Move Amount
        controller.Move(moveAmount);
        
        //Checking For PickUp
        if (!heldPickUpObject)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
                {
                    heldPickUpObject = hit.transform.GetComponent<StockObject>();
                    heldPickUpObject.transform.SetParent(holdPoint);
                    heldPickUpObject.PickUp();
                    
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    heldPickUpObject = hit.transform.GetComponent<ShelfSpaceController>().GetItemFromStock();
                    if (heldPickUpObject != null)
                    {
                        heldPickUpObject.transform.SetParent(holdPoint);
                        heldPickUpObject.PickUp();
                    }
                }
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    hit.transform.GetComponent<ShelfSpaceController>().StartPriceUpdateFlow();
                }
            }
        }
        else
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    hit.transform.GetComponent<ShelfSpaceController>().PlaceItemOnStock(heldPickUpObject);
                    if (heldPickUpObject.isPlaced)
                    {
                        heldPickUpObject = null;
                    }
                }
            }

            //Leaving or throwing Object
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                heldPickUpObject.ReleaseItem();
                heldPickUpObject.transform.SetParent(null);
                heldPickUpObject.rigidbody.AddForce(_camera.transform.forward * throwForce, ForceMode.Impulse);
                heldPickUpObject = null;
            }
        }
    }
}
