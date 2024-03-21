using UnityEngine;
using Unity.Netcode;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour {

    private Rigidbody rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 300f;

    [SerializeField] private Renderer[] renderers;


    public NetworkVariable<bool> hasFrog = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public GameObject frog;
    public GameObject gameFrog;
    public NetworkVariable<Team> team = new NetworkVariable<Team>(Team.NONE,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private readonly ulong[] targetClientsArray = new ulong[1];
    public bool hasInitialized = false;

    //combate aqui em baixo renan
    
    public float throwForce = 0f;
    public bool hasRock;
    public GameObject rockProjectile;
    public Transform rockAimSpot;


    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();

        if (IsServer) {
            GameManager.instance.lastPlayerTeam.Value = GameManager.instance.lastPlayerTeam.Value + 1 > 2 ? 1 : GameManager.instance.lastPlayerTeam.Value + 1;
        }

        if (IsOwner){
            rb = GetComponent<Rigidbody>();
        }
        ChangeColor(team.Value);

        hasFrog.OnValueChanged += delegate { OnHasFrogChanged(); };
        team.OnValueChanged += delegate { ChangeColor(team.Value); };
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (!IsServer) GameManager.instance.uIManager.Panel.SetActive(true);
    }

    public void Update(){
        if (!IsOwner) { return; }
        if (!hasInitialized) {
            hasInitialized = true;
            Team _team = (Team)GameManager.instance.lastPlayerTeam.Value;
            team.Value = _team;
            GameManager.instance.AddPlayer(this);
        }
        
        Throw();
        
        Move();
    }

    private void ChangeColor(Team team){
        if(team == Team.RED){
            for (int i = 0; i < renderers.Length; i++){
                renderers[i].material.color = Color.red;
            }
        }else{
            for (int i = 0; i < renderers.Length; i++){
                renderers[i].material.color = Color.blue;
            }
        }
    }
    private void Move() {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if (moveDir != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        rb.velocity +=  moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (!IsOwner) return;
        if (!other.CompareTag("Frog")) return; 
        hasFrog.Value = true;

        
    }

 

    [ServerRpc(RequireOwnership = false)]
    public void LooseFrogServerRpc(ulong player1) {
        targetClientsArray[0] = player1;
        ClientRpcParams clientRpcParams = new ClientRpcParams{
            Send = new ClientRpcSendParams
            {
                TargetClientIds = targetClientsArray
            }
        };
        ThrowFrogClientRpc(clientRpcParams);
    }

    [ClientRpc(RequireOwnership = false)]
    private void ThrowFrogClientRpc(ClientRpcParams clientRpcParams = default) {
        if (!IsOwner) return;
        hasFrog.Value = false; 
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Rock") && hasRock == false)
        {
            hasRock = true;
            Destroy(other.gameObject);
        }
        if (!IsOwner) return;
        if (!other.collider.CompareTag("Player")) return;

        if (other.gameObject.TryGetComponent(out PlayerController player)) {
            player.LooseFrogServerRpc(player.OwnerClientId);
        }
        if (hasFrog.Value) {
            hasFrog.Value = false;
        }
        
            
        Debug.Log("Collided with player");
    }

    public void OnHasFrogChanged() {
        if (hasFrog.Value) {
            frog.SetActive(true);
            GameManager.instance.frogController.gameObject.SetActive(false);
        } else {
            frog.SetActive(false);
            GameManager.instance.frogController.gameObject.SetActive(true);
            GameManager.instance.frogController.transform.SetPositionAndRotation(new Vector3(0, 16, 0), Quaternion.identity);
        }
    }

    public void Throw()
    {
        if (hasRock)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (throwForce <= 10)
                    throwForce += 5 * Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                GameObject rock = Instantiate(rockProjectile, rockAimSpot.position, rockAimSpot.rotation);
                if (rock.gameObject.TryGetComponent<Rigidbody>(out Rigidbody r))
                {
                    r.AddForce(r.transform.forward * throwForce, ForceMode.Impulse);
                }
                throwForce = 0;
                hasRock = false;
            }
        }
        
    }
}