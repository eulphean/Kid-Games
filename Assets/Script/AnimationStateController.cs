using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator; 
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {

        bool isRunning = animator.GetBool("isRunning"); 
        bool isWalking = animator.GetBool("isWalking"); 
        bool fwdPress = Input.GetKey("w"); 
        bool runPress = Input.GetKey("left shift");

        if (fwdPress && !isWalking) {
            animator.SetBool("isWalking", true);
        }

        if (isWalking && !fwdPress) {
            animator.SetBool("isWalking", false); 
        }

         if (!isRunning && (fwdPress && runPress)) {
            animator.SetBool("isRunning", true);
        }

        if (isRunning && (!fwdPress || !runPress)) {
            animator.SetBool("isRunning", false); 
        }
    }
}
