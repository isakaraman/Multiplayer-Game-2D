using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class health : MonoBehaviourPun
{ 
    public Image fillimage;

    public float heaelth = 1f;

    public Rigidbody2D rb;

    public SpriteRenderer sr;

    public BoxCollider2D colleider;

    public GameObject playerCanvas;

    public cowboyScript playerScript;

    public GameObject killGotKilledText;

    public void CheckHealth()
    {
        if (photonView.IsMine&&heaelth<=0)
        {
            gamemenager.instance.EnableRespawn();

            playerScript.disableinputs = true;
            GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void death()
    {
        rb.gravityScale = 0;
        colleider.enabled = false;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }

    [PunRPC]
    public void healthUpdate(float Damage)
    {
        fillimage.fillAmount -= Damage;
        heaelth = fillimage.fillAmount;
        CheckHealth();
    }
    [PunRPC]
    public void revive()
    {
        rb.gravityScale = 1;
        colleider.enabled = true;
        sr.enabled = true;
        playerCanvas.SetActive(true);
        fillimage.fillAmount = 1;
        heaelth = 1;
    }
    public void EnableInputs()
    {
        playerScript.disableinputs = false;
    }

     [PunRPC]
     public void YouGotKilledBy(string name)
    {
        GameObject go = Instantiate(killGotKilledText, new Vector2(0, 0), Quaternion.identity);

        go.transform.SetParent(gamemenager.instance.KillGotKillTextBox.transform, false);

        go.GetComponent<Text>().text = "You Got Killed By : " + name;
        go.GetComponent<Text>().color = Color.red;

    }

    [PunRPC]
    public void YouKilled(string name)
    {
        GameObject go = Instantiate(killGotKilledText, new Vector2(0, 0), Quaternion.identity);

        go.transform.SetParent(gamemenager.instance.KillGotKillTextBox.transform, false);

        go.GetComponent<Text>().text = "You Killed : " + name;
        go.GetComponent<Text>().color = Color.green;
    }
}
