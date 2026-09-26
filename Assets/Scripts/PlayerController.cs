using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Public References")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference lookAction;
    [Tooltip("Attack Action")]
    public InputActionReference LeftMouseAction;
    public Transform holdPoint;
    
    public CharacterController controller;
    
    public LayerMask WhatIsStock;
    
    [Header("Private References")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private GameObject heldPickUpObject;
    
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
        Vector3 horizontalMove = transform.forward * moveInput.x;
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

                if (Physics.Raycast(ray, out hit, interactionRange, WhatIsStock))
                {
                    heldPickUpObject = hit.transform.gameObject;
                    heldPickUpObject.transform.SetParent(holdPoint);
                    heldPickUpObject.transform.localPosition = new Vector3(0.2f,0f,0f);
                    heldPickUpObject.transform.localRotation = Quaternion.identity;
                    heldPickUpObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
        else
        {
            //Leaving or throwing Object
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                var heldObjectRigidBody = heldPickUpObject.GetComponent<Rigidbody>();
                heldPickUpObject.transform.SetParent(null);
                heldPickUpObject = null;
                heldObjectRigidBody.isKinematic = false;
                heldObjectRigidBody.AddForce(_camera.transform.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
