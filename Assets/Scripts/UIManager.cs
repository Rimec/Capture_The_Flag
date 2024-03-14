using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button buttonStartServer;
    [SerializeField] private Button buttonStartHost;
    [SerializeField] private Button buttonStartClient;
    [SerializeField] private Button buttonExit;
    [SerializeField] private TextMeshProUGUI textCurrentTeam;
    [SerializeField] private TextMeshProUGUI textBlueTeamPoints;
    [SerializeField] private TextMeshProUGUI textRedTeamPoints;
    [SerializeField] private TMP_InputField inputFieldIp;

    public GameObject Panel {get => panel; set {panel = value;} }
    private void Start(){
        panel.SetActive(true);
        buttonStartServer.onClick.AddListener(delegate { NetworkManager.Singleton.StartServer(); panel.SetActive(false); });
        buttonStartHost.onClick.AddListener(delegate { NetworkManager.Singleton.StartHost(); panel.SetActive(false); });
        buttonStartClient.onClick.AddListener(delegate { NetworkManager.Singleton.StartClient(); panel.SetActive(false); });
        buttonExit.onClick.AddListener(delegate { SceneManager.LoadScene(0); });
        inputFieldIp.onEndEdit.AddListener(delegate { if(inputFieldIp.text != "") {UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport; transport.ConnectionData.Address = inputFieldIp.text;} });
        inputFieldIp.onValueChanged.AddListener(delegate { if(inputFieldIp.text != "") {UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport; transport.ConnectionData.Address = inputFieldIp.text;} });
        inputFieldIp.onSubmit.AddListener(delegate { if(inputFieldIp.text != "") {UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport; transport.ConnectionData.Address = inputFieldIp.text;} });
    }

    public void SetRedTeamPoints(int points){
        textRedTeamPoints.text = "Red: " + points;
    }
    public void SetBlueTeamPoints(int points){
        textBlueTeamPoints.text = "Blue: " + points;
    }

    public void SetCurrentTeamText(string text){
        textCurrentTeam.text = text;
    }
}
