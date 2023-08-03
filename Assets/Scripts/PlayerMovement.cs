using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float fallGravity = 5f;
    public float jumpGravity = 4.5f;
    //private float peakHeight;
    //private float? previousHeight = null;

    /********************** Player Attributes *****************************/
    private bool combatRange = false;
    /********************** Player Attributes *****************************/

    /********************** Enemy Attributes *****************************/
    private int health = 100;
    /********************** Enemy Attributes *****************************/

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    private SpriteRenderer enemy;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 20f;  //7
    [SerializeField] private float jumpForce = 18f; //14

    private GameObject hitbox;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Find the Hitbox GameObject by name and store a reference to it
        hitbox = transform.Find("Hitbox").gameObject;
        // Ensure the Hitbox GameObject is initially inactive
        hitbox.SetActive(false);
    }

    private enum MovementState { idle, running, jumping, falling , attack1 }
    private MovementState state = MovementState.idle;

    void Update()
    {
        //Assign horizontal velocity
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        //Assign jumping force
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = jumpGravity;
        }
        
        //Assign a separate upward-jumping gravity and a falling gravity, allowing for faster falling.
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = jumpGravity;
        } else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallGravity;
        }

        UpdateAnimationState();
    }

    //Assign the animation state to the plater
    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            state = MovementState.attack1;
            
            //Register the player's attack and deduct 10 HP from the enemy
            if (combatRange == true)
            {
                health -= 10;
                Debug.Log("Enemy Health: " + health);
                if (health <= 0)
                {
                    enemyObject.SetActive(false);
                }
            }
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        combatRange = true;
    }

    GameObject enemyObject;
    public void OnTriggerStay2D(Collider2D other)
    {
        enemyObject = other.gameObject;
        combatRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        combatRange = false;
    }
}
