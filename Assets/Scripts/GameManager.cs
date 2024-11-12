using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject playerOneCamera;

    public List<PlayerController> Players;
    //first spawn point in list is for player one
    public List<Transform> spawnPoints;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        SpawnPlayerOne();
        SpawnEnemies();

    }

    public void SpawnPlayerOne()
    {
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        GameObject playerTankPrefab = Instantiate(playerTankPawnPrefab, spawnPoints[0].position, spawnPoints[0].rotation) as GameObject;
        PlayerController newPlayerController = newPlayerObj.GetComponent<PlayerController>();
        TankPawn newTankPawn = playerTankPrefab.GetComponent<TankPawn>();
        newPlayerController.TakeControl(newTankPawn);
    }

    private void SpawnEnemies()
    {
        //first spawn point is for player one
        foreach (var spawnPoint in spawnPoints.Skip(1))
        {
            GameObject enemyTankPrefab = Instantiate(enemyTankPawnPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        
    }
    
    #region InstatiatingPrefabs
    public GameObject playerControllerPrefab;
    public GameObject enemyControllerPrefab;
    public GameObject playerTankPawnPrefab;
    public GameObject enemyTankPawnPrefab;
    #endregion InstatiatingPrefabs
}
