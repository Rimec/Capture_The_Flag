using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public enum Team{
    NONE = 0,
    RED = 1,
    BLUE = 2
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public NetworkVariable<int> redTeamPoints = new NetworkVariable<int>();
    public NetworkVariable<int> blueTeamPoints = new NetworkVariable<int>();

    public MultipleTargetsCamera multipleTargetsCamera;
    public UIManager uIManager;

    private void Awake() {
        if(instance)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddPoint(Team team){
        if(team == Team.RED)
            redTeamPoints.Value++;
        else
            blueTeamPoints.Value++;
    }

    public NetworkVariable<int> lastPlayerTeam = new NetworkVariable<int>(0);
    public void AddPlayer(NetworkBehaviour player){
        lastPlayerTeam.Value = lastPlayerTeam.Value + 1 % 2;
        Team team = (Team)lastPlayerTeam.Value;
        PlayerController playerController = player as PlayerController;
        if(playerController.team.Value == Team.NONE){
            playerController.team.Value = team;
            if(team == Team.RED)
                uIManager.SetCurrentTeamText("Red");
            else
                uIManager.SetCurrentTeamText("Blue");
        }
        multipleTargetsCamera.targets.Add(player.transform);
    }
}
