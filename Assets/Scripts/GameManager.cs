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
    public FrogController frogController;

    public NetworkVariable<bool> onePlayerHasFrog = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> redTeamPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> blueTeamPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> lastPlayerTeam = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public MultipleTargetsCamera multipleTargetsCamera;
    public UIManager uIManager;


    private readonly ulong[] targetClientsArray = new ulong[1];

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        blueTeamPoints.OnValueChanged += delegate { OnPointsChanged(); };
        redTeamPoints.OnValueChanged += delegate { OnPointsChanged(); };
        onePlayerHasFrog.OnValueChanged += delegate { OnChangeOnePlayerHasFrog(); };
    }

    private void Awake() {
        if (instance)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    [ServerRpc(RequireOwnership = false)]
    public void AddPointServerRpc(int team) {
        if (team == (int)Team.RED) {
            redTeamPoints.Value++;
        }
        else {
            blueTeamPoints.Value++;
        }
    }
    public void OnPointsChanged() {
        uIManager.SetRedTeamPoints(redTeamPoints.Value);
        uIManager.SetBlueTeamPoints(blueTeamPoints.Value);
    }
    public void AddPoint(Team team){
        AddPointServerRpc((int)team);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeOnePlayerHasFrogServerRpc(bool hasFrog){
        onePlayerHasFrog.Value = hasFrog;
    }
    public void OnChangeOnePlayerHasFrog(){
        if(!onePlayerHasFrog.Value) {
            frogController.gameObject.SetActive(true);
            frogController.transform.SetPositionAndRotation(new Vector3(0, 16, 0), Quaternion.identity);
        }
        else {
            frogController.gameObject.SetActive(false);
        }
    }

    public void AddPlayer(NetworkBehaviour player){
        Team team = (Team)lastPlayerTeam.Value;
        if(team == Team.RED)
            uIManager.SetCurrentTeamText("Red");
        else
            uIManager.SetCurrentTeamText("Blue");
        //multipleTargetsCamera.targets.Add(player.transform);
    }

}
