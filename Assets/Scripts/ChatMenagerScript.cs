using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChatMenagerScript : MonoBehaviourPun, IPunObservable
{
    public PhotonView photonwiev;

    public GameObject BubbleSpeech;

    public Text ChatText;

    public cowboyScript player;


    InputField ChatInput;

    bool disableSend;

    private void Awake()
    {
        ChatInput = GameObject.Find("ChatInputField").GetComponent<InputField>(); // sahnedeki obje
    }


    void Update()
    {
        if (photonView.IsMine) //player oyunun içerisindeyse 
        {
            if (true)
            {
                if (ChatInput.isFocused) // mouse ile üzerine geldiysek
                {
                    player.disableinputs = true;
                }
                else
                {
                    player.disableinputs = false;
                }
                if (!disableSend && ChatInput.isFocused)
                {
                    if (ChatInput.text != "" && ChatInput.text.Length > 1 && Input.GetKeyDown(KeyCode.Space))
                    {
                        photonView.RPC("SendMsg", RpcTarget.AllBuffered, ChatInput.text);

                        BubbleSpeech.SetActive(true);
                        ChatInput.text = "";
                        disableSend = true;
                    }
                }
            }

        }
    }

    [PunRPC] //herkesin görmesi için yazılması gerek
    void SendMsg(string msg)
    {
        ChatText.text = msg;

        StartCoroutine(hideBubbleSpeech());
    }

    IEnumerator hideBubbleSpeech()
    {
        yield return new WaitForSeconds(3f);
        disableSend = false;
        BubbleSpeech.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(BubbleSpeech.activeSelf);//hiyerarşideki akiftliği check
        }
        else if (stream.IsReading)// yazma değilde okuma işlemi mi yapılıyor check
        {
            BubbleSpeech.SetActive((bool)stream.ReceiveNext());
        }
    }
  
    

}
