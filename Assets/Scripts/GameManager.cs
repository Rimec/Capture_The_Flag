using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public List<int> playerPoints = new List<int>();
    public static GameManager instance;
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
}
