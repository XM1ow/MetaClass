using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;

public class MoveBehaviour : NetworkBehaviour
{
    [SerializeField] private PlayerInputActions playerInputActions;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float speed;

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
        speed = 5f;
        playerInputActions.EnableGameplayInput();
    }

    private void FixedUpdate()
    {
        if (_rigidbody && isLocalPlayer)
        {
            _rigidbody.velocity = _direction * speed;
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
}
