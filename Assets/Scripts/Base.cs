using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    public Team baseType;
    bool has_completed = false;

    private void OnTriggerEnter(Collider other) {
        if (has_completed) return;
        if (!other.CompareTag("Player")) return;
        if (!other.attachedRigidbody.TryGetComponent(out PlayerController player)) return;
        if (!player.hasFrog.Value) return;
        if (player.team.Value != baseType) return;
        
        player.LooseFrogServerRpc(player.OwnerClientId);
        GameManager.instance.AddPoint(baseType);
        has_completed = true;
        StartCoroutine(ResetHasCompleted(player));
    }

    IEnumerator ResetHasCompleted(PlayerController playerController){
        yield return new WaitUntil(() => !playerController.hasFrog.Value);
        has_completed = false;
    }
}
