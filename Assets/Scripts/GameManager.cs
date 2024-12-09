using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AAA;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsGameStarted { get; private set; }
    public bool IsPaused { get; private set; }

    public GameObject pauseMenu;
    public GameObject optionsMenu;

    public List<PlayerController> Players;
    public List<AIController> AIControllers;
    public List<TankPawn> PlayerPawns;
    public List<TankPawn> EnemyPawns;
    public List<TankPawn> AllPawns;
    public List<Transform> Waypoints;

    public List<PatrolPath> PatrolPaths;

    public Camera P1Camera => HumanPlayers == 1 ? soloCamera : p1Camera;

    public Camera soloCamera;

    public Camera p1Camera;
    public Camera p2Camera;
    public Camera UICamera;

    public int numTanks = 5;

    public int HumanPlayers { get; private set; } = 1;

    public bool debugMode;

    public GameObject SplitScreen;
    public GameObject SinglePlayer;

    List<PlayerController> DefeatedPlayers = new List<PlayerController>();
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseMenu(!IsPaused);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerPawns[0].GetComponent<Health>().OnDamageTaken(99);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayerPawns[1].GetComponent<Health>().OnDamageTaken(99);
        }
    }

    public void StartGame(bool coop)
    {
        if (IsGameStarted) return;

        HudHelper.Instance.SetCoop(coop);

        HumanPlayers = coop ? 2 : 1;
        IsGameStarted = true;
        MapGenerator.Instance.GenerateMap();
        SetPauseMenu(false);
        PatrolPaths = FindObjectsOfType<PatrolPath>().ToList();
        SpawnAll();
        
        PlayerPawns = GameObject.FindGameObjectsWithTag("Player").Select(go => go.GetComponent<TankPawn>()).ToList();
        EnemyPawns = GameObject.FindGameObjectsWithTag("Enemy").Select(go => go.GetComponent<TankPawn>()).ToList();
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").Select(go => go.GetComponent<Transform>()).ToList();

        AllPawns = new List<TankPawn>();
        AllPawns.AddRange(EnemyPawns);
        AllPawns.AddRange(PlayerPawns);

        if (PlayerPawns.Count < 2)
        {
            SplitScreen.SetActive(false);
            SinglePlayer.SetActive(true);
        }
        else
        {
            SplitScreen.SetActive(true);
            SinglePlayer.SetActive(false);
        }

        if(debugMode)
        {
            PlayerPawns[0].hullMoveSpeed = 50;
            PlayerPawns[0].hullRotateSpeed = 100;
            PlayerPawns[0].name = "Player";
        }

        Debug.Log("Player Controllers: " + Players.Count);
        Debug.Log("AI Controllers: " + AIControllers.Count);
        Debug.Log("Player Pawns: " + PlayerPawns.Count);
        Debug.Log("Enemy Pawns: " + EnemyPawns.Count);
        
        Debug.Log("Waypoints in GameManager: " + Waypoints.Count);
    }

    public void EndGame()
    {
        SetPauseMenu(true);
        MapGenerator.Instance.DestroyMap();
        IsGameStarted = false;
        foreach(var tank in PlayerPawns)
        {
            HudHelper.Instance.UnsetTank(tank);
        }
        
        for(int i = 0; i < AllPawns.Count; i++)
        {
            var pawn = AllPawns[i];

            Destroy(pawn.gameObject);
        }


        Players.Clear();
        AIControllers.Clear();
        PlayerPawns.Clear();
        EnemyPawns.Clear();
        AllPawns.Clear();
        Waypoints.Clear();
        PatrolPaths.Clear();
        DefeatedPlayers.Clear();
    }

    private void SpawnAll()
    {
        System.Random rng = new System.Random();
        Spawner[] allSpawns = FindObjectsOfType<Spawner>();

        //public void Shuffle<T> (T[] values);
        Shuffle(allSpawns);

        if (allSpawns.Length > 0)
        {
            int index = 0;
            for (int i = 0; i < HumanPlayers; i++)
            {
                var c = allSpawns[i].SpawnPlayer(index);
                c.PlayerNumber = index;
                {
                    HudHelper.Instance.SetTank(c);

                    c.name = "Player";

                    index++;
                }
            }
            // Assign AIControllers to the next 4 Spawners
            for (int i = index; i < allSpawns.Length && i < numTanks + HumanPlayers; i++)
            {
                var controller = allSpawns[i].SpawnEnemy();
                controller.name = controller.GetComponent<AIController>().enemyType.name;
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
    
    //method for opening options menu
    public void ToggleOptionsMenu()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }
    
    //method for opening pause menu
    public void SetPauseMenu(bool val)
    {
        if (!IsGameStarted) return;
        pauseMenu.SetActive(val);
        IsPaused = val;
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

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnEnemyDeath(TankPawn pawn)
    {
        if (AllPawns.Contains(pawn))
            AllPawns.Remove(pawn);

        if (EnemyPawns.Contains(pawn))
            EnemyPawns.Remove(pawn);

        Destroy(pawn.gameObject);
        
        if(EnemyPawns.Count == 0)
        {
            HudHelper.Instance.ShowWinScreen();
        }
    }

    internal void Respawn(Pawn pawn)
    {
        Spawner[] allSpawns = FindObjectsOfType<Spawner>();

        var random = UnityEngine.Random.Range(0, allSpawns.Length);
        var respawn = allSpawns[random].transform;

        pawn.GetComponent<Rigidbody>().MovePosition(respawn.position);
        pawn.transform.rotation = (respawn.rotation);
    }

    
    internal void GameOver(PlayerController controller)
    {
        if(!DefeatedPlayers.Contains(controller))
            DefeatedPlayers.Add(controller);
      
        if(DefeatedPlayers.Count == HumanPlayers)
        {
            HudHelper.Instance.ShowGameOver();
        }
    }
}
