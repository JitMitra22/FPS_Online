using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform viewPoint;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 1f;
    public bool invertMouse = false;

    private float _verticalRotationStore;
    private Vector2 _mouseInput;

    public float moveSpeed = 5f, runSpeed = 8f;
    private float _activeMoveSpeed;

    private Vector3 _moveDirection, _movement;

    public CharacterController charCon;

    private Camera _camera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _camera = Camera.main;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void LateUpdate()
    {
        _camera.transform.position = viewPoint.position;
        _camera.transform.rotation = viewPoint.rotation;
    }

    private void PlayerMovement()
    {
        _mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        float xRotationEuler = transform.rotation.eulerAngles.x;
        float yRotationEuler = transform.rotation.eulerAngles.y;
        float zRotationEuler = transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(xRotationEuler, yRotationEuler + _mouseInput.x, zRotationEuler);

        float yRotationEulerView = viewPoint.rotation.eulerAngles.y;
        float zRotationEulerView = viewPoint.rotation.eulerAngles.z;

        if (invertMouse)
        {
            _verticalRotationStore += _mouseInput.y;
        }
        else
        {
            _verticalRotationStore -= _mouseInput.y;
        }
        
        _verticalRotationStore = Mathf.Clamp(_verticalRotationStore, -60f, 60f);

        viewPoint.rotation = Quaternion.Euler(_verticalRotationStore, yRotationEulerView, zRotationEulerView);

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float forwardInput = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector3(horizontalInput, 0f, forwardInput);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _activeMoveSpeed = runSpeed;
        }
        else
        {
            _activeMoveSpeed = moveSpeed;
        }
        _movement = ((transform.forward * _moveDirection.z) + (transform.right * _moveDirection.x)).normalized * _activeMoveSpeed;
        charCon.Move(_movement * Time.deltaTime);
    }
}
