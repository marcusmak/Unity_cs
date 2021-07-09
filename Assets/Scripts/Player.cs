using System.Collections.Generic;
using UnityEngine;
public class Player{
    public string id { get; set; }
    public string name { get; set; }
    public int[] position { get; set; }
    public int[] rotation { get; set; }
}

public class PlayersInfo{
    public HashSet<Player> players;
    public Player self;
    
    public int Count {
        get => players.Count;
    }
    public PlayersInfo(Player self){
        players = new HashSet<Player>();
        this.self = self;
    }

    public PlayersInfo(List<Player> players, Player self){
        this.players = new HashSet<Player>(players);
        this.self = self;
    } 

    public void AddPlayer(Player player){
        players.Add(player);
    }

    
    public void AddPlayer(List<Player> players){
        players.AddRange(players);
    }

    public void Interact(Player MainPlayer, Player OtherPLayer){
        Debug.Log(MainPlayer.name +"is interacting with " + OtherPLayer.name);
    }
}