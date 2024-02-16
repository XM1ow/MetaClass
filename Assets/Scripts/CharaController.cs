using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ive;

public class CharaController : NetworkBehaviour
{
    public float speed;

    [SerializeField] private PlayerInputActions playerInputActions;
    private Rigidbody rb;
    private Vector3 inputDir;
    private Animator animator;
    private Transform cameraTransform;
    private Vector3 velocity;
    // Start is called before the first frame update
    private void OnEnable()
    {
        playerInputActions.onMovement += Move;
        playerInputActions.onStopMove += StopMove;
    }

    private void OnDisable()
    {
        playerInputActions.onMovement -= Move;
        playerInputActions.onStopMove -= StopMove;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(1).GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        playerInputActions.EnableGameplayInput();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isLocalPlayer)
        {
            SetAnimator();
            MoveModel();
        }
    }

    private void SetAnimator()
    {
        animator.SetBool("IsWalking", inputDir.magnitude > float.Epsilon);
    }

    private void MoveModel()
    {
        if (inputDir.magnitude < float.Epsilon)
        {
            velocity = Vector3.zero;
            return;
        }
        Vector3 cameraForward = cameraTransform.forward - Vector3.Dot(cameraTransform.forward, Vector3.up) * Vector3.up;
        float angleTmp = Vector3.Angle(Vector3.forward, inputDir);
        if (inputDir.x < 0) angleTmp = -angleTmp;
        transform.forward = Quaternion.AngleAxis(angleTmp, Vector3.up) * cameraForward;
        velocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }
}