using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    //Values setted at inspector
    [SerializeField]public float speed;
    [SerializeField]public float rotationSpeed;

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
            float forceMultiplier = Vector3.Dot(desiredDir.normalized, currentDir);
            if (forceMultiplier < 0) forceMultiplier = 0;
            rb.AddForce(currentDir * forceMultiplier * speed * Time.fixedDeltaTime);
            rb.drag = drag;

            GameValues.me.movement = desiredDir;
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            GameValues.me.timestamp = timestamp;
            GameValues.me.method = "game";
            string playerDataJSON = JsonUtility.ToJson(GameValues.me);
            GameValues.socket.Send(playerDataJSON); 
        }
    }
}