using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<PlayerController> Players;
    public List<AIController> AIControllers;
    public List<TankPawn> PlayerPawns;
    public List<TankPawn> EnemyPawns;
    public List<Transform> Waypoints;
    
    public List<PatrolPath> PatrolPaths;
    
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
        
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        
        PatrolPaths = FindObjectsOfType<PatrolPath>().ToList();
        
        PlayerPawns = GameObject.FindGameObjectsWithTag("Player").Select(go => go.GetComponent<TankPawn>()).ToList();
        EnemyPawns = GameObject.FindGameObjectsWithTag("Enemy").Select(go => go.GetComponent<TankPawn>()).ToList();
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(go => go.GetComponent<Transform>()).ToList();
        
        SpawnAll();
        
        Debug.Log("Player Controllers: " + Players.Count);
        Debug.Log("AI Controllers: " + AIControllers.Count);
        Debug.Log("Player Pawns: " + PlayerPawns.Count);
        Debug.Log("Enemy Pawns: " + EnemyPawns.Count);
        
        Debug.Log("Waypoints in GameManager: " + Waypoints.Count);
    }

    private void SpawnAll()
    {
        System.Random rng = new System.Random();
        Spawner[] allSpawns = FindObjectsOfType<Spawner>();
        
        //public void Shuffle<T> (T[] values);
        Shuffle(allSpawns);
        
        if (allSpawns.Length > 0)
        {
            // Assign PlayerController to the first Spawner
            allSpawns[0].Spawn(ControllerType.Player);

            // Assign AIControllers to the next 4 Spawners
            for (int i = 1; i < allSpawns.Length && i < 5; i++)
            {
                allSpawns[i].Spawn(ControllerType.AI);
            }
        }
       
    }
    
    public void Shuffle(Spawner[] allSpawns)
    {
        int n = allSpawns.Length;
        while (n > 1)
        {
            int k = UnityEngine.Random.Range(0, n--);
            Spawner temp = allSpawns[n];
            allSpawns[n] = allSpawns[k];
            allSpawns[k] = temp;
        }
    }
    
    
    //Vector3 position in the argument is provided by the call of this function, the position of the tank checking for the closest patrol path
    public PatrolPath GetClosestPatrolPath(Vector3 position)
    {
        Dictionary<float, PatrolPath> pathDistances = new Dictionary<float, PatrolPath>();
        foreach (var patrolPath in PatrolPaths)
        {
            Transform nearestWaypoint = patrolPath.GetNearestWaypoint(position);
            //float distance = Vector3.Distance(nearestWaypoint.position, position);
            //PathDistances.Add(distance);
            pathDistances.Add(Vector3.Distance(nearestWaypoint.position, position), patrolPath);
        }

        float minDistance = float.MaxValue;
        foreach (var key in pathDistances.Keys)
        {
            if (key < minDistance)
            {
                minDistance = key;
            }
        }
        if (minDistance == float.MaxValue)
        {
            return null;
        }
        else
        {
            return pathDistances[minDistance];
        }
    }
    
    #region InstatiatingPrefabs
    public GameObject playerControllerPrefab;
    public GameObject enemyControllerPrefab;
    public GameObject playerTankPawnPrefab;
    public GameObject enemyTankPawnPrefab;
    #endregion InstatiatingPrefabs
}
