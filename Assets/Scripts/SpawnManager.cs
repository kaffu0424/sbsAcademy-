using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoint;

    public float timeBetweenWaves = 5;
    float countDown = 3;
    public int waveIndexMax;
    int waveIndex = 0;

    private void Update()
    {
        if(countDown<=0)
        {
            StartCoroutine(SpawnWave());
            countDown = timeBetweenWaves;
        }
        countDown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        if(waveIndex<waveIndexMax)
        {
            for (int i = 0; i < waveIndex; i++)
            {
                int randomResult = Random.Range(0, spawnPoint.Length);
                SpawnEnemy(randomResult);
                yield return new WaitForSeconds(0.5f);
            }
        }
        
    }

    void SpawnEnemy(int num)
    {
        Instantiate(enemyPrefab, spawnPoint[num].position, spawnPoint[num].rotation);
    }
   
}
