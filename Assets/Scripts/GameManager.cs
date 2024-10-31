using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform[] SpawnTransform;
    public GameObject playerOneCamera;

    public List<PlayerController> Players;
    
    
    
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

    }

    public void SpawnPlayerOne()
    {
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        GameObject playerTankPrefab = Instantiate(playerTankPawnPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        PlayerController newPlayerController = newPlayerObj.GetComponent<PlayerController>();
        TankPawn newTankPawn = playerTankPrefab.GetComponent<TankPawn>();
        
    }
    
    #region InstatiatingPrefabs
    public GameObject playerControllerPrefab;
    public GameObject enemyControllerPrefab;
    public GameObject playerTankPawnPrefab;
    public GameObject enemyTankPawnPrefab;
    #endregion InstatiatingPrefabs
}
