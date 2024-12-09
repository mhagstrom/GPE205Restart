using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace AAA
{
    public class MapGenerator : MonoBehaviour
    {
        public static MapGenerator Instance;
        
        public GameObject[] gridPrefabs;
        public int rows;
        public int columns;
        public float RoomWidth = 50.0f;
        public float RoomHeight = 50.0f;
        private Room[,] mapGrid;

        private GameObject mapRoot;

        [SerializeField]
        private GameObject[] powerups;

        public void DestroyMap()
        {
            if (mapRoot == null) return;

            Destroy(mapRoot.gameObject);
        }

        public void Awake()
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

        public GameObject RandomRoomPrefab()
        {
            return gridPrefabs[Random.Range(0, gridPrefabs.Length)];
        }

        public void SetMapSize(float value)
        {
            //get float from UI MapSize Slider and convert to proportional int, assign to rows and columns, where 1.0f = 20 rows/columns and 0.0f = 2 rows/columns
            rows = Mathf.RoundToInt(value * 18) + 2;
            columns = Mathf.RoundToInt(value * 18) + 2;
        }
        
        public void GenerateMap()
        {
            mapRoot = new GameObject("MapRoot");
            mapRoot.transform.parent = transform;

            mapGrid = new Room[rows, columns];
            
            for (int currentRow = 0; currentRow < rows; currentRow++)
            {
                for (int currentCol = 0; currentCol < columns; currentCol++)
                {
                    float XPosition = RoomWidth * currentRow;
                    float ZPosition = RoomHeight * currentCol;
                    Vector3 newPosition = new Vector3(XPosition, 0, ZPosition);

                    GameObject TempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                    TempRoomObj.transform.parent = mapRoot.transform;

                    TempRoomObj.name = "Room_" + "(" + currentRow + "," + currentCol + ") ";

                    Room TempRoom = TempRoomObj.GetComponent<Room>();

                    mapGrid[currentCol, currentRow] = TempRoom;

                    if (currentCol == 0)
                    {
                        TempRoom.northDoor.SetActive(false);
                    }
                    else if (currentCol == columns - 1)
                    {
                        TempRoom.southDoor.SetActive(false);
                    }
                    else
                    {
                        TempRoom.northDoor.SetActive(false);
                        TempRoom.southDoor.SetActive(false);
                    }

                    if (currentRow == 0)
                    {
                        TempRoom.eastDoor.SetActive(false);
                    }
                    else if (currentRow == rows - 1)
                    {
                        TempRoom.westDoor.SetActive(false);

                    }
                    else
                    {
                        TempRoom.westDoor.SetActive(false);
                        TempRoom.eastDoor.SetActive(false);
                    }
                    SetupPowerups(TempRoom);
                }
            }

        }

        private void SetupPowerups(Room TempRoom)
        {
            var powerupSpawner = TempRoom.GetComponentInChildren<PowerupSpawner>();

            foreach (var spawn in powerupSpawner.spawns)
            {
                var doSpawn = Random.Range(0, 100) > 70;

                if (doSpawn)
                {
                    var randomPowerup = powerups[Random.Range(0, powerups.Length)];

                    if (randomPowerup)
                    {
                        var p = Instantiate(randomPowerup, spawn.position, Quaternion.identity);
                        p.transform.parent = spawn;
                    }
                }
            }
        }
    }
}