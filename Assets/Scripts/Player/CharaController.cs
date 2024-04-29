using System;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ive;
using UnityEngine.Windows;
using System.Security.Policy;

public class CharaController : NetworkBehaviour
{
    public float walkSpeed;
    public float runSpeed;

    private float nowSpeed;
    private PlayerInputActions _playerInputActions;
    private Vector3 inputDir;
    private Animator animator;
    private Transform cameraTransform;
    private Rigidbody rb;
    private Vector3 velocity;
    private Transform chairTop;
    private CapsuleCollider capCollider;
    private BoxCollider boxCollider;
    private bool walk;
    private bool run;
    private bool sit;

    private void Awake()
    {
        if(!_playerInputActions)
            _playerInputActions = ScriptableObject.CreateInstance<PlayerInputActions>();
        _playerInputActions.EnableGameplayInput();
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();
        SetCamera();
    }

    private void OnEnable()
    {
        _playerInputActions.onMovement += Move;
        _playerInputActions.onStopMove += StopMove;
        _playerInputActions.onStartRun += Run;
        _playerInputActions.onStopRun += StopRun;
        _playerInputActions.onStartSit += Sit;
    }

    private void OnDisable()
    {
        _playerInputActions.onMovement -= Move;
        _playerInputActions.onStopMove -= StopMove;
        _playerInputActions.onStartRun -= Run;
        _playerInputActions.onStopRun -= StopRun;
        _playerInputActions.onStartSit -= Sit;
    }
    private void Move(Vector2 dir)
    {
        if (isLocalPlayer) inputDir = dir.ToVector3XZ().normalized;
    }

    private void StopMove()
    {
        if (isLocalPlayer) inputDir = Vector3.zero;
    }
    private void Run()
    {
        if(isLocalPlayer) run = true;
    }

    private void StopRun()
    {
       if(isLocalPlayer) run = false;
    }

    private void Sit()
    {
        if (isLocalPlayer)
        {
            if (sit)
            {
                sit = false;
                capCollider.enabled = true;
                boxCollider.enabled = false;
            }
            else
            {
                if (chairTop == null) return;
                sit = true;
                transform.position = chairTop.position;
                transform.forward = chairTop.forward;
                capCollider.enabled = false;
                boxCollider.enabled = true;
            }
        }
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
        if (!sit) walk = inputDir.magnitude > float.Epsilon;
        else walk = false;
        animator.SetBool("Walk", walk);
        animator.SetBool("Run", run);
        animator.SetBool("Sit", sit);
    }

    private void MoveModel()
    {
        if (inputDir.magnitude < float.Epsilon || sit)
        {
            velocity = Vector3.zero;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
            return;
        }
        nowSpeed = run ? runSpeed : walkSpeed;
        Vector3 cameraForward = cameraTransform.forward - Vector3.Dot(cameraTransform.forward, Vector3.up) * Vector3.up;
        float angleTmp = Vector3.Angle(Vector3.forward, inputDir);
        if (inputDir.x < 0) angleTmp = -angleTmp;
        transform.forward = Quaternion.AngleAxis(angleTmp, Vector3.up) * cameraForward;
        velocity = transform.forward * nowSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    public void SetCamera()
    {
        Camera.main.GetComponent<CameraController>().CameraControllerInit(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Chair")
        {
            chairTop = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Chair") chairTop = null;
    }
}