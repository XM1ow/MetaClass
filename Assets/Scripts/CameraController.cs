using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraCenterOffset;
    public float thirdCameraRadius;
    public float firstCameraRadius;
    public float initVerticalAngle;
    public float cameraSpeed;
    public float verticalAngleLimit;

    private bool isFirstPerson;
    private Transform follow;
    private Vector3 cameraCenter;
    private Vector3 cameraRadiusVector;
    private float verticalAngle;
    private float horizontalAngle;
    // Start is called before the first frame update

    public void CameraControllerInit(Transform trans)
    {
        follow = trans;
        Cursor.visible = false;
        cameraCenter = follow.position + cameraCenterOffset;
        cameraRadiusVector = -Vector3.forward * thirdCameraRadius;
        if (initVerticalAngle > verticalAngleLimit) initVerticalAngle = verticalAngleLimit;
        if (initVerticalAngle < -verticalAngleLimit) initVerticalAngle = -verticalAngleLimit;
        verticalAngle = initVerticalAngle;
        horizontalAngle = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (follow != null)
        {
            CalculateCameraPosition();
            GetMouseInput();
        }
        if (Input.GetKeyDown(KeyCode.V)) isFirstPerson = !isFirstPerson;
    }

    private void CalculateCameraPosition()
    {
        cameraCenter = follow.position + cameraCenterOffset;
        Vector3 nowPosition = cameraRadiusVector;
        nowPosition = Quaternion.AngleAxis(verticalAngle, Vector3.right) * nowPosition;
        nowPosition = Quaternion.AngleAxis(horizontalAngle, Vector3.up) * nowPosition;
        if (isFirstPerson) nowPosition = -nowPosition * firstCameraRadius / thirdCameraRadius;
        transform.position = cameraCenter + nowPosition;
        transform.forward = isFirstPerson ? nowPosition : -nowPosition;
    }

    private void GetMouseInput()
    {
        float verticalMouse = Input.GetAxis("Mouse Y");
        float horizontalMouse = Input.GetAxis("Mouse X");
        verticalAngle -= verticalMouse * Time.deltaTime * cameraSpeed;
        horizontalAngle += horizontalMouse * Time.deltaTime * cameraSpeed;
        if (verticalAngle > verticalAngleLimit) verticalAngle = verticalAngleLimit;
        if (verticalAngle < -verticalAngleLimit) verticalAngle = -verticalAngleLimit;
        if (horizontalAngle > 360) horizontalAngle -= 360;
        if (horizontalAngle < -360) horizontalAngle += 360;
    }
}