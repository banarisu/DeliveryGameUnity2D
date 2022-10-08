using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] //enable variable to be inputted from inspector
    private float movespeed;

    [SerializeField]
    private bool isFacingRight;
   
    [SerializeField]
    private float radius;

    [SerializeField]
    private Transform[] groundCheckers;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl; //jika dinyalakan bisa bergerak ketika di melompat

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject pause, winUI, nextorderUI, mainUI;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerHealth life;
    private bool isGrounded, jump, falling;

    public Text score, highscore, finalscore, winscore, nextorder;
    
    public int skor, highskor;
    public int order;

    private void Awake()
    {
        life = this.GetComponent<PlayerHealth>();
        mainUI.SetActive(true);
        nextorderUI.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        skor = 0;
        highskor = PlayerPrefs.GetInt("highscore", 0);
        highscore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        isFacingRight = true;
        order = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        score.text = skor.ToString();

        if (Input.GetKeyDown(KeyCode.Escape)) //tombol escape untuk pause dan resume
        {
            if (pause.activeInHierarchy == false)
            {
                pauseGame();
            }
            else
            {
                resumeGame();
            }
        }
        if(order <= 5) //jika order belum selesai semua, tetap hitung dan proses "next order"
        {
            asciConversionToString(order);
        }
    }

    void FixedUpdate()
    {
        float move = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGrounded();
        HandleMovement(move);        
        Flip(move);
        HandleLayers();
        Reset();
    }

    private void HandleMovement(float move)
    {
        if (rb.velocity.y < 0) //animation on falling down
        {
            anim.SetBool("land", true);
            falling = true;
        }
        if (isGrounded || airControl) //move if grounded or air control turned on
        {
            rb.velocity = new Vector2(move * movespeed, rb.velocity.y);
            anim.SetFloat("speed", Mathf.Abs(move));
        }
        if (isGrounded && jump) //jumping if grounded
        {
            rb.AddForce(new Vector2(0, jumpForce));
            anim.SetTrigger("jump");
            isGrounded = false;
        }
    }

    private void HandleInput() //manage input
    {
        if (Input.GetKeyDown(KeyCode.Space)) //jump input
        {
            jump = true;
        }
    }

    private void Flip(float move) //flip player
    {
        if (move > 0 && !isFacingRight || move < 0 && isFacingRight) 
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void Reset() //reset after every action
    {
        jump = false;
        if (isGrounded)
        {
            falling = false;
        }
        clearGame();
    }

    private bool IsGrounded() //check if player grounded
    {
        if (rb.velocity.y <= 0)
        {
            foreach (Transform point in groundCheckers)
            {
                Collider2D[] collides = Physics2D.OverlapCircleAll(point.position, radius, whatIsGround);
                for (int i = 0; i < collides.Length; i++)
                {
                    if (collides[i].gameObject != gameObject)
                    {
                        anim.ResetTrigger("jump");
                        anim.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void pauseGame() //pause game
    {
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void resumeGame() //resume game
    {
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void restartGame() //restart game
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame() //quit game, load initial menu scene
    {
        SceneManager.LoadScene("InitialMenu");
    } 

    private void clearGame() //win game
    {
        if(order > 5)
        {
            Time.timeScale = 0;
            nextorderUI.SetActive(false);
            winUI.SetActive(true);
        }
    }

    private void HandleLayers() //manage jumping animation
    {
        if (!isGrounded)
        {
            anim.SetLayerWeight(1, 1);
        }
        else
        {
            anim.SetLayerWeight(1, 0);
        }
    }

    public void asciConversionToString(int code) //convert order number to string to show on "next order"
    {
        code += 96;
        char tempChar = (char)code;
        string tempString = char.ToString(tempChar);
        nextorder.text = tempString;
        print("Next : Asci value is "+ code + " and String is "+ tempChar);
        code -= 96;
    }

    public void addScore() //add score after defeat enemy or success delivery
    {
        skor++;
        finalscore.text = skor.ToString();
        winscore.text = skor.ToString();

        if (skor > highskor)
        {
            highskor = skor;
            PlayerPrefs.SetInt("highscore", highskor);
        }
        highscore.text = PlayerPrefs.GetInt("highscore").ToString();
    }

    /*private void resetHS()
    {
        PlayerPrefs.SetInt("Highscore", 0);
    }*/

    private void OnTriggerEnter2D(Collider2D collision) //check collision with enemy and water
    {
        if (collision.CompareTag("Enemy"))
        {
            if (falling) //if collide with enemy while falling, step on the enemy
            {
                print("Enemy destroyed on falling");    
                Destroy(collision.transform.parent.gameObject);
                addScore();
            }
            else
            {
                print("Player took damage from enemy");
                life.takeDmg();
            }
        }
        else if (collision.CompareTag("Water")) //tenggelam
        {
            print("Hello i'm under the water");
            life.takeDmg();
        }
    }

}
