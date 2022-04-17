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
    private void Update()
    {
        MouseMovement();
    }

    private void MouseMovement()
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
        
    }
}
