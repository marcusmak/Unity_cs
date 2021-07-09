using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Local.Networking;

public class PlayersHandler : MonoBehaviour
{
    // Start is called before the first frame update
    // PlayersInfo playersInfo;
    // OtherPlayerController[] OtherPlayerControllers
    Object playerPrefab;
    Object mainPlayerPrefab;
    
    void Start()
    {
       InitPlayers(HttpClient.instance.playersInfo);
       Debug.Log("PlayersHandler");
    }
    void InitPlayers(PlayersInfo playersInfo){
        Transform parent = GameObject.Find("Players").transform;
        mainPlayerPrefab = Resources.Load("Prefabs/MainCharacter");
        playerPrefab= Resources.Load("Prefabs/Character");
        Debug.Log("InitPLayers " + playersInfo.Count);
        foreach(Player player in playersInfo.players){
            Debug.Log(player.name);
            GameObject temp = Instantiate(playerPrefab) as GameObject;
            temp.transform.SetParent(parent);
        }
        GameObject temp2 = Instantiate(mainPlayerPrefab) as GameObject;
        temp2.transform.SetParent(parent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
