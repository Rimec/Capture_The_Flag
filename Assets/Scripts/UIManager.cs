using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button buttonStartHost;
    [SerializeField] private Button buttonStartClient;
    [SerializeField] private TextMeshProUGUI textCurrentTeam;
    [SerializeField] private TextMeshProUGUI textBlueTeamPoints;
    [SerializeField] private TextMeshProUGUI textRedTeamPoints;
    private void Start(){
        panel.SetActive(true);
        buttonStartHost.onClick.AddListener(delegate { NetworkManager.Singleton.StartHost(); panel.SetActive(false); });
        buttonStartClient.onClick.AddListener(delegate { NetworkManager.Singleton.StartClient(); panel.SetActive(false); });
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
