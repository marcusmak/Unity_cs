using SocketIOClient;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HttpClient: MonoBehaviour{
    SocketIO socket;
    GameObject roomUI, signinUI, loadingUI;

    private void Start() {
        socket = new SocketIO("http://localhost:3000/");
        // inputField = GameObject.FindObjectOfType<InputField>();
        roomUI = GameObject.Find("Room");
        roomUI.SetActive(true);
        signinUI = GameObject.Find("Sign-in");
        signinUI.SetActive(false);
        loadingUI = GameObject.Find("loadingUI");
        loadingUI.SetActive(false);
    }

    public async void Connect(InputField input){
        var room_id = input.text;
        if(room_id != null && room_id != "")
            Debug.Log(room_id);

        socket.On("room_entered",res=>{
            Debug.Log(res.ToString());
            Debug.Log("go to next scene");

        });


        socket.OnConnected += async (sender, e) => {
           await socket.EmitAsync("enter_room",room_id);
        };
        EventSetup();
        //Change to loading UI
        loadingUI.SetActive(true);
        roomUI.SetActive(false);

        //connect to socketio server
        await socket.ConnectAsync();
        
        //change to signinUI
        loadingUI.SetActive(false);
        signinUI.SetActive(true);
    }

    public void SubmitName(InputField input){
        socket.EmitAsync("fetch_canvas_data",input.text);
        
        socket.On("Onfetch_canvas_data",response=>{
            List<Player> others = response.GetValue<List<Player>>();
            Debug.Log("other" + others[0].id);

            //Load Game Canvas and spawn players
            // Scene.Load()


        });
    }


    private async void EventSetup(){
        socket.On("new_player",response=>{
            Debug.Log("new_player: " + response.GetValue<Player>().id);
        });
        
    }

     

    

    private void OnDestroy() {
        socket.DisconnectAsync();
    }
}
