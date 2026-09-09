using System.Runtime.CompilerServices;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private GameObject player;
    private Transform target;
    private bool chasing;
    private Vector2 moveDirection;
    NavMeshAgent agent;
    private Vector3 agentTarget;
    private Vector3 targetDistance;

    private GameObject statManager;
    private StatScript statManagerScript;
    private PlayerHealthManager playerHealthManagerScript;
    private PlayerMovement playerMovementScript;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject flashlightObject;
    private GameObject weaponObject;
    private GameObject headObject;
    private GameObject effectsObject;
    private float attackLength;
    private float attackTimer;

    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    [SerializeField] private Transform[] patrolPoints;
    public int patrolDestination;

    private float fovAngle = 90f;
    [SerializeField] private Transform flashlightTransform;
    private float range = 100f;

    [SerializeField] private AudioClip swordSound;
    private AudioSource audioSource;

    private Vector3 contactVector;
    Quaternion bloodRotation;

    private int currentFloor;
    private void Awake()
    {
        player = GameObject.Find("Player");
        target = player.transform;
        agentTarget = target.position;
        chasing = true;
        rb = GetComponent<Rigidbody2D>();
        statManager = GameObject.Find("StatManager");
        statManagerScript = statManager.GetComponent<StatScript>();
        //then implement system to change activities by getting activitynum from statscript, the script the guard able to open red doors
        playerHealthManagerScript = player.GetComponent<PlayerHealthManager>();
        playerMovementScript = player.GetComponent<PlayerMovement>();
        animator.SetBool("isWalking", true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashlightObject = transform.Find("Flashlight").gameObject;
        //weaponObject = transform.Find();
        headObject = transform.Find("Head").gameObject; 
        effectsObject = transform.Find("Effects").gameObject;
        attackLength = 40f;
        attackTimer = 0f;
        patrolDestination = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = swordSound;
        currentFloor = 1;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (target)
        //{
        //    Vector3 direction2 = (target.position - transform.position).normalized;
        //    moveDirection = direction2;

        //}

        //if (moveDirection.x < 0)
        //{
        //    spriteRenderer.flipX = true;
        //}
        //else
        //{
        //    spriteRenderer.flipX = false;
        //}

        /*if (animator.GetBool("isAttacking") == true)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackLength)
            {
                animator.SetBool("isAttacking", false);
            }
        }*/
        //Debug.Log(animator.GetBool("isRunning"));
        
    }

    private void FixedUpdate()
    {
        //if (target)
        //{
        //    rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        //}

        if (currentFloor == playerMovementScript.currentFloor)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.enabled = true;
            animator.enabled = true;
            flashlightObject.SetActive(true);
            headObject.SetActive(true);
            effectsObject.SetActive(true);
            rb.simulated = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            animator.enabled = false;
            flashlightObject.SetActive(false);
            headObject.SetActive(false);
            effectsObject.SetActive(false);
        }
            Vector2 direction = target.position - transform.position;
        float angle = Vector3.Angle(direction, flashlightTransform.right);
        if (targetDistance.x > 0)
        {
            
            angle = Vector3.Angle(direction, Quaternion.AngleAxis(180f, Vector3.forward) * flashlightTransform.right);
        }
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D rayHit = Physics2D.Raycast(flashlightTransform.position, direction, range, Physics.DefaultRaycastLayers);
        if (angle < (fovAngle / 2) && rayHit.collider)
        {
            if (rayHit.collider.CompareTag("Player") && (currentFloor == playerMovementScript.currentFloor))
            {
                //print("Found!");
                if (patrolDestination != -1)
                {
                    statManagerScript.spawnTimer = 0f;
                    patrolDestination = -1;
                    statManagerScript.securityLevel = 1;
                }
                attackTimer = 0f;
                animator.SetTrigger("attack");
                
            }
            else
            {
                //print("Nothing Seen");
            }
            //print(rayHit.collider.name);
            Vector2 hitDirection = rayHit.collider.transform.position - transform.position;
            Debug.DrawRay(flashlightTransform.position, hitDirection, Color.green);
        }

        if (patrolDestination == -1)
        {
            agentTarget = target.position;
        }
        else if (patrolDestination == 0)
        {
            agentTarget = patrolPoints[0].position;
            if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.1)
            {
                patrolDestination = 1;

            }
        }
        else if (patrolDestination == 1)
        {
            agentTarget = patrolPoints[1].position;
            if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.1)
            {
                patrolDestination = 0;
            }
        }

        //if (statManagerScript.activityNumber == 0)//cells
        //{
        //    agentTarget = patrolPoints[3].position;
        //    if (Vector2.Distance(transform.position, patrolPoints[3].position) < 0.1)
        //    {
        //        agentTarget = patrolPoints[2].position;
        //    }

        //    if (Vector2.Distance(transform.position, patrolPoints[2].position) < 0.1)
        //    {
        //        agentTarget = patrolPoints[0].position;
        //    }
        //} else if (statManagerScript.activityNumber == 1)//yard
        //{
        //    agentTarget = patrolPoints[3].position;
        //}else if (statManagerScript.activityNumber == 2)//caf
        //{
        //    agentTarget = patrolPoints[2].position;
        //    if (Vector2.Distance(transform.position, patrolPoints[2].position) < 0.1)
        //    {
        //        agentTarget = patrolPoints[3].position;
        //    }

        //    if (Vector2.Distance(transform.position, patrolPoints[2].position) < 0.1)
        //    {
        //        agentTarget = patrolPoints[0].position;
        //    }
        //}


        targetDistance = transform.position - agentTarget;
        if (targetDistance.x < 0)
        {
            transform.localScale = new Vector3(3, 3, 1);
        }
        else
        {
            transform.localScale = new Vector3(-3, 3, 1);
        }
        agent.SetDestination(agentTarget);
    }

    private void OnCollisionEnter2D(Collision2D context)
    {
        //if (context.gameObject.tag == "Bullet")
        //{
        //    damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
        //    statManagerScript.addScore(100);
        //    Destroy(gameObject);
        //}

       
    }

    private void OnTriggerEnter2D(Collider2D context)
    {
       
        if (context.gameObject.tag == "Player")
        {
            //attackTimer = 0f;
            //animator.SetTrigger("attack");
            
            if (this.tag == "Tank Enemy")
            {
                playerHealthManagerScript.takeDamage("sword", 20);
            }
            else
            {
                playerHealthManagerScript.takeDamage("fists", 20);
            }

        }
        if (context.gameObject.tag == "Bullet")
        {
            //first get vector between bullet and enemy
            contactVector = context.gameObject.transform.position - transform.position;
            //next, find the angle between 0 degrees and the new vector 
            float bulletAngle = Vector3.Angle( transform.right,contactVector);
            //lastly add or subtract 180 degrees to that vector to show angle where to shoot particles from
            //bloodRotation = Quaternion.LookRotation(gameObject.transform.forward, gameObject.transform.up);
            //Quaternion.AngleAxis(180, Vector3.forward) ;
            damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.LookRotation(Quaternion.AngleAxis(-90, Vector3.forward) * context.gameObject.transform.forward , Quaternion.AngleAxis(-90, Vector3.forward) * context.gameObject.transform.up));
            statManagerScript.addScore(100);
            Destroy(gameObject);
        }

        if (context.gameObject.name == "Main Building (1)")
        {
            currentFloor = 2;
            spriteRenderer.enabled = false;
            animator.enabled = false;
            flashlightObject.SetActive(false);
            headObject.SetActive(false);
            effectsObject.SetActive(false);
            rb.simulated = false;
        }
        if (context.gameObject.name == "Main Building (2.5)")
        {
            currentFloor = 1;
            spriteRenderer.enabled = false;
            animator.enabled = false;
            flashlightObject.SetActive(false);
            headObject.SetActive(false);
            effectsObject.SetActive(false);
            rb.simulated = false;
        }
    }

}
