using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage;

    void OnCollisionEnter(Collision other) {
        Target target = other.gameObject.GetComponent<Target>();
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
        Debug.Log(""+ other.gameObject.name + "    " + this.name);
         if(target != null && other.gameObject.name != this.name) {
            Destroy(gameObject);
            target.Hit(damage, int.Parse(this.name));
         }
    }
}