using System.Runtime.CompilerServices;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera mainCam;
    private Vector3 mousePos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;
    private bool canFire;
    private float timer;
    [SerializeField] private float timeBetweenFiring;
    [SerializeField] private AudioClip firingSound;
    private AudioSource audioSource;
    private StatScript statManagerScript;
    private GameObject newBullet;
    private GameObject newPelletOne;
    private GameObject newPelletTwo;
    private GameObject newPelletThree;
    private Vector3 rotation;
    private Vector3 rotation1;
    private Vector3 rotation2;

    private float fovAngle = 90f;
    [SerializeField] private Transform flashlightTransform;
    private float range = 0.2f;
    [SerializeField] private Transform target;

    void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = firingSound;
        statManagerScript = GameObject.Find("StatManager").GetComponent<StatScript>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        rotation = mousePos - transform.position;
        //makes another vector that is 20 degrees more than rotation along the z axis(Vector3.forward)
        rotation1 = Quaternion.AngleAxis(20f, Vector3.forward) * rotation;
        rotation2 = Quaternion.AngleAxis(-20f, Vector3.forward) * rotation;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        if (canFire == false)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && canFire && statManagerScript.usingRevolver)
        {
            fireSingle();
        }else if (Input.GetKeyDown(KeyCode.Space) && canFire && statManagerScript.usingShotgun)
        {
            fireSpread();
        }

        Vector2 direction = target.position - transform.position;
        float angle = Vector3.Angle(direction, flashlightTransform.right);
        RaycastHit2D rayHit = Physics2D.Raycast(flashlightTransform.position, direction, range);

        if ((angle < (fovAngle / 2)) && rayHit.collider.CompareTag("Player"))
        {
            print("Found!");
            
        }
        else
        {
            print("Nothing Seen");
        }
        Debug.DrawRay(flashlightTransform.position, direction* range, Color.red);
    }

    void fireSingle()
    {
         canFire = false;
         AudioSource.PlayClipAtPoint(firingSound,transform.position,1f);
         newBullet = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
         newBullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(rotation.x, rotation.y).normalized * newBullet.GetComponent<BulletScript>().force;
         //Debug.Log("Roation : " + rotation);

        //magic number 2 in above line is same as force in unity editor serialized filed for bulletScript
    }

    void fireSpread()
    {
        canFire = false;
        AudioSource.PlayClipAtPoint(firingSound, transform.position, 1f);
        newPelletOne = Instantiate(bullet, bulletTransform.position, Quaternion.identity);
        newPelletOne.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(rotation.x, rotation.y).normalized * newPelletOne.GetComponent<BulletScript>().force;
        newPelletTwo = Instantiate(bullet, bulletTransform.position, Quaternion.Euler(0,0,0));
        newPelletTwo.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(rotation1.x, rotation1.y).normalized * newPelletOne.GetComponent<BulletScript>().force;
        newPelletThree = Instantiate(bullet, bulletTransform.position, Quaternion.Euler(0, 0, 0));
        newPelletThree.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(rotation2.x, rotation2.y).normalized * newPelletOne.GetComponent<BulletScript>().force;
        //newPelletThree.transform.rotation = Quaternion.Euler(0, 0, 90);



    }
}
