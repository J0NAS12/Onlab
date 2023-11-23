using UnityEngine;

public class CharacterShooting : MonoBehaviour {
    public Gun gun;
    public KeyCode shootKey;

    void Update() {
        if(Input.GetKeyDown(shootKey)) {
            gun.Shoot();
        }
    }
}