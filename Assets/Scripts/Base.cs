using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Base : NetworkBehaviour
{
    public Team baseType;
    bool hasCompleted = true;

    private void OnTriggerEnter(Collider other) {
        //Debug.Log(OwnerClientId);
        //GameManager.instance.uIManager.SetCurrentTeamText($"{}");
        if (!IsOwner) return;
        if (!hasCompleted) return;
        if (!other.CompareTag("Player")) return;
        if (!other.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
        if (!player.hasFrog.Value) return;
        if (player.team.Value != baseType) return;
        
        hasCompleted = false;
        player.LooseFrogServerRpc(player.OwnerClientId);
        StartCoroutine(ResetHasCompleted(player));
    }

    IEnumerator ResetHasCompleted(PlayerController playerController){
        yield return new WaitUntil(() => GameManager.instance.frogController.gameObject.activeSelf);
        GameManager.instance.AddPoint(baseType);
        hasCompleted = true;
    }
}
