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
    private GameObject yardDoor1;
    private GameObject yardDoor1Open;
    private GameObject yardDoor1Close;
    private GameObject yardDoor2;
    private GameObject yardDoor2Open;
    private GameObject yardDoor2Close;
    private GameObject securityDoor1;
    private GameObject securityDoor1Open;
    private GameObject securityDoor1Close;
    private GameObject securityDoor2;
    private GameObject securityDoor2Open;
    private GameObject securityDoor2Close;
    private GameObject closetDoor;
    private GameObject closetDoorOpen;
    private GameObject closetDoorClose;
    private GameObject medDoor;
    private GameObject medDoorOpen;
    private GameObject medDoorClose;



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
        yardDoor1 = GameObject.Find("Yard Door (1)");
        yardDoor1Open = yardDoor1.transform.Find("Open").gameObject;
        yardDoor1Close = yardDoor1.transform.Find("Close").gameObject;
        yardDoor2 = GameObject.Find("Yard Door (2)");
        yardDoor2Open = yardDoor2.transform.Find("Open").gameObject;
        yardDoor2Close = yardDoor2.transform.Find("Close").gameObject;
        securityDoor1 = GameObject.Find("Security Door (1)");
        securityDoor1Open = securityDoor1.transform.Find("Open").gameObject;
        securityDoor1Close = securityDoor1.transform.Find("Close").gameObject;
        securityDoor2 = GameObject.Find("Security Door (2)");
        securityDoor2Open = securityDoor2.transform.Find("Open").gameObject;
        securityDoor2Close = securityDoor2.transform.Find("Close").gameObject;
        closetDoor = GameObject.Find("Closet");
        closetDoorOpen = closetDoor.transform.Find("Open").gameObject;
        closetDoorClose = closetDoor.transform.Find("Close").gameObject;
        medDoor = GameObject.Find("Med Bay");
        medDoorOpen = medDoor.transform.Find("Open").gameObject;
        medDoorClose = medDoor.transform.Find("Close").gameObject;



        //bridgeDoorClose.SetActive(false);
        firstStairsBackground.SetActive(false);
        secondBackground.SetActive(false);
        yardDoor1Open.SetActive(false);
        yardDoor2Open.SetActive(false);
        securityDoor1Open.SetActive(false);
        securityDoor2Open.SetActive(false);
        closetDoorOpen.SetActive(false);
        medDoorOpen.SetActive(false);

        yardDoor1Close.SetActive(true);
        yardDoor2Close.SetActive(true);
        securityDoor1Close.SetActive(true);
        securityDoor2Close.SetActive(true);
        closetDoorClose.SetActive(true);
        medDoorClose.SetActive(true);

        //firstBackgroundRenderer = firstBackground.GetComponent<TilemapRenderer>();
        //firstWallsRenderer = firstWallsRenderer.GetComponent<TilemapRenderer>();
        //firstStairsBackgroundRenderer;
        //secondBackgroundRenderer;

        currentFloor = 1;


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
            if (yardDoor1Open.activeSelf)
            {
                yardDoor1Open.SetActive(false);
                yardDoor1Close.SetActive(true);
     
            }
            else
            {
                yardDoor1Open.SetActive(true);
                yardDoor1Close.SetActive(false);
            }
        }

        if ((context.gameObject.name == "Yard Door (2)") && (Input.GetKeyDown(KeyCode.F)))
        {
            if (yardDoor2Open.activeSelf)
            {
                yardDoor2Open.SetActive(false);
                yardDoor2Close.SetActive(true);

            }
            else
            {
                yardDoor2Open.SetActive(true);
                yardDoor2Close.SetActive(false);
            }
        }

        if ((context.gameObject.name == "Security Door(1)") && (Input.GetKeyDown(KeyCode.F)))
        {
            if (securityDoor1Open.activeSelf)
            {
                securityDoor1Open.SetActive(false);
                securityDoor1Close.SetActive(true);

            }
            else
            {
                securityDoor1Open.SetActive(true);
                securityDoor1Close.SetActive(false);
            }
        }

        if ((context.gameObject.name == "Security Door (2)") && (Input.GetKeyDown(KeyCode.F)))
        {
            if (securityDoor2Open.activeSelf)
            {
                securityDoor2Open.SetActive(false);
                securityDoor2Close.SetActive(true);

            }
            else
            {
                securityDoor2Open.SetActive(true);
                securityDoor2Close.SetActive(false);
            }
        }

        if ((context.gameObject.name == "Closet") && (Input.GetKeyDown(KeyCode.F)))
        {
            if (closetDoorOpen.activeSelf)
            {
                closetDoorOpen.SetActive(false);
                closetDoorClose.SetActive(true);

            }
            else
            {
                closetDoorOpen.SetActive(true);
                closetDoorClose.SetActive(false);
            }
        }

        if ((context.gameObject.name == "Med Bay") && (Input.GetKeyDown(KeyCode.F)))
        {
            if (medDoorOpen.activeSelf)
            {
                medDoorOpen.SetActive(false);
                medDoorClose.SetActive(true);

            }
            else
            {
                medDoorOpen.SetActive(true);
                medDoorClose.SetActive(false);
            }
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
   
