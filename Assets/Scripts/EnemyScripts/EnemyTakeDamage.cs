using System.Collections;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    [SerializeField] int health = 1;
    [SerializeField] int rewardedScore = 1;

    int currentHealth = 0;

    private void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(int damageAmount = 1)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreSystem>().AddScore(rewardedScore);
        StartCoroutine(DieAnim());
    }

    IEnumerator DieAnim()
    {
        float timer = 0;

        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 targetScale = new(0, 0, 1);

        while (timer <= 0.1f)
        {
            float t = timer / 0.1f;
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            timer += Time.deltaTime;

            yield return null;
        }

        if (gameObject.GetComponent<FolderEnemy>() != null) gameObject.GetComponent<FolderEnemy>().SpawnEnemy();

        Destroy(gameObject);
    }
}
