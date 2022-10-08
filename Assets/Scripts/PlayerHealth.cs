using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int health;

    [SerializeField]
    private GameObject[] healthUI;

    [SerializeField]
    private GameObject gameoverUI;

    private Vector2 originalpos;

    private void Awake()
    {
        originalpos = transform.position; //save initial position
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDmg() //player took damage and teleported to initial position
    {
        health--;
        healthUI[health].SetActive(false);
        transform.position = originalpos;
        if (health <= 0)
        {
            health = 0;
            lose();
        }
    }

    public void lose() //lose the game, game over
    {
        gameoverUI.SetActive(true);
        Time.timeScale = 0;
    }
}
