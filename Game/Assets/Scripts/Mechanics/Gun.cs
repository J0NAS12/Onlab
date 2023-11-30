using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject round;

    [Range(0.5f, 100)] public float roundSpeed;

    /// Attempts to fire the gun
    public void Shoot() {
        GameObject spawnedRound = Instantiate(
            round,
            transform.position,
            transform.rotation
        );
        spawnedRound.name = GameValues.me.index.ToString();
        double timestamp = Clock.getTime();
        var bulletData = new BulletData{
            method = "bullet",
            shooter = GameValues.me,
            startPoint = transform.position,
            rotation = transform.rotation,
            speed = roundSpeed,
            timestamp = timestamp
        };
        bulletData.SendToServer();
        Rigidbody rb = spawnedRound.GetComponent<Rigidbody>();
        rb.velocity = spawnedRound.transform.forward * roundSpeed;
    }
}