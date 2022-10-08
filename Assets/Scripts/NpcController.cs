using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    public Transform player;
    public GameObject deliver, done;
    private bool status;
    private PlayerController control;
    private GameObject plyr;
    private SpriteRenderer spr;

    [SerializeField]
    private int number;

    private void Awake()
    {
        deliver.SetActive(false);
        plyr = GameObject.Find("Player");
        control = plyr.GetComponent<PlayerController>();
        this.spr = this.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        status = false; //status initial belum delivery
    }

    // Update is called once per frame
    void Update()
    {
        this.spr.flipX = player.transform.position.x < this.transform.position.x; //flip npc ke arah player
    }

    private void delivery(Collider2D collision)
    {
        if (collision.CompareTag("Player") && status == false) //jika trigger collide dengan player dan status belum deliver
        {
            deliver.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (number == control.order)
                {
                    control.addScore();
                    control.order++;
                    deliver.SetActive(false);
                    status = true;
                }
            }
        }
        else if (collision.CompareTag("Player") && status == true) //jika trigger collide dengan player dan status sudah deliver
        {
            done.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        delivery(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        delivery(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) //menghilangkan text status delivery ketika player keluar dari area trigger
    {
        deliver.SetActive(false);
        done.SetActive(false);
    }
}
