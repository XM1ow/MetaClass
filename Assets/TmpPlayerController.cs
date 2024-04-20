using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TmpPlayerController : MonoBehaviour
{
    public CameraController cc;
    private Animator animator;
    private bool speech;
    private bool sitting;
    private bool run;
    private bool walk;
    // Start is called before the first frame update
    void Start()
    {
        cc.CameraControllerInit(transform);   
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.X)) speech = true;
        else speech = false;
        if (!walk && Input.GetKeyDown(KeyCode.Space)) sitting = !sitting;
        if (Input.GetKey(KeyCode.LeftShift)) run = true;
        else run = false;
        if (!sitting && Input.GetKey(KeyCode.W)) walk = true;
        else walk = false;
        animator.SetBool("Speech", speech);
        animator.SetBool("Sitting", sitting);
        animator.SetBool("Run", run);
        animator.SetBool("Walk", walk);
        if (Input.GetKeyDown(KeyCode.O)) SceneManager.LoadScene(0);
    }
}
