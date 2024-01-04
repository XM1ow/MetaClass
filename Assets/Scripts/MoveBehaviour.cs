using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Ive;

public class MoveBehaviour : NetworkBehaviour
{
    [SerializeField] private PlayerInputActions playerInputActions;
    private Vector2 _direction;
    private Rigidbody _rigidbody;
    private float _speed;

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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _speed = 5f;
        playerInputActions.EnableGameplayInput();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            _rigidbody.velocity = _direction.ToVector3XZ() * _speed;
        }
    }

    private void Move(Vector2 dir)
    {
        if (isLocalPlayer)
        {
            _direction = dir.normalized;
        }
    }

    private void StopMove()
    {
        if (isLocalPlayer)
        {
            _direction = Vector3.zero;
        }
    }
    
    [Command]
    void LetServerSayHelloToMe()
    {
        Debug.Log("Server: Hello");
        LetTargetSayHelloBack();
    }

    [TargetRpc]
    void LetTargetSayHelloBack()
    {
        Debug.Log("Client: hello");
    }
}
