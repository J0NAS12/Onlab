using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentMovement : MonoBehaviour
{
    //Values setted at inspector
    [SerializeField]public float speed;
    [SerializeField]public float rotationSpeed;

    public float drag;

    public int index;
 
    Rigidbody rb;
 
    void Awake() => rb = GetComponent<Rigidbody>();
    void FixedUpdate() {
            var playerData = GameValues.lobbyPlayers[index];
            var desiredDir = playerData.movement;
            if (desiredDir != Vector3.zero)
            {
                Vector3 currentDir = transform.rotation * Vector3.forward;
                transform.LookAt(Vector3.Lerp(currentDir, desiredDir, rotationSpeed * Time.fixedDeltaTime) + transform.position);
                float forceMultiplier = Vector3.Dot(desiredDir.normalized, currentDir);
                //since we never want to apply a negative force if the character is facing opposite to the direction we will move it
                //we make sure forceMultipler is not negative.
                if (forceMultiplier < 0) forceMultiplier = 0;
                //we add the force in the direction character is facing
                rb.AddForce(currentDir * forceMultiplier * speed * Time.fixedDeltaTime);
                rb.drag = drag;
            }
        }
}
