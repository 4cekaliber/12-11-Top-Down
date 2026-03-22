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

   void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = firingSound;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

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
        
        if(Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            fire();
        }
    }

    void fire()
    {
         canFire = false;
         AudioSource.PlayClipAtPoint(firingSound,transform.position,1f);
         Instantiate(bullet, bulletTransform.position, Quaternion.identity);
   
    }
}
