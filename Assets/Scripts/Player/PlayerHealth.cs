using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] TMP_Text healthText = null;
    [SerializeField] GameObject audio_Error = null;

    [SerializeField] int maxHealth = 3;
    int currentHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = $"Health: {currentHealth}";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
            collision.gameObject.GetComponent<EnemyTakeDamage>().TakeDamage(100000);
        }
        else if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        healthText.text = $"Health: {currentHealth}";

        if (currentHealth <= 0)
        {
            GameObject audio = Instantiate(audio_Error);
            DontDestroyOnLoad(audio);

            SceneManager.LoadScene("DeathScreen");
        }
    }
}
