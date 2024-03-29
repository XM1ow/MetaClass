using System;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ive;
using UnityEngine.Windows;

public class CharaController : NetworkBehaviour
{
    public float speed;

    private PlayerInputActions _playerInputActions;
    private Rigidbody rb;
    private Vector3 inputDir;
    private Animator animator;
    private Transform cameraTransform;
    private Vector3 velocity;

    private void Awake()
    {
        if(!_playerInputActions)
            _playerInputActions = ScriptableObject.CreateInstance<PlayerInputActions>();
        _playerInputActions.EnableGameplayInput();
    }

    private void OnEnable()
    {
        _playerInputActions.onMovement += Move;
        _playerInputActions.onStopMove += StopMove;
    }

    private void OnDisable()
    {
        _playerInputActions.onMovement -= Move;
        _playerInputActions.onStopMove -= StopMove;
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

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(1).GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        SetCamera();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isLocalPlayer)
        {
            SetAnimator();
            MoveModel();
            transform.position += velocity * Time.deltaTime;
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

    public void SetCamera()
    {
        Camera.main.GetComponent<CameraController>().CameraControllerInit(transform);
    }
}