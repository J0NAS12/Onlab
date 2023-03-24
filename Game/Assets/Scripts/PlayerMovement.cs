using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //Values setted at inspector
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;

    public float drag;
 
    Rigidbody rb;
 
    void Awake() => rb = GetComponent<Rigidbody>();
 
    private void FixedUpdate()
    {
        Vector3 desiredDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
 
        if (desiredDir != Vector3.zero)
        {
            Vector3 currentDir = transform.rotation * Vector3.forward;
            transform.LookAt(Vector3.Lerp(currentDir, desiredDir, rotationSpeed * Time.fixedDeltaTime) + transform.position);
 
            //If desiredDir = currentDir just we get 1 (the max) if the angle between them is 90º we get 0, so player can't move till he rotates.
            //If were facing the opposite direction we desire to go, we get -1
            //remember to normalize desiredDir since if you're in a keyBoard pressing left and up at same time, desiredDir = (1,0,1)
            //and we want always a magnitude 1 vector in order to not get a forceMultiplier > 1
            //the max multiplier we want to get is 1 (100)
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