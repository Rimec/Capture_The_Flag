using UnityEngine;
using Unity.Netcode;
public class PlayerController : NetworkBehaviour {
    public NetworkVariable<bool> hasFlag = new NetworkVariable<bool>();
    public NetworkVariable<Team> team = new NetworkVariable<Team>(Team.NONE);
    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        GameManager.instance.AddPlayer(this);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Frog")) {
            hasFlag.Value = true;
        }else if (other.CompareTag("Player")) {
            if (other.TryGetComponent<PlayerController>(out PlayerController playerController)) {
                if (playerController.hasFlag.Value) {
                    playerController.hasFlag.Value = false;
                }
            }
        }
    }
}