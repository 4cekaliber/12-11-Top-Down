using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float damageCooldown;
    private float damageCooldownTimer;

    private float playerHealth;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damageCooldownTimer += Time.deltaTime;
    }
    private void Awake()
    {
        playerHealth = 100;
        damageCooldown = 2f;
    }
    void takeDamage()
    {
        if (damageCooldownTimer >=damageCooldown)
        {
            damageCooldownTimer = 0f;
            playerHealth -= 20;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D context)
    {
        if (context.gameObject.tag == "Enemy")
        {
            takeDamage();
        }
    }
}
