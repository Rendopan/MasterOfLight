using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.VolumeComponent;

public class EnemiesWavesManager : MonoBehaviour
{
    public static EnemiesWavesManager enemiesWave;

    public List<GameObject> enemiesType;
    public List<Transform> targetDestPoints;


    private List<EnemyController> enemiesInWave;
    private float waveTimer = 20f;
    private float waveSpawnTime = 20f;
    private int enemiesInWaveNumber;
    private int currentWaveEnemies = 0;

    private int currentWave;
    //private bool isWaveInProgress;

    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        enemiesWave = this;
    }


    // Update is called once per frame
    void Update()
    {
        if (currentWaveEnemies < enemiesInWaveNumber)
        {
            SpawnEnemyInWave();
        }
    }

    public void StartWave(int waveNumeber)
    {
        switch (waveNumeber)
        {
            case 1:
                CurrentWave++;
                currentWaveEnemies = 0;
                enemiesInWaveNumber = 4;
                enemiesInWave = new List<EnemyController>();
                break;
            default:
                break;
        }
        
    }

    public void SpawnEnemyInWave()
    {
        waveTimer += Time.deltaTime;
        if (waveTimer >= waveSpawnTime)
        {
            GameObject enemyObj = Instantiate(enemiesType[0], this.transform);
            enemyController = enemyObj.GetComponent<EnemyController>();
            enemyController.StartTarget(targetDestPoints[0].gameObject);

            enemiesInWave.Add(enemyController);

            foreach (Transform t in targetDestPoints)
            {
                enemyController.ChooseNewTarget(t);
            }
            currentWaveEnemies++;
            waveTimer -= waveSpawnTime;
        }
    }

    public void RemoveEnemyInWave(EnemyController enemy)
    {
        for (int idEnemy = 0; idEnemy < enemiesInWave.Count; idEnemy++)
        {
            if (enemiesInWave[idEnemy].Equals(enemy))
            {
                enemiesInWave.RemoveAt(idEnemy);
                idEnemy = enemiesInWave.Count;
            }
        }
    }

    public void AddTargetPoint(Transform targetObject)
    {
        targetDestPoints.Add(targetObject);

        for (int idEnemy = 0; idEnemy < enemiesInWave.Count; idEnemy++)
        {
            for (int idDest = 0; idDest < targetDestPoints.Count; idDest++)
            {
                enemiesInWave[idEnemy].ChooseNewTarget(targetDestPoints[idDest]);
            }
        }

    }

    public void ChooseNewTarget()
    {
        for (int idEnemy = 0; idEnemy < enemiesInWave.Count; idEnemy++)
        {
            enemiesInWave[idEnemy].StartTarget(targetDestPoints[0].gameObject);
            for (int idDest = 0; idDest < targetDestPoints.Count; idDest++)
            {
                enemiesInWave[idEnemy].ChooseNewTarget(targetDestPoints[idDest]);
            }
        }
    }

    public void RemoveEnemiesTarget(Transform targetToRemove)
    {
        for (int idDest = 0; idDest < targetDestPoints.Count; idDest++)
        {
            if (targetDestPoints[idDest].Equals(targetToRemove))
            {
                targetDestPoints.RemoveAt(idDest);
                idDest = targetDestPoints.Count;
            }
        }
        ChooseNewTarget();
    }

    public int CurrentWave
    {
        get { return currentWave; }
        set { currentWave = value; } 
    }

}
