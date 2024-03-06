using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour {
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        GameManager.instance.AddPlayer(this.gameObject);
    }
}