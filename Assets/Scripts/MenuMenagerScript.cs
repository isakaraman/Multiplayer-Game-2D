using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class MenuMenagerScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject userNameScreen, createScreen, createNameButton;

    [SerializeField] private InputField usernameField, createRoomField, joinRoomField;


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("bağlandı");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ye");
        if (!createScreen.activeSelf)
        {
            userNameScreen.SetActive(true);
        }
       
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void OnClick_CreateNameButton()
    {
        PhotonNetwork.NickName = usernameField.text;
        userNameScreen.SetActive(false);
        createScreen.SetActive(true);
    }

    public void OnNameField_Changed()
    {
        if (usernameField.text.Length>=3)
        {
            createNameButton.SetActive(true);
        }
        else
        {
            createNameButton.SetActive(false);
        }
    }

    public void Onclick_JoinRoom()
    {
        RoomOptions ru = new RoomOptions();
        ru.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom(joinRoomField.text, ru, TypedLobby.Default);
    }
    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomField.text, new RoomOptions { MaxPlayers = 4 }, null);
    }
}


