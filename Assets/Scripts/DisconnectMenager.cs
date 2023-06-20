using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectMenager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject canvas, menuButton, reconnectButton;
    [SerializeField] private Text statusText;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Application.internetReachability==NetworkReachability.NotReachable)
        {
            canvas.SetActive(true);

            if (SceneManager.GetActiveScene().buildIndex==0)
            {
                reconnectButton.SetActive(true);
                statusText.text = "Lost connection to photon,pls try to connect";
            }
            if (SceneManager.GetActiveScene().buildIndex==1)
            {
                menuButton.SetActive(true);
                statusText.text = "Lost connection to photon,pls try to connect in the main menu";
            }
        }
    }
    public void OnClick_TryConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void OnClick_Menu()
    {
        PhotonNetwork.LoadLevel(0);
    }
    public override void OnConnectedToMaster()
    {
        if (canvas.activeSelf)
        {
            canvas.SetActive(false);
            menuButton.SetActive(false);
            reconnectButton.SetActive(false);
        }
        
    }
}
