using SocketIOClient;
using UnityEngine;
// using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace Local.Networking{
    public class HttpClient{
        SocketIO socket;
        public string room_id;

        private static HttpClient _instance;

        public PlayersInfo playersInfo;

        public static HttpClient instance{
            get {
                if(_instance != null)
                    return _instance;
                return _instance = new HttpClient();
            }
            set {
                _instance = value;
            }
        }

        private HttpClient() {
            socket = new SocketIO("http://localhost:3000/");
        }

        public async void Connect(string room_id, Action OnSuccess){
            if(room_id != null && room_id != "")
                Debug.Log(room_id);

            socket.On("room_entered",res=>{
                // Debug.Log(res.ToString());
                Debug.Log("go to next scene");
                this.room_id = room_id;
                OnSuccess();
            });


            socket.OnConnected += async (sender, e) => {
            await socket.EmitAsync("enter_room",room_id);
            };
            EventSetup();

            //connect to socketio server
            await socket.ConnectAsync();

        }

        public void SubmitName(string name, Action<PlayersInfo> OnSuccess){
            socket.On("Onfetch_canvas_data",response=>{
                Debug.Log("on fetch_canvas data");
                List<Player> others = response.GetValue<List<Player>>();
                Player self = response.GetValue<Player>(1);
                Debug.Log(self.ToString());
                playersInfo = new PlayersInfo(others,self);
                OnSuccess(playersInfo);
                

            });

            socket.EmitAsync("fetch_canvas_data",name);
            

        }


        private async void EventSetup(){
            socket.On("new_player",response=>{
                Debug.Log("new_player: " + response.GetValue<Player>().id);
                if(playersInfo != null)
                    playersInfo.AddPlayer(response.GetValue<Player>());
            });

        }
        
        ~HttpClient() {
            socket.DisconnectAsync();
        }
    }
}