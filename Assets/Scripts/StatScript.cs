
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

}
