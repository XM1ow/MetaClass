using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ive;

public class UserModelController : NetworkBehaviour
{
    public float walkSpeed;
    public float runSpeed;

    [SerializeField] private PlayerInputActions playerInputActions;
    private Rigidbody rb;
    private Vector3 inputDir;
    private bool isRunning;
    private Animator animator;
    private Transform cameraTransform;
    // Start is called before the first frame update
    private void OnEnable()
    {
        playerInputActions.onMovement += Move;
        playerInputActions.onStopMove += StopMove;
        playerInputActions.onStartRunning += StartRunning;
        playerInputActions.onStopRunning += StopRunning;
    }

    private void OnDisable()
    {
        playerInputActions.onMovement -= Move;
        playerInputActions.onStopMove -= StopMove;
        playerInputActions.onStartRunning -= StartRunning;
        playerInputActions.onStopRunning -= StopRunning;
    }
    private void Move(Vector2 dir)
    {
        if (isLocalPlayer)
        {
            inputDir = dir.ToVector3XZ().normalized;
        }
    }

    private void StopMove()
    {
        if (isLocalPlayer)
        {
            inputDir = Vector3.zero;
        }
    }

    private void StartRunning()
    {
        if (isLocalPlayer)
        {
            isRunning = true;
        }
    }

    private void StopRunning()
    {
        if (isLocalPlayer)
        {
            isRunning = false;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        playerInputActions.EnableGameplayInput();
    }

    // Update is called once per frame
    private void Update()
    {
        //GetPlayerInput();
        if (isLocalPlayer)
        {
            SetAnimator();
            MoveModel();
        }
    }

    /*private void GetPlayerInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        inputDir.x = x;inputDir.z = z;
    }*/

    private void SetAnimator()
    {
        animator.SetBool("IsWalking", inputDir.magnitude > float.Epsilon);
        animator.SetBool("IsRunning", isRunning);
    }

    private void MoveModel()
    {
        if (inputDir.magnitude < float.Epsilon) return;
        Vector3 cameraForward = cameraTransform.forward - Vector3.Dot(cameraTransform.forward, Vector3.up) * Vector3.up;
        float angleTmp = Vector3.Angle(Vector3.forward, inputDir);
        if (inputDir.x < 0) angleTmp = -angleTmp;
        transform.forward = Quaternion.AngleAxis(angleTmp, Vector3.up) * cameraForward;
        rb.velocity = transform.forward * (isRunning ? runSpeed : walkSpeed);
        //transform.position += transform.forward * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime;
    }
}