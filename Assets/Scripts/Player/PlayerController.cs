using UnityEngine;
using Unity.Netcode;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour {

    private Rigidbody rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 300f;

    public NetworkVariable<bool> hasFlag = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<Team> team = new NetworkVariable<Team>(Team.NONE,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public bool hasInitialized = false;
    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        if(IsServer){
            GameManager.instance.lastPlayerTeam.Value = (GameManager.instance.lastPlayerTeam.Value + 1) % 2;
            GameManager.instance.uIManager.textRedTeamDebug.text = GameManager.instance.lastPlayerTeam.Value.ToString();
        }

        if(IsOwner)
            rb = GetComponent<Rigidbody>();
    }

    public void Update(){
        if(!IsOwner){ return; }
        if(!hasInitialized){
            hasInitialized = true;
            Team _team = (Team)GameManager.instance.lastPlayerTeam.Value;
            team.Value = _team;
            GameManager.instance.AddPlayer(this);
        }
        Move();
    }

    private void Move(){
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if(moveDir != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        rb.velocity +=  moveDir * speed * Time.deltaTime;
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