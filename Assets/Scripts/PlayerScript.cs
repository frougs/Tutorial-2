using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text lives;
    private int livesValue = 3;
    public Text winText;
    public Text loseText;
    public float jumpForce = 3;
    private int stage = 1;
    private int win = 0;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public AudioClip bkgrndMusic;
    public AudioClip winMusic;
    public AudioSource musicSource;
    public AudioClip coinSound;
    public AudioSource soundEffect;

    private bool facingRight = true;

        

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winText.enabled = false;
        loseText.enabled = false;
        musicSource.clip = bkgrndMusic;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
        soundEffect.clip = coinSound;
        soundEffect.loop = false;
       

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (hozMovement > 0 && facingRight == true)
        {
            Debug.Log ("Facing Right");
        }

        if (hozMovement < 0 && facingRight == false)
        {
            Debug.Log ("Facing Left");
        }

        if (verMovement > 0 && isOnGround == false)
        {

            Debug.Log ("Jumping");
            anim.SetInteger("State", 2);
        }

        if (isOnGround == false && Input.GetKey(KeyCode.W) == false)
        {
            
            Debug.Log ("Falling");
            anim.SetInteger("State", 3);
        }

        if (verMovement == 0 && hozMovement == 0 && isOnGround == true)
        {
            anim.SetInteger("State", 0);
        }

        if (hozMovement != 0 && isOnGround == true)
        {
            anim.SetInteger("State", 1);
        }

        if (isOnGround == true)
        {
            Debug.Log("Touching Ground");
        }
        
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
 
        if(collision.collider.tag == "Ground" && isOnGround == true)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
            }
        }

        if(collision.collider.tag =="Lava")
        {
            livesValue = 0;
        }

    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         if(other.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            other.gameObject.SetActive(false);
            soundEffect.Play();
        }

        if(other.tag == "Enemy")
        {
        livesValue -= 1;
        lives.text = "Lives: " + livesValue.ToString();
        other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
            }
        }

    }
    void Update()
    {
        if(livesValue <= 0)
        {
            loseText.enabled = true;
            Destroy(gameObject);
        }

        if(stage == 1)
        {
            if(scoreValue == 4)
            {
                transform.position = new Vector2(135, 0);
                livesValue = 3;
                stage = 2;
                lives.text = "Lives: " + livesValue.ToString();
            }
        }  

        if(scoreValue == 8)
        {
            winText.enabled = true;
            if (win == 0)
            {
            musicSource.loop = false;
            musicSource.clip = winMusic;
            musicSource.Play();
            win = 1;
            }
        }  

    } 

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }  
}
