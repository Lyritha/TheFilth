using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpawnPointGeneration))]
public class SpawnEnemies : MonoBehaviour
{
    [Header("Spawn amount")]
    [SerializeField] int minSpawnCount = 20;
    [SerializeField] int maxSpawnCount = 50;

    [Header("Spawn biases")]
    [SerializeField] int biasIncrease = 5;
    [SerializeField] int easyEnemiesBias = 100;
    [SerializeField] int MediumEnemiesBias = 5;
    [SerializeField] int HardEnemiesBias = 1;

    [Header("Objects to spawn")]
    [SerializeField] GameObject[] easyEnemies = null;
    [SerializeField] GameObject[] MediumEnemies = null;
    [SerializeField] GameObject[] HardEnemies = null;

    [Header("Scripts")]
    [SerializeField] SpawnPointGeneration spawnPointGeneration = null;
    [SerializeField] WavePopup WavePopup = null;

    private List<GameObject> enemies = new();
    private int round = 0;
    bool delayedStart = false;

    private void Start()
    {
        StartCoroutine(DelayedStartSpawns());
    }

    IEnumerator DelayedStartSpawns()
    {
        yield return new WaitForSeconds(3f);

        //spawns enemies
        SpawnEnemiesGroup();
        delayedStart = true;

        yield return null;
    }

    private void Update()
    {
        foreach (GameObject enemy in enemies.FindAll(enemy => enemy == null)) enemies.Remove(enemy);
        if (enemies.Count <= 0 && !WavePopup.IsEnabled && delayedStart) WavePopup.WavePause();
    }

    public void SpawnEnemiesGroup()
    {
        round++;
        UpdateBias();

        StartCoroutine(SpawnEnemiesSlowly());
    }

    IEnumerator SpawnEnemiesSlowly()
    {
        int amount = Random.Range(minSpawnCount, maxSpawnCount);

        foreach (Vector2 randomPos in RandomSpawnPoints(amount))
        {
            GameObject instance = Instantiate(RandomEnemy(), randomPos, Quaternion.identity);
            instance.transform.parent = transform;
            enemies.Add(instance);

            yield return new WaitForEndOfFrameUnit();
        }

        yield return null;
    }

    private void UpdateBias()
    {
        easyEnemiesBias += (int)(biasIncrease / 2);
        MediumEnemiesBias += biasIncrease;

        if (round % 2 == 0) HardEnemiesBias += biasIncrease;
    }

    private GameObject RandomEnemy()
    {
        return BiasRandom() switch
        {
            0 => easyEnemies[Random.Range(0, easyEnemies.Length)],
            1 => MediumEnemies[Random.Range(0, MediumEnemies.Length)],
            2 => HardEnemies[Random.Range(0, HardEnemies.Length)],
            _ => easyEnemies[Random.Range(0, easyEnemies.Length)],
        };
    }

    private int BiasRandom()
    {
        int biasRandomNum = Random.Range(0, easyEnemiesBias + MediumEnemiesBias + HardEnemiesBias);

        if (biasRandomNum < easyEnemiesBias) return 0;
        else if (biasRandomNum < easyEnemiesBias + MediumEnemiesBias) return 1;
        else return 2;
    }

    private List<Vector2> RandomSpawnPoints(int pAmount)
    {
        //get spawn points from other script
        Vector2[] spawnPoints = spawnPointGeneration.ValidSpawnPositions;

        //create list of the randomly chosen spawn points
        List<Vector2> chosenSpawnPoints = new();

        //fall back to filling all slots if there are more spawn attempts than spawn slots
        if (pAmount >= spawnPoints.Length)
        {
            chosenSpawnPoints.AddRange(spawnPoints);

            return chosenSpawnPoints;
        }
        else
        {
            //loop until the requisted amount of random spawn points have been acquired
            for (int amount = 0; amount < pAmount;)
            {
                int randomPos = Random.Range(0, spawnPoints.Length);

                if (!chosenSpawnPoints.Contains(spawnPoints[randomPos]))
                {
                    chosenSpawnPoints.Add(spawnPoints[randomPos]);
                    amount++;
                }
            }

            //returns value
            return chosenSpawnPoints;
        }
    }

    public List<GameObject> Enemies { get { return enemies; } set { enemies = value; } }
}
