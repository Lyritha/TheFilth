using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FolderEnemy : MonoBehaviour
{
    [SerializeField] int MediumEnemiesBias = 70;
    [SerializeField] int HardEnemiesBias = 30;

    [SerializeField] GameObject[] MediumEnemies;
    [SerializeField] GameObject[] HardEnemies;

    [SerializeField] int minEnemies = 2;
    [SerializeField] int maxEnemies = 6;

    [SerializeField] float maxSpawnDiameter = 0.1f;

    [SerializeField, Tooltip("used to offset collider for nav bar")] float bottomOffset = 0f;
    [SerializeField, Tooltip("used to offset collider for nav bar")] float topOffset = 0f;
    [SerializeField, Tooltip("used to offset collider for nav bar")] float leftOffset = 0f;
    [SerializeField, Tooltip("used to offset collider for nav bar")] float rightOffset = 0f;


    int enemiesCount = 0;
    SpawnEnemies spawnEnemies = null;
    float minX, maxX, minY, maxY = 0;

    private void Start()
    {
        spawnEnemies = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnEnemies>();
        enemiesCount = Random.Range(minEnemies, maxEnemies + 1);

        Vector2 a = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        minX = a.x + leftOffset;
        minY = a.y + bottomOffset;

        Vector2 b = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        maxX = b.x + rightOffset;
        maxY = b.y + bottomOffset;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i <= enemiesCount; i++)
        {
            Vector2 randomOffsetSpawn = OffsetSpawn();

            GameObject instance = Instantiate(RandomEnemy(), randomOffsetSpawn, Quaternion.identity);

            spawnEnemies.Enemies.Add(instance);
        }

        gameObject.GetComponent<EnemyTakeDamage>().TakeDamage(1000000);
    }

    private Vector2 OffsetSpawn()
    {
        Vector2 offsetSpawn = Vector2.zero;
        bool validSpawn = false;
        while (!validSpawn)
        {
            offsetSpawn = transform.position;

            offsetSpawn.x = Random.Range(offsetSpawn.x - maxSpawnDiameter, offsetSpawn.x + maxSpawnDiameter);
            offsetSpawn.y = Random.Range(offsetSpawn.y - maxSpawnDiameter , offsetSpawn.y + maxSpawnDiameter);

            if ( offsetSpawn.x > minX && offsetSpawn.x < maxX && offsetSpawn.y > minY && offsetSpawn.y < maxY) validSpawn = true;
        }

        return offsetSpawn;
    }

    private GameObject RandomEnemy()
    {
        return BiasRandom() switch
        {
            0 => MediumEnemies[Random.Range(0, MediumEnemies.Length)],
            1 => HardEnemies[Random.Range(0, HardEnemies.Length)],
            _ => MediumEnemies[Random.Range(0, MediumEnemies.Length)],
        };
    }

    private int BiasRandom()
    {
        int biasRandomNum = Random.Range(0, MediumEnemiesBias + HardEnemiesBias);

        if (biasRandomNum < MediumEnemiesBias) return 0;
        else return 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSpawnDiameter * 2);
    }
}
