using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{

    public GameObject assistantCanvas;
    public GameObject mainCamera;
    public GameObject chatCamera;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !assistantCanvas.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                assistantCanvas.SetActive(true);
                mainCamera.SetActive(false);
                chatCamera.SetActive(true);
            }
        }
    }
}
