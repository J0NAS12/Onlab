 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class move : MonoBehaviour
 {
 
     public float moveSpeed = 20f;
     public float turnSpeed = 20f;
 
 
     Rigidbody m_Rigidbody;
     Vector3 m_Movement;
     Quaternion m_Rotation = Quaternion.identity;
 
 
     void Start()
     {
         m_Rigidbody = GetComponent<Rigidbody>();
     }
 
 
     void FixedUpdate()
     {
         float horizontal = Input.GetAxis("Horizontal");
         float vertical = Input.GetAxis("Vertical");
 
         m_Movement.Set(horizontal, 0f, vertical);
         m_Movement.Normalize();
 
         bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); // Sets the bool to true if 'horizontal' input is approx 0
         bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
         bool isWalking = hasHorizontalInput || hasVerticalInput;
         
 
         Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
         m_Rotation = Quaternion.LookRotation(desiredForward);
 
         m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * moveSpeed * Time.deltaTime);
         m_Rigidbody.MoveRotation(m_Rotation);
     }
 }
