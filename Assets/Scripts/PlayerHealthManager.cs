using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float damageCooldown;
    private float damageCooldownTimer;

    public float playerHealth;
    [SerializeField] private Image healthBar;
    public float playerStamina;
    [SerializeField] private Image staminaBar;
    [SerializeField] private AudioClip bloodHitSound;
    private AudioSource audioSource;

    private void Awake()
    {
        playerHealth = 100;
        playerStamina = 100;
        damageCooldown = 2f;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bloodHitSound;
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
    
    public void takeDamage(string weapon, float damageTaken)
    {
        if (damageCooldownTimer >=damageCooldown && (weapon == "sword"))
        {
            damageCooldownTimer = 0f;
            playerHealth -= damageTaken;
            healthBar.fillAmount = playerHealth/ 100f;
            AudioSource.PlayClipAtPoint(bloodHitSound, transform.position, 1f);
            print("Health Damaged");
        //issue: Tank doing stamina damage and not health damage
        }
        else if(damageCooldownTimer >= damageCooldown)
        {
            reduceStamina(33.334f);
        }
        
    }
    public void reduceStamina(float staminaTaken)
    {
        if (damageCooldownTimer >= damageCooldown)
        {
            damageCooldownTimer = 0f;
            playerStamina -= staminaTaken;
            staminaBar.fillAmount = playerStamina / 100f;
            AudioSource.PlayClipAtPoint(bloodHitSound, transform.position, 1f);
            print("Stamina Damaged");
        }
    }

    public void heal(float healingApplied)
    {
        playerHealth += healingApplied;
        healthBar.fillAmount = playerHealth / 100f;
    }

    public void regenStamina(float stamina)
    {
        playerStamina += stamina;
        staminaBar.fillAmount = playerStamina / 100f;
    }

    //private void OnCollisionEnter2D(Collision2D context)
    //{
    //    if (context.gameObject.tag == "Enemy")
    //    {
    //        takeDamage(20);
    //    }
    //}
}
