using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class cowboyScript : MonoBehaviourPun
{
    public float movespeed = 5;
    private float jumpforce=60000 ;

    public GameObject playercamera;
    public GameObject bulletprefab;

    public Transform bulletspawnpointright;
    public Transform bulletspawnpointleft;

    public SpriteRenderer sprite;

    public PhotonView photonview;

    public Animator animatior;

    public Text playername;

    private bool allowmoving = true;
    public bool isground = false;
    public bool disableinputs = false;

    private Rigidbody2D rigidbod;

    public string myName;
    private void Awake()
    {
        if (photonview.IsMine)
        {
            gamemenager.instance.localPlayer = this.gameObject;
            playercamera.SetActive(true);
            playercamera.transform.SetParent(null,false);
            playername.text ="You :" + PhotonNetwork.NickName;
            playername.color = Color.green;

            myName = PhotonNetwork.NickName;


        }
        else
        {
            playername.text = photonView.Owner.NickName;
            playername.color = Color.red;
        }
    }

    void Start()
    {
        rigidbod = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if (photonView.IsMine&& !disableinputs)
        {
            checkInputs();
        }
        
    }
    private void checkInputs()
    {
        if (allowmoving)
        {
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += movement * movespeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.RightControl)&&animatior.GetBool("IsMove")==false)
        {
            shot();
        }
        else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            animatior.SetBool("IsShot", false);
            allowmoving = true;
        }
        if (Input.GetKeyDown(KeyCode.Space)&&isground)
        {
            jump();
        }
        if (Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.A))
        {
            animatior.SetBool("IsMove", true);
        }
        if (Input.GetKeyDown(KeyCode.D) && animatior.GetBool("IsShot") == false)
        {
            playercamera.GetComponent<CamerFollow2D>().offset = new Vector3(1.3f,1.53f,0);
            photonView.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            animatior.SetBool("IsMove", false);
        }
        if (Input.GetKeyDown(KeyCode.A) && animatior.GetBool("IsShot") == false)
        {
            playercamera.GetComponent<CamerFollow2D>().offset = new Vector3(-1.3f, 1.53f, 0);
            photonView.RPC("FlipSprite_Left", RpcTarget.AllBuffered);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            animatior.SetBool("IsMove", false);
        }
    }
    [PunRPC]
    private void FlipSprite_Right()
    {
        sprite.flipX = false;
    }
    [PunRPC]
    private void FlipSprite_Left()
    {
        sprite.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="ground")
        {
            isground = false;
        }
    }
    void jump()
    {
        rigidbod.AddForce(new Vector2(0, jumpforce * Time.deltaTime));
    }

    void shot()
    {
        if (sprite.flipX==false)
        {
            GameObject bullete = PhotonNetwork.Instantiate(bulletprefab.name,
                new Vector2(bulletspawnpointright.position.x, bulletspawnpointright.position.y), Quaternion.identity, 0);

            bullete.GetComponent<bullete>().localPlayerObj = this.gameObject;   
        
        
        }
        if (sprite.flipX==true)
        {
            GameObject bullete = PhotonNetwork.Instantiate(bulletprefab.name,
                new Vector2(bulletspawnpointleft.position.x, bulletspawnpointleft.position.y), Quaternion.identity, 0);
            
            
            bullete.GetComponent<bullete>().localPlayerObj = this.gameObject;
            bullete.GetComponent<PhotonView>().RPC("changeDirection", RpcTarget.AllBuffered);
        }
        animatior.SetBool("IsShot", true);
        allowmoving = false;
    }
}
