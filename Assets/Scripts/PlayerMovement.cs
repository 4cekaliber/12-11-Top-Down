using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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

    public int currentFloor;
    private GameObject firstBackground;
    private GameObject firstWalls;
    private GameObject bridgeDoorClose;
    private GameObject bridgeDoorOpen;
    private GameObject firstStairsBackground;
    private GameObject secondBackground;




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

        firstBackground = GameObject.Find("Main Building (1)");
        firstWalls = GameObject.Find("Main Building Collision(1)");
        bridgeDoorClose = GameObject.Find("Main Building Bridge Door (Close)");
        bridgeDoorOpen = GameObject.Find("Main Building Bridge Door (Open)");
        firstStairsBackground = GameObject.Find("Main Building (2.5)");
        secondBackground = GameObject.Find("Main Building (2)");
        

        //firstBackgroundRenderer = firstBackground.GetComponent<TilemapRenderer>();
        //firstWallsRenderer = firstWallsRenderer.GetComponent<TilemapRenderer>();
        //firstStairsBackgroundRenderer;
        //secondBackgroundRenderer;

        currentFloor = 1;

        firstStairsBackground.SetActive(false);
        secondBackground.SetActive(false);


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
        if (healthManager.playerStamina <= 0)
        {
            moveSpeed = 0.5f;
        }
        rb.linearVelocity = new Vector2(moveDirection.x*moveSpeed, moveDirection.y*moveSpeed);
    }

    
    private void OnTriggerEnter2D(Collider2D context)
    {
        if (context.gameObject.name == "Main Building (1)")
        {
            firstBackground.SetActive(false);
            firstStairsBackground.SetActive(true);
            secondBackground.SetActive(true);
            currentFloor = 2;
        }
        if (context.gameObject.name == "Main Building (2.5)")
        {
            firstBackground.SetActive(true);
            firstStairsBackground.SetActive(false);
            secondBackground.SetActive(false);
            currentFloor = 1;
        }
        if (context.gameObject.name == "Main Building (2)")
        {
            
            if (context.GetType() == typeof(BoxCollider2D))
            {
                firstStairsBackground.SetActive(true);
            }
            else if (context.GetType() == typeof(PolygonCollider2D))
            {
                firstStairsBackground.SetActive(false);
            }
            //print("Colliding with: " + context.GetType());
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

        if ((context.gameObject.name == "Yard Door (1)") && (Input.GetKeyDown(KeyCode.F)))
{
            statManagerScript.interactDoor(context.gameObject);
        }

        if ((context.gameObject.name == "Yard Door (2)") && (Input.GetKeyDown(KeyCode.F)))
        {
            statManagerScript.interactDoor(context.gameObject);
        }

        if ((context.gameObject.name == "Security Door(1)") && (Input.GetKeyDown(KeyCode.F)))
        {
            statManagerScript.interactDoor(context.gameObject);
        }

        if ((context.gameObject.name == "Security Door (2)") && (Input.GetKeyDown(KeyCode.F)))
        {
            statManagerScript.interactDoor(context.gameObject);
        }

        if ((context.gameObject.name == "Closet") && (Input.GetKeyDown(KeyCode.F)))
        {
            statManagerScript.interactDoor(context.gameObject);
        }

        if ((context.gameObject.name == "Med Bay") && (Input.GetKeyDown(KeyCode.F)))
        {
            statManagerScript.interactDoor(context.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D context)
    {
        if ((context.gameObject.name == "Main Building Bridge Door (Close)") && (Input.GetKeyDown(KeyCode.F)))
        {
            bridgeDoorClose.SetActive(false);
        }
    }


}
   
