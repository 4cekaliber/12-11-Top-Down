using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    NavMeshAgent agent;

    private GameObject statManager;
    private StatScript statManagerScript;
    private PlayerHealthManager playerHealthManagerScript;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float attackLength;
    private float attackTimer;

    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    [SerializeField] private Transform[] patrolPoints;
    private int patrolDestination;

    private float fovAngle = 90f;
    [SerializeField] private Transform flashlightTransform;
    private float range = 100f;
    private void Awake()
    {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        statManager = GameObject.Find("StatManager");
        statManagerScript = statManager.GetComponent<StatScript>();
        animator.SetBool("isRunning", true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackLength = 40f;
        attackTimer = 0f;
        patrolDestination = 0;
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
        Vector2 direction = target.position - transform.position;
        float angle = Vector3.Angle(direction, flashlightTransform.right);
        if (patrolDestination == 0)
        {
            angle = Vector3.Angle(direction, Quaternion.AngleAxis(180f, Vector3.forward) * flashlightTransform.right);
        }
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D rayHit = Physics2D.Raycast(flashlightTransform.position, direction, range, Physics.DefaultRaycastLayers);
        if (angle < (fovAngle / 2) && rayHit.collider)
        {
            if (rayHit.collider.CompareTag("Player"))
            {
                //print("Found!");
                attackTimer = 0f;
                animator.SetTrigger("attack");

            }
            else
            {
                //print("Nothing Seen");
            }
            print(rayHit.collider.name);
            Vector2 hitDirection = rayHit.collider.transform.position - transform.position;
            Debug.DrawRay(flashlightTransform.position, hitDirection, Color.green);
        }
    }

    private void FixedUpdate()
    {
        //if (target)
        //{
        //    rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        //}

        if (patrolDestination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.1)
            {
                patrolDestination = 1;
                transform.localScale = new Vector3(3, 3, 1);
            }
        }
        else if (patrolDestination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.1)
            {
                patrolDestination = 0;
                transform.localScale = new Vector3(-3, 3, 1);
            }
        }
        //agent.ResetPath();
        //agent.SetDestination(target.position);

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
       
        //if (context.gameObject.tag == "Player")
        //{
        //    //attackTimer = 0f;
        //    //animator.SetTrigger("attack");
        //    playerHealthManagerScript = GetComponent<PlayerHealthManager>();
        //    playerHealthManagerScript.takeDamage(20);

        //}
        if (context.gameObject.tag == "Bullet")
        {
            damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
            statManagerScript.addScore(100);
            Destroy(gameObject);
        }
    }
}
