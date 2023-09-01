using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float timeBetweenWaves = 5;
    float countDown = 3;
    int waveIndexMax;
    int waveIndex = 0;

    MapGenerator map;

    private void Start()
    {
        map = FindObjectOfType<MapGenerator>();
    }

    private void Update()
    {
        if(countDown<=0)
        {
            if(!GameObject.FindGameObjectWithTag("Enemy"))
            {
                StartCoroutine(SpawnWave());
                countDown = timeBetweenWaves;
            }
            
        }
        countDown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        waveIndexMax = map.maps[map.mapIndex].waveMax;
        enemyPrefab = map.maps[map.mapIndex].enemy;

        if (waveIndex<waveIndexMax)
        {
            for (int i = 0; i < waveIndex; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            StartCoroutine(WaitAndClear());        
        }
        
    }

    IEnumerator WaitAndClear()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.StageClear();
    }

    void SpawnEnemy()
    {
        Transform spawnTile = map.GetRandomOpenTile();          
        Instantiate(enemyPrefab, spawnTile.position, spawnTile.rotation);
        enemyPrefab.GetComponent<Enemy>().lv = map.mapIndex+1;  //맵(스테이지) 번호에 따라 적의 레벨이 변화하도록 설정
    }

    public void Reset()
    {
        waveIndex = 0;
    }

}
