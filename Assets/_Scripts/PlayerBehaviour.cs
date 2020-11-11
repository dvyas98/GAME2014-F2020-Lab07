using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Joystick joystick;
    public float joystickHorizontalSenstivity;
    public float joystickVerticalSensitivity;
    public float horiontalForce;
    public float verticalForce;
    bool isGrounded;
    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    public Transform spawnPoint;
    public bool isJumping;
    public bool isCrouching;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _move(); 
    }
     
    void _move()
    {
        if (isGrounded)
        {
            if(!isJumping && !isCrouching)
            {
                if (joystick.Horizontal > joystickHorizontalSenstivity)
                {
                    //Move Right
                    m_rigidbody2D.AddForce(Vector2.right * horiontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = false;
                    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);
                }
                else if (joystick.Horizontal < -joystickHorizontalSenstivity)
                {
                    //Move Left
                    m_rigidbody2D.AddForce(Vector2.left * horiontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);

                }

                else 
                {
                    //Idle
                    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.IDLE);

                }
            }
            
        }
        if ((joystick.Vertical > joystickVerticalSensitivity) && (!isJumping))
        {
            m_rigidbody2D.AddForce(Vector2.up * verticalForce * verticalForce);
            m_animator.SetInteger("AnimState", (int)PlayerAnimationType.JUMP);
            isJumping = true;
        }
        else 
        {
            isJumping = false;
        }
        if ((joystick.Vertical < -joystickVerticalSensitivity) && (!isJumping))
        {
            m_animator.SetInteger("AnimState", (int)PlayerAnimationType.CROUCH);
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {      
            if (other.gameObject.CompareTag("Platforms"))
            {
                isGrounded = true;
            }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //respawn
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            transform.position = spawnPoint.position;
        }
       
    }
}
