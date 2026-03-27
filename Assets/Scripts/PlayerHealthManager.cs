using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float damageCooldown;
    private float damageCooldownTimer;

    private float playerHealth;
    [SerializeField] private Image healthBar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damageCooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.H))
        {
            heal(10);
        }

        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("Game Over");
        }
    }
    private void Awake()
    {
        playerHealth = 100;
        damageCooldown = 2f;
    }
    public void takeDamage(float damageTaken)
    {
        if (damageCooldownTimer >=damageCooldown)
        {
            damageCooldownTimer = 0f;
            playerHealth -= damageTaken;
            healthBar.fillAmount = playerHealth / 100f;
        }
        
    }

    public void heal(float healingApplied)
    {
        playerHealth += healingApplied;
        healthBar.fillAmount = playerHealth / 100f;
    }

    private void OnCollisionEnter2D(Collision2D context)
    {
        if (context.gameObject.tag == "Enemy")
        {
            takeDamage(20);
        }
    }
}
