using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;
    public Vector3 cameraCenterOffset;
    public float cameraRadius;
    public float initVerticalAngle;
    public float cameraSpeed;
    public float verticalAngleLimit;

    private Vector3 cameraCenter;
    private Vector3 cameraRadiusVector;
    private float verticalAngle;
    private float horizontalAngle;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.visible = false;
        cameraCenter = follow.position + cameraCenterOffset;
        cameraRadiusVector = -Vector3.forward * cameraRadius;
        if (initVerticalAngle > verticalAngleLimit) initVerticalAngle = verticalAngleLimit;
        if (initVerticalAngle < -verticalAngleLimit) initVerticalAngle = -verticalAngleLimit;
        verticalAngle = initVerticalAngle;
        horizontalAngle = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateCameraPosition();
        GetMouseInput();
    }

    private void CalculateCameraPosition()
    {
        cameraCenter = follow.position + cameraCenterOffset;
        Vector3 nowPosition = cameraRadiusVector;
        nowPosition = Quaternion.AngleAxis(verticalAngle, Vector3.right) * nowPosition;
        nowPosition = Quaternion.AngleAxis(horizontalAngle, Vector3.up) * nowPosition;
        transform.position = cameraCenter + nowPosition;
        transform.forward = -nowPosition;
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
