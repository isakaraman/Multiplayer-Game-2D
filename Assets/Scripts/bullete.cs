using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class bullete : MonoBehaviourPun
{
    public bool movingDirections;
    
    public float moveSpeed;
    public float bulleteDamage = 0.3f;
    
    public string killerName;
    
    public GameObject localPlayerObj;

    
    void Start()
    {
        if (photonView.IsMine)
        {
            killerName = localPlayerObj.GetComponent<cowboyScript>().myName;
        }
    }

    void Update()
    {
        if (!movingDirections)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }
    [PunRPC]
    public void changeDirection()
    {
        movingDirections = true;
    }


    [PunRPC]
    private void Destroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine)
        {
            return;
        }

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target !=null&&(!target.IsMine))
        {
            if (target.tag=="Player")
            {
                target.RPC("healthUpdate", RpcTarget.AllBuffered,bulleteDamage);

                if (target.GetComponent<health>().heaelth<=0)
                {
                    Player gotKilled = target.Owner;

                    target.RPC("YouGotKilledBy", gotKilled, killerName);
                    target.RPC("YouKilled", localPlayerObj.GetComponent<PhotonView>().Owner, target.Owner.NickName);
                }
            
            }
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
        }
    }
}
