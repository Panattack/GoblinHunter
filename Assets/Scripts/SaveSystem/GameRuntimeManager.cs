using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class GameRuntimeManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] List<GameObject> EnemyPrefabs;
    [SerializeField] GameObject Terrain;
    [SerializeField] bool ProceduralGen;
    Dictionary<string, GameObject> enemies;
    GameObject playerInstance;
    void OnEnable()
    {
        enemies = new Dictionary<string, GameObject>();
        if (!ProceduralGen) 
        {
            Spawn();
        }
  
    }


    private void SpawnPlayer()
    {
        Vector3 playerPos = new Vector3(PlayerProfile.gameData.currentArea.GetPlayerPosition()[0], PlayerProfile.gameData.currentArea.GetPlayerPosition()[1], PlayerProfile.gameData.currentArea.GetPlayerPosition()[2]);
        Vector3 playerRotation = new Vector3(PlayerProfile.gameData.currentArea.GetPlayerRotation()[0], PlayerProfile.gameData.currentArea.GetPlayerRotation()[1], PlayerProfile.gameData.currentArea.GetPlayerRotation()[2]);
        playerInstance = Instantiate(Player);
        GameObject.Find("Player").transform.position = playerPos;
        GameObject.Find("Camera Holder").transform.rotation.eulerAngles.Set(playerRotation.x, playerRotation.y, playerRotation.z);
    }

    private void SpawnEnemies()
    {
/*        if (ProceduralGen || Terrain.GetComponent<NavMeshSurface>() != null)
        {*/
/*            if (Terrain.GetComponent<NavMeshSurface>() != null) 
            {*/
                if (PlayerProfile.gameData.currentArea.GetEnemies() != null && PlayerProfile.gameData.currentArea.GetEnemies().Count != 0)
                {
                    foreach (EnemyData enemy in PlayerProfile.gameData.currentArea.GetEnemies())
                    {
                        GameObject tempEnemy = Instantiate(EnemyPrefabs[enemy.GetEnemyType()]);
                        tempEnemy.name = enemy.GetName();
                        Vector3 spawnPos = new Vector3(enemy.GetEnemyPosition()[0], enemy.GetEnemyPosition()[1], enemy.GetEnemyPosition()[2]);
                        tempEnemy.transform.position = spawnPos;
                        enemies.Add(tempEnemy.name, tempEnemy);
                        tempEnemy.GetComponent<NavMeshAgent>().Warp(spawnPos);
                    }
                }
/*            }*/
/*
        }*/

    }
    private void OnDisable()
    {
        DisableGameObjects();
    }

    public void DisableGameObjects()
    {
        Time.timeScale = 1f;
        Destroy(playerInstance);
        playerInstance = null;
        foreach (var entry in enemies)
        {
            Destroy(entry.Value);
        }
    }

    public void Spawn()
    {
        SpawnPlayer();
        SpawnEnemies();
    }
}

