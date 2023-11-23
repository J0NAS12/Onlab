using TMPro;
using UnityEngine;

public class Target : MonoBehaviour {
    public bool destroyable = true;
    public float health;

    private int lasthit;



    void Update() {
        if(health <= 0) {
            var hitData = new HitData(){
                method = "hit",
                player = GameValues.me,
                roomID = GameValues.me.roomID,
                shooter = lasthit
            };
            Destroy(gameObject);
            GameValues.socket.Send(JsonUtility.ToJson(hitData));
        }
    }

    /// 'Hits' the target for a certain amount of damage
    public void Hit(float damage, int tag) {
        if(destroyable){
            lasthit = tag;
            health -= damage;
        }
    }
}