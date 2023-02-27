using UnityEngine;

public class CharacterShooting : MonoBehaviour {
    public Gun gun;

    public KeyCode shootKey;
    public KeyCode reloadKey;

    void Update() {
        if(Input.GetKeyDown(shootKey)) {
            gun.Shoot();
        }

        if(Input.GetKeyDown(reloadKey)) {
            gun.Reload();
        }
    }
}