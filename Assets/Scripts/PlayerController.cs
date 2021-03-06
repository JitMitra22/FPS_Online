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

    public float jumpForce = 12f, gravityMod = 2.5f;

    public Transform groundCheckPoint;
    private bool _isGrounded;
    public LayerMask groundLayers;

    public GameObject bulletImpact;

    public float timeBetweenShots = .1f;
    private float shotCounter;

    public float maxHeat = 10f, heatPerShot = 1f, coolRate = 4f, overHeatCoolRate = 5f;
    private float heatCounter;
    private bool overHeated;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _camera = Camera.main;
    }

    private void Update()
    {
        PlayerMovement();

        CursorControl();
    }

    private void LateUpdate()
    {
        _camera.transform.position = viewPoint.position;
        _camera.transform.rotation = viewPoint.rotation;
    }

    void Shoot()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = _camera.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Target: " + hit.collider.gameObject.name);

            GameObject bulletImpactObj = Instantiate(bulletImpact, hit.point + (hit.normal * .002f), Quaternion.LookRotation(hit.normal, Vector3.up));
            Destroy(bulletImpactObj, 10f);
        }

        shotCounter = timeBetweenShots;

        heatCounter += heatPerShot;
        if (heatCounter >= maxHeat)
        {
            heatCounter = maxHeat;

            overHeated = true;
        }

    }


    void CursorControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
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

        float yVel = _movement.y;
        _movement = ((transform.forward * _moveDirection.z) + (transform.right * _moveDirection.x)).normalized * _activeMoveSpeed;
        _movement.y = yVel;
      
        if (charCon.isGrounded)
        {
            _movement.y = 0f;
        }

        _isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f, groundLayers);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _movement.y = jumpForce;
        }

        

        _movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

        charCon.Move(_movement * Time.deltaTime);

        if (!overHeated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }

            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    Shoot();
                }
            }
            heatCounter -= coolRate * Time.deltaTime;
        }
        else
        {
            heatCounter -= overHeatCoolRate * Time.deltaTime;

            if(heatCounter <= 0)
            {
                
                overHeated = false;
            }
        }

        if(heatCounter < 0)
        {
            heatCounter = 0f;
        }

        
    }
}
