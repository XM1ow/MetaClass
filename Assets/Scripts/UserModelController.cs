using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserModelController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;

    private Vector3 inputDir;
    private bool isRunning;
    private Animator animator;
    private Transform cameraTransform;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        GetPlayerInput();
        SetAnimator();
        MoveModel();
    }

    private void GetPlayerInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        inputDir.x = x;inputDir.z = z;
    }

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
        transform.position += transform.forward * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime;
        //transform.forward = inputDir;
        //transform.position += inputDir * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime;
    }
}
