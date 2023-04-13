using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinLobbyUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject panel;
    public GameObject buttonTemplate;
    public List<GameObject> buttons;
    void Start()
    {
        var getLobbies = "{\"method\" : \"getLobbies\"}";
        GameValues.socket.Send(getLobbies);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameValues.listHasChanged){
            GameValues.listHasChanged = false;
            GameObject g;
            foreach(Transform c in panel.transform){
                Destroy(c.gameObject);
            }
            int i = 0;
            foreach(var v in GameValues.lobbies){
                g = Instantiate (buttonTemplate, panel.transform);
                g.SetActive(true);
                g.transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = v.lobbyName;
                g.GetComponent<Button>().onClick.AddListener(()=>ButtonClicked(i++));
            }
        }
    }

    void ButtonClicked(int i){
        GameValues.me.lobbyID = GameValues.lobbies[i].lobbyID;
        var me = GameValues.me;
        me.method = "joinLobby";
        GameValues.socket.Send(JsonUtility.ToJson(me));
        SceneManager.LoadScene("Lobby");
    }

}
