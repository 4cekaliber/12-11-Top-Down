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

    private GameObject statManager;
    private StatScript statManagerScript;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    private GameObject Key1;
    private GameObject Key1Image;

    private GameObject inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        move.action.Enable();

        statManager = GameObject.Find("StatManager");
        statManagerScript = statManager.GetComponent<StatScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Key1 = GameObject.Find("Key");
        inventory = GameObject.Find("Inventory");
        inventory.SetActive(false);
        

        Key1Image = GameObject.Find("Key Image");
        Key1Image.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(statManagerScript.getItemState("Key1"));
            inventoryActivation();
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
            statManagerScript.takeDamage(10);
        }

    }

    private void OnTriggerStay2D(Collider2D context)
    {
        Debug.Log("Enter trigger");
        if ((context.gameObject.tag == "Key1") && (Input.GetKeyDown(KeyCode.F)))
        {
            Debug.Log("grabbing key");
            statManagerScript.grabbed("Key1");
            Key1.SetActive(false);
        }
    }

    private void inventoryActivation()
    {
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
            Key1Image.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
            if (statManagerScript.getItemState("Key1"))
            {
                Key1Image.SetActive(true);
            }
        }

        
    }
}
   
