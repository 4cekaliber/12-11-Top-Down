using UnityEngine;
using UnityEngine.AI;

public class ProjectileEnemyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Transform target;
    private bool chasing;
    private Vector2 moveDirection;
    NavMeshAgent agent;
    private Vector3 agentTarget;
    private Vector3 targetDistance;

    private GameObject statManager;
    private StatScript statManagerScript;
    private PlayerHealthManager playerHealthManagerScript;

    //[SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float attackLength;
    private float attackTimer;

    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    [SerializeField] private Transform[] patrolPoints;
    public int patrolDestination;

    private float fovAngle = 90f;
    [SerializeField] private Transform flashlightTransform;
    private float range = 100f;

    private AudioSource audioSource;

    private Vector3 contactVector;
    Quaternion bloodRotation;

    private float shootTimer;
    private  float shootCharge;
    private float reloadingTimer;
    private float reloadingCooldown;
    [SerializeField] private GameObject bulletPrefab;
    private GameObject newBullet;
    [SerializeField] float bulletForce;
    [SerializeField] private AudioClip revolverFiringSound;
    private void Awake()
    {
        target = GameObject.Find("Player").transform;
        agentTarget = target.position;
        chasing = true;
        rb = GetComponent<Rigidbody2D>();
        statManager = GameObject.Find("StatManager");
        statManagerScript = statManager.GetComponent<StatScript>();
        //animator.SetBool("isWalking", true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackLength = 40f;
        attackTimer = 0f;
        patrolDestination = 0;
        audioSource = GetComponent<AudioSource>();
        shootTimer = reloadingTimer = 0f;
        shootCharge = 1.5f;
        reloadingCooldown = 3f;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    

    private void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Vector3.Angle(direction, flashlightTransform.right);
        if (targetDistance.x > 0)
        {

            angle = Vector3.Angle(direction, Quaternion.AngleAxis(180f, Vector3.forward) * flashlightTransform.right);
        }
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D rayHit = Physics2D.Raycast(flashlightTransform.position, direction, range, Physics.DefaultRaycastLayers);
        reloadingTimer += Time.deltaTime;
        if (angle < (fovAngle / 2) && rayHit.collider)
        {
            if (rayHit.collider.CompareTag("Player"))
            {
                print("Found!");
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootCharge && reloadingTimer >= reloadingCooldown)
                {
                    spriteRenderer.color = Color.red;
                    fireSingle(direction);
                    reloadingTimer = 0f;
                    if (shootTimer < shootCharge)
                    {
                        print("low charge");
                    }
                    if (reloadingTimer < reloadingCooldown)
                    {
                        print("low reload");
                    }
                }
                if (patrolDestination != -1)
                {
                    //statManagerScript.spawnTimer = 0f;
                    //patrolDestination = -1;
                    //statManagerScript.securityLevel = 1;
                }
                //attackTimer = 0f;
                //animator.SetTrigger("attack");

            }
            else
            {
                shootTimer = 0f;
                //print("Nothing Seen");
            }
            //print(rayHit.collider.name);
            Vector2 hitDirection = rayHit.collider.transform.position - transform.position;
            Debug.DrawRay(flashlightTransform.position, hitDirection, Color.green);
        }
        //agent.ResetPath();
        if (patrolDestination == -1)
        {
            agentTarget = transform.position;
        }
        else if (patrolDestination == 0)
        {
            //transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
            agentTarget = patrolPoints[0].position;
            if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.1)
            {
                patrolDestination = 1;

            }
        }
        else if (patrolDestination == 1)
        {
            //transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
            agentTarget = patrolPoints[1].position;
            if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.1)
            {
                patrolDestination = 0;
            }
        }

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
            playerHealthManagerScript = context.gameObject.GetComponent<PlayerHealthManager>();
            playerHealthManagerScript.takeDamage("bullet",20);

        }
        if (context.gameObject.tag == "Bullet")
        {
            //first get vector between bullet and enemy
            contactVector = context.gameObject.transform.position - transform.position;
            //next, find the angle between 0 degrees and the new vector 
            float bulletAngle = Vector3.Angle(transform.right, contactVector);
            //lastly add or subtract 180 degrees to that vector to show angle where to shoot particles from
            //bloodRotation = Quaternion.LookRotation(gameObject.transform.forward, gameObject.transform.up);
            //Quaternion.AngleAxis(180, Vector3.forward) ;
            damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.LookRotation(Quaternion.AngleAxis(-90, Vector3.forward) * context.gameObject.transform.forward, Quaternion.AngleAxis(-90, Vector3.forward) * context.gameObject.transform.up));
            statManagerScript.addScore(100);
            Destroy(gameObject);
        }
    }

    void fireSingle(Vector2 direction)
    {
        AudioSource.PlayClipAtPoint(revolverFiringSound, transform.position, 1f);
        newBullet = Instantiate(bulletPrefab, flashlightTransform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * newBullet.GetComponent<BulletScript>().force;
        //Debug.Log("Roation : " + rotation);

        //magic number 2 in above line is same as force in unity editor serialized filed for bulletScript
    }
}
