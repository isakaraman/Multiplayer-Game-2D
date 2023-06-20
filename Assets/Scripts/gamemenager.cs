using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class gamemenager : MonoBehaviourPunCallbacks
{
    public GameObject playerprefab;
    public GameObject canvas;
    public GameObject scenecam;
    public GameObject respawnUI;
    [HideInInspector]
    public GameObject localPlayer;

    public Text spawnTimer;

    [HideInInspector]
    public static gamemenager instance = null;

    private float timeAmount;

    private bool startSpawn;

    public GameObject KillGotKillTextBox;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (startSpawn)
        {
            startreSpawn();
        }
    }
    
    public void spawnplayer()
    {
        float randomvalue = Random.Range(-5f, 5f);
        PhotonNetwork.Instantiate(playerprefab.name, new Vector2(playerprefab.transform.position.x * randomvalue, playerprefab.transform.position.y), Quaternion.identity,0);

        canvas.SetActive(false);
        scenecam.SetActive(false);
    
    }
    public void EnableRespawn()
    {
        timeAmount = 5;
        startSpawn = true;
        respawnUI.SetActive(true);
    }
    public void startreSpawn()
    {
        timeAmount -= Time.deltaTime;
        spawnTimer.text = "Respawn in: " + timeAmount.ToString();

        if (timeAmount<=0)
        {
            respawnUI.SetActive(false);
            startSpawn = false;
            playerRelocation();
            localPlayer.GetComponent<health>().EnableInputs();
            localPlayer.GetComponent<PhotonView>().RPC("revive", RpcTarget.AllBuffered);
        }
    }

  
    void playerRelocation()
    {
        float randomPosition = Random.Range(-5f, 5f);
        localPlayer.transform.localPosition = new Vector2(randomPosition, 2f);
    }
}
