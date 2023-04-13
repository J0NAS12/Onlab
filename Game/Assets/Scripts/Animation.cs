using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator mAnimator;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mAnimator!=null){
            if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0){
                mAnimator.SetTrigger("Walk");
            }
            else{
                mAnimator.SetTrigger("Stop");
            }
        }
    }
}
