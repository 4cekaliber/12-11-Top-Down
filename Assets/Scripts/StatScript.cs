
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(10);
            addScore(100);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            heal(10);
            minusScore(100);
        }*/

        if (healthAmount <= 0)
        {
            SceneManager.LoadScene("Game Over");
        }

        damageDelayTimer++;
        //Debug.Log(damageDelayTimer);
    }

    private void Awake()
    {
        healthAmount = 100f;
        healthBar.fillAmount = healthAmount / 100f;
        damageDelay = 1f;
        damageDelayTimer = 0f;
        hasKey1 = false;
    }
    public void takeDamage(float damageTaken)
    {
        if (damageDelayTimer >= damageDelay)
        {
            healthAmount -= damageTaken;
            healthBar.fillAmount = healthAmount / 100f;
            damageDelayTimer = 0f;
            //Debug.Log(damageDelayTimer);
        }
        
    }

    public void heal(float healingApplied)
    {
        healthAmount += healingApplied;
        healthBar.fillAmount = healthAmount / 100f;
    }

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
        }
    }

    public bool getItemState(string itemName)
    {
        if (itemName == "Key1")
        {
            return hasKey1;
        }
        else
        {
            return false;
        }

        
    }
}
