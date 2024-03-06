using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    public List<int> playerPoints = new List<int>();
    public List<GameObject> players = new List<GameObject>();
    public static GameManager instance;

    public MultipleTargetsCamera multipleTargetsCamera;

    Action onConnectionEvent;
    private void Awake() {
        if(instance)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);


    }

    public void AddPoint(int id){
        if (playerPoints.Count == 0){
            playerPoints.Add(1);
        }
        else if(id >= playerPoints.Count - 1){
            playerPoints.Add(1);
        }
        else{
            playerPoints[id]++;   
        }
    }

    public void AddPlayer(GameObject player){
        players.Add(this.gameObject);
        multipleTargetsCamera.targets.Add(player.transform);
    }
}
