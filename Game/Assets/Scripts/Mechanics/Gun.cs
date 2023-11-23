using UnityEngine;

public class Gun : MonoBehaviour {
    public enum ShootState {
        Ready,
        Shooting,
        Reloading
    }

    // How far forward the muzzle is from the centre of the gun
    private float muzzleOffset;

    [Header("Magazine")]
    public GameObject round;

    [Range(0.5f, 100)] public float roundSpeed;


    private ShootState shootState = ShootState.Ready;

    // The next time that the gun is able to shoot at
    private float nextShootTime = 0;

    void Start() {
        //muzzleOffset = GetComponent<Renderer>().bounds.extents.z;
        muzzleOffset = 0;
    }

    void Update() {
        switch(shootState) {
            case ShootState.Shooting:
                // If the gun is ready to shoot again...
                if(Time.time > nextShootTime) {
                    shootState = ShootState.Ready;
                }
                break;
            case ShootState.Reloading:
                // If the gun has finished reloading...
                if(Time.time > nextShootTime) {
                    shootState = ShootState.Ready;
                }
                break;
        }
    }

    /// Attempts to fire the gun
    public void Shoot() {
        // Checks that the gun is ready to shoot
        if(shootState == ShootState.Ready) {
            GameObject spawnedRound = Instantiate(
                round,
                transform.position + transform.forward * muzzleOffset,
                transform.rotation
            );
            spawnedRound.name = GameValues.me.index.ToString();
            double timestamp = Clock.getTime();
            var bulletData = new BulletData{
                method = "bullet",
                shooter = GameValues.me,
                startPoint = transform.position + transform.forward * muzzleOffset,
                rotation = transform.rotation,
                speed = roundSpeed,
                timestamp = timestamp
            };
            GameValues.socket.Send(JsonUtility.ToJson(bulletData));
            Rigidbody rb = spawnedRound.GetComponent<Rigidbody>();
            rb.velocity = spawnedRound.transform.forward * roundSpeed;
        }
    }
}