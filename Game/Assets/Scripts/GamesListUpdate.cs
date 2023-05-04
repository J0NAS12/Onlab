using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesListUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject panel;
    public GameObject buttonTemplate;
    public List<GameObject> buttons;
    void Start()
    {
        var getLobbies = "{\"method\" : \"getGames\"}";
        GameValues.socket.Send(getLobbies);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameValues.listHasChanged){
            GameValues.listHasChanged = false;
            foreach(Transform c in panel.transform){
                Destroy(c.gameObject);
            }
            for (var i = 0; i<GameValues.lobbies.Count; i++){
                var v = GameValues.lobbies[i];
                GameObject g = Instantiate (buttonTemplate, panel.transform);
                g.SetActive(true);
                g.transform.GetChild (0).GetComponent <TextMeshProUGUI> ().text = v.lobbyName;
                g.name = i.ToString();
                g.GetComponent<Button>().onClick.AddListener(()=>ButtonClicked(int.Parse(g.name)));
            }
        }
    }

    void ButtonClicked(int i){
        GameValues.me.lobbyID = GameValues.lobbies[i].lobbyID;
        GameValues.me.lobbyName = GameValues.lobbies[i].lobbyName;
        var me = GameValues.me;
        me.method = "joinGame";
        GameValues.socket.Send(JsonUtility.ToJson(me));
        SceneManager.LoadScene("Lobby");
    }

}
