using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage;

    void OnCollisionEnter(Collision other) {
        Target target = other.gameObject.GetComponent<Target>();
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
         if(target != null) {
             target.Hit(damage);
             Debug.Log(target.gameObject.name);
             Destroy(gameObject);
         }
    }
}