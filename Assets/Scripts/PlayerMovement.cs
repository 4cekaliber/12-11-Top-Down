using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [SerializeField]private InputActionReference move;

    private GameObject player;
    private PlayerHealthManager healthManager;

    private GameObject statManager;
    private StatScript statManagerScript;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    private GameObject Key1;
    private GameObject Shotgun;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        move.action.Enable();

        player = GameObject.Find("Player");
        healthManager = player.GetComponent<PlayerHealthManager>();

        statManager = GameObject.Find("StatManager");
        statManagerScript = statManager.GetComponent<StatScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Key1 = GameObject.Find("Key");
        Shotgun = GameObject.Find("Shotgun");


    }
    void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (moveDirection != Vector2.zero)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x*moveSpeed, moveDirection.y*moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D context)
    {
        if (context.gameObject.tag == "Enemy" )
        {
            healthManager.takeDamage(10);
        }

    }

    private void OnTriggerStay2D(Collider2D context)
    {
        //Debug.Log("Enter trigger");
        if ((context.gameObject.tag == "Key1") && (Input.GetKeyDown(KeyCode.F)))
        {
            Debug.Log("grabbing key");
            statManagerScript.grabbed("Key1");
            Key1.SetActive(false);
        }

        if ((context.gameObject.tag == "Shotgun") && (Input.GetKeyDown(KeyCode.F)))
        {
            Debug.Log("grabbing Shotgun");
            statManagerScript.grabbed("Shotgun");
            Shotgun.SetActive(false);
        }
    }

    
}
   
