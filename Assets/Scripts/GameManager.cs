using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<PlayerController> Players;
    
    public Camera mainCamera;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    private void Start()
    {
        SpawnAll();
    }

    private void SpawnAll()
    {
        foreach (var spawner in Component.FindObjectsOfType<Spawner>())
        {
            spawner.Spawn();
        }
    }
    
    #region InstatiatingPrefabs
    public GameObject playerControllerPrefab;
    public GameObject enemyControllerPrefab;
    public GameObject playerTankPawnPrefab;
    public GameObject enemyTankPawnPrefab;
    #endregion InstatiatingPrefabs
}
