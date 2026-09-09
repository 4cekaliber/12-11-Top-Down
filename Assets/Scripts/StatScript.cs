
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.CompilerServices;

public class StatScript : MonoBehaviour
{
    private float healthAmount;
    [SerializeField]private Image healthBar;

    private float playerScore;
    [SerializeField] TMP_Text scoreText;

    private float damageDelay;
    private float damageDelayTimer;

    private bool hasKey1;
    private GameObject Key1;
    private GameObject Key1Image;
    private GameObject ShotgunImage;
    public bool hasRevolver;
    public bool usingRevolver;
    public bool hasShotgun;
    public bool usingShotgun;
    [SerializeField] private Sprite revolverSprite;
    [SerializeField] private Sprite ShotgunSprite;

    private GameObject bulletTransform;
    private GameObject inventory;

    public int securityLevel;
    [SerializeField] private Transform[] spawnPoints;
    public float spawnTimer;
    private float spawnCooldown;
    private float spawnIntervalTimer;
    private float spawnIntervalCooldown;
    [SerializeField] private GameObject enemyPrefab;
    private GameObject enemyInstance;

    //0 means in cells, 1, means yard, and 2 means cafeteria
    public int activityNumber;
    public float activityTimer;
    public float activityDuration;

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
    void Start()
    {
        
    }


    private void Awake()
    {
        healthAmount = 100f;
        healthBar.fillAmount = healthAmount / 100f;
        damageDelay = 1f;
        damageDelayTimer = 0f;
        hasKey1 = false;
        hasRevolver = true;
        usingRevolver = true;
        hasShotgun = false;
        usingShotgun = false;

        Key1 = GameObject.Find("Key");
        ShotgunImage = GameObject.Find("Shotgun Image");
        ShotgunImage.SetActive(false);
        Key1Image = GameObject.Find("Key Image");
        Key1Image.SetActive(false);

        inventory = GameObject.Find("Inventory");
        inventory.SetActive(false);

        bulletTransform = GameObject.Find("BulletTransform");

        spawnTimer = 0f;
        spawnCooldown = 5f;
        spawnIntervalTimer = 0f;
        spawnIntervalCooldown = .5f;

        activityNumber = 0;
        activityTimer = 0f;
        activityDuration = 60f;

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
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(10);
            addScore(100);
        }

        */

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switchItem(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchItem(2);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivation();
        }

        damageDelayTimer++;

        
        if (securityLevel == 1 && spawnTimer < spawnCooldown && spawnIntervalTimer > spawnIntervalCooldown)
        {
            spawnWave();

        }
        else
        {
            //print("Securitylvl: " + securityLevel);
            //print("Spawntimer: " + spawnTimer);
            //print("spawnintervalTimer: " + spawnIntervalTimer);

        }
        if (securityLevel > 0)
        {
            spawnTimer += Time.deltaTime;
        }
        
        spawnIntervalTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        activityTimer += Time.deltaTime;
        if (activityTimer >= activityDuration)
        {
            nextActivity();
        }
    }
    //public void takeDamage(float damageTaken)
    //{
    //    if (damageDelayTimer >= damageDelay)
    //    {
    //        healthAmount -= damageTaken;
    //        healthBar.fillAmount = healthAmount / 100f;
    //        damageDelayTimer = 0f;
    //        //Debug.Log(damageDelayTimer);
    //    }

    //}



    public void addScore(float addAmount)
    {
        playerScore += addAmount;
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void minusScore(float minusAmount)
    {
        playerScore -= minusAmount;
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void grabbed(string itemName)
    {
        if (itemName == "Key1")
        {
            hasKey1 = true;
        }else if (itemName == "Shotgun")
        {
            hasShotgun = true;
        }
    }

    public bool getItemState(string itemName)
    {
        if (itemName == "Key1")
        {
            return hasKey1;
        }
        else if(itemName == "Shotgun")
        {
            return hasShotgun;
        }
        else
        {
            return false;
        }
    }

    public void switchItem(int number)
    {
        if (bulletTransform == null)
        {
            Debug.Log("bulletTransform not got");
        }
        if (number == 1 && !usingRevolver)
        {
            Debug.Log("switching to weapon 1");
            usingRevolver = true;
            usingShotgun = false;
            bulletTransform.GetComponent<SpriteRenderer>().sprite = revolverSprite;
        }else if (number == 2 && !usingShotgun)
        {
            Debug.Log("switching to weapon 2");
            usingShotgun = true;
            usingRevolver = false;
            bulletTransform.GetComponent<SpriteRenderer>().sprite = ShotgunSprite;
        }
    }

    private void inventoryActivation()
    {
        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
            Key1Image.SetActive(false);
            ShotgunImage.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
            if (getItemState("Key1"))
            {
                Key1Image.SetActive(true);
            }

            if (getItemState("Shotgun"))
            {
                ShotgunImage.SetActive(true);
            }
        }


    }

    private void spawnWave()
    {

        //print("spawning wave");
        enemyInstance = Instantiate(enemyPrefab, spawnPoints[Random.Range(0,2)].position, Quaternion.identity);
        enemyInstance.GetComponent<Enemy>().patrolDestination = -1;
        spawnIntervalTimer = 0f;
    }

    private void nextActivity()
    {
        activityNumber = (activityNumber + 1) % 3;
    }
    //idea knockback gun

    public void interactDoor(GameObject doorObject)
    {
        GameObject doorClosed = doorObject.transform.Find("Close").gameObject;
        GameObject doorOpen = doorObject.transform.Find("Open").gameObject;
        if (doorOpen.activeSelf)
        {
            doorOpen.SetActive(false);
            doorClosed.SetActive(true);

        }
        else
        {
            doorOpen.SetActive(true);
            doorClosed.SetActive(false);
        }
    }
}




//if ((context.gameObject.name == "Yard Door (1)") && (Input.GetKeyDown(KeyCode.F)))
//{
//    if (yardDoor1Open.activeSelf)
//    {
//        yardDoor1Open.SetActive(false);
//        yardDoor1Close.SetActive(true);

//    }
//    else
//    {
//        yardDoor1Open.SetActive(true);
//        yardDoor1Close.SetActive(false);
//    }
//}

//if ((context.gameObject.name == "Yard Door (2)") && (Input.GetKeyDown(KeyCode.F)))
//{
//    if (yardDoor2Open.activeSelf)
//    {
//        yardDoor2Open.SetActive(false);
//        yardDoor2Close.SetActive(true);

//    }
//    else
//    {
//        yardDoor2Open.SetActive(true);
//        yardDoor2Close.SetActive(false);
//    }
//}

//if ((context.gameObject.name == "Security Door(1)") && (Input.GetKeyDown(KeyCode.F)))
//{
//    if (securityDoor1Open.activeSelf)
//    {
//        securityDoor1Open.SetActive(false);
//        securityDoor1Close.SetActive(true);

//    }
//    else
//    {
//        securityDoor1Open.SetActive(true);
//        securityDoor1Close.SetActive(false);
//    }
//}

//if ((context.gameObject.name == "Security Door (2)") && (Input.GetKeyDown(KeyCode.F)))
//{
//    if (securityDoor2Open.activeSelf)
//    {
//        securityDoor2Open.SetActive(false);
//        securityDoor2Close.SetActive(true);

//    }
//    else
//    {
//        securityDoor2Open.SetActive(true);
//        securityDoor2Close.SetActive(false);
//    }
//}

//if ((context.gameObject.name == "Closet") && (Input.GetKeyDown(KeyCode.F)))
//{
//    if (closetDoorOpen.activeSelf)
//    {
//        closetDoorOpen.SetActive(false);
//        closetDoorClose.SetActive(true);

//    }
//    else
//    {
//        closetDoorOpen.SetActive(true);
//        closetDoorClose.SetActive(false);
//    }
//}

//if ((context.gameObject.name == "Med Bay") && (Input.GetKeyDown(KeyCode.F)))
//{
//    if (medDoorOpen.activeSelf)
//    {
//        medDoorOpen.SetActive(false);
//        medDoorClose.SetActive(true);

//    }
//    else
//    {
//        medDoorOpen.SetActive(true);
//        medDoorClose.SetActive(false);
//    }
//}
