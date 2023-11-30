using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]public float speed;
    [SerializeField]public float rotationSpeed;

    private int repeat = 0;

    public float drag;
 
    Rigidbody rb;
 
    void Awake() => rb = GetComponent<Rigidbody>();
 
    private void FixedUpdate()
    {
        Vector3 desiredDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 currentDir = transform.rotation * Vector3.forward;
            transform.LookAt(Vector3.Lerp(currentDir, desiredDir, rotationSpeed * Time.fixedDeltaTime) + transform.position);
            float forceMultiplier = Vector3.Dot(desiredDir.normalized, currentDir);
            if (forceMultiplier < 0) forceMultiplier = 0;
            rb.AddForce(currentDir * forceMultiplier * speed * Time.fixedDeltaTime);
            rb.drag = drag;
            GameValues.me.position = transform.position;
            GameValues.me.rotation = transform.rotation;
            GameValues.me.velocity = rb.velocity;
            if(repeat++ % 10 == 0){
                double timestamp = Clock.getTime();
                GameValues.me.timestamp = timestamp;
                GameValues.me.method = "game";
                GameValues.me.newData = true;
                GameValues.me.SendToServer();
            }
            if(repeat % 100 == 0){
                Clock.startSync();
            }
    }
}