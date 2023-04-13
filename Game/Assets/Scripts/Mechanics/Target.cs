using TMPro;
using UnityEngine;

public class Target : MonoBehaviour {
    public bool destroyable = true;
    public float health;



    void Update() {
        if(health <= 0) {
            Destroy(gameObject);
            GameValues.spidersLeft--;

        }
    }

    /// 'Hits' the target for a certain amount of damage
    public void Hit(float damage) {
        if(destroyable){
            health -= damage;
        }
    }
}