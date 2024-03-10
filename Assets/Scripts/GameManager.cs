using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public enum Team{
    NONE = 0,
    RED = 1,
    BLUE = 2
}

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    public NetworkVariable<int> redTeamPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> blueTeamPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> lastPlayerTeam = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public MultipleTargetsCamera multipleTargetsCamera;
    public UIManager uIManager;

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
    }

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

    public void AddPlayer(NetworkBehaviour player){
        Team team = (Team)lastPlayerTeam.Value;
        if(team == Team.RED)
            uIManager.SetCurrentTeamText("Red");
        else
            uIManager.SetCurrentTeamText("Blue");
        multipleTargetsCamera.targets.Add(player.transform);
    }

}
