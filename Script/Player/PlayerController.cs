using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;

    [Header("Jump")]
    public float jumpPower;
    public LayerMask groundedLayerMask;
    public float checkGroundRayDistance;

    [Header("Look")]
    public Transform cameraContainer;
    public float mixXLook;
    public float maxXlook;
    private float camCurXRot;
    public float lookSensitivity;

    [Header("Inventory")]
    public Action inventory;
    public bool canLook;

    [Header("Setting")]
    private bool setFinder;
    public GameObject setObj;
    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        setFinder = false;
        setObj.SetActive(false);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook == true)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, mixXLook, maxXlook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnInventroy(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    public void OnSetting(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!setFinder)
            {
                setObj.SetActive(true);
                setFinder = true;
                ToggleCursor();
            }
            else
            {
                setObj.SetActive(false);
                setFinder = false;
                ToggleCursor();
            }
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public bool IsGrounded()
    {
        Ray[] rays = new Ray[4];
        rays[0] = new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down);
        rays[1] = new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down);
        rays[2] = new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down);
        rays[3] = new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down);

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], checkGroundRayDistance, groundedLayerMask))
            {
                return true;
            }
        }
        return false;
    }
}