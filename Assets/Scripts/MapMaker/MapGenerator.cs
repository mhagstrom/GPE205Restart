using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace AAA
{
    public class MapGenerator : MonoBehaviour
    {
        
        public GameObject[] gridPrefabs;
        public int rows;
        public int columns;
        public float RoomWidth = 50.0f;
        public float RoomHeight = 50.0f;
        private Room[,] mapGrid;

        // Start is called before the first frame update
        void Start()
        {
            GenerateMap();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject RandomRoomPrefab()
        {
            return gridPrefabs[Random.Range(0, gridPrefabs.Length)];
        }
        public void GenerateMap()
        {
            mapGrid = new Room[rows, columns];
            
            for (int currentRow = 0; currentRow < rows; currentRow++)
            {
                for (int currentCol = 0; currentCol < columns; currentCol++)
                {
                    float XPosition = RoomWidth * currentRow;
                    float ZPosition = RoomHeight * currentCol;
                    Vector3 newPosition = new Vector3(XPosition, 0, ZPosition);
                    
                    GameObject TempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;
                    
                    TempRoomObj.transform.parent = transform;
                    
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
                }
            }

        }

    }
}