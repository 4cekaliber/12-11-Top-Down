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

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float attackLength;
    private float attackTimer;

    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;
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
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;

            //float angle = Mathf.Atan2(direction.y,direction.x)* Mathf.Rad2Deg;
            //rb.rotation = angle;
        }

        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

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
        //    rb.linearVelocity = new Vector2(moveDirection.x,moveDirection.y) * moveSpeed;
        //}
        agent.ResetPath();
        agent.SetDestination(target.position);

    }

    private void OnCollisionEnter2D(Collision2D context)
    {
        if (context.gameObject.tag == "Bullet")
        {
            damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
            statManagerScript.addScore(100);
            Destroy(gameObject);
        }

       
    }

    private void OnTriggerStay2D(Collider2D context)
    {
       
        if (context.gameObject.tag == "Player")
        {
            attackTimer = 0f;
            animator.SetTrigger("attack");

        }
    }
}
