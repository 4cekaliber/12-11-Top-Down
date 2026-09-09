
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

}
