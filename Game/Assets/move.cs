/* using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    private Vector3 targetVelocity;
    // Start is called before the first frame update
    void Start()
    {
        targetVelocity = new Vector3(0,0,0);

    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        float length = (float)Math.Sqrt(Math.Pow(Horizontal, 2) + Math.Pow(Vertical, 2));
        if(length!=0){
            Horizontal/=length;
            Vertical/=length;
        }
        Debug.Log(targetVelocity.x + "+" +  targetVelocity.z);
        targetVelocity = 0.5f * targetVelocity + new Vector3(Horizontal*0.5f, 0, Vertical*0.5f);
        m_Rigidbody.velocity = targetVelocity;
                    Quaternion rot = gameObject.transform.rotation;
/*        if(Math.Abs(targetVelocity.x*targetVelocity.z)>0.01f){
             rot.y = (float)Math.Asin(targetVelocity.z/(float)Math.Sqrt(Math.Pow(targetVelocity.x,  2) + Math.Pow(targetVelocity.z, 2)));
            if(targetVelocity.x<0){
                rot.y += 3.14f;  
        if(Math.Abs(targetVelocity.x)>0.01f){ 
            if(targetVelocity.x>0){
                rot.y=1.0f;
            }else {
                rot.y=(float)(-3.14/2);
            }
        }
        else if(Math.Abs(targetVelocity.z)>0.01f){
            if(targetVelocity.z>0){
                rot.y=0;
            }else {
                rot.y=3.14f;
            }
        }
        gameObject.transform.rotation = rot;
    }
} */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = true;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}

