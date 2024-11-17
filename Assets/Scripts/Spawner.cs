using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    public Pawn pawn;
    //public Controller SpawnedController;
    public EnemyTypes[] enemyTypes;
    
    public Controller Spawn(ControllerType spawnedControllerType)
    {
        Pawn pawnInstance = Instantiate<Pawn>(pawn, transform.position, transform.rotation);
        //This needs to instantiate either a player controller or an AI controller respective to the tank being spawned
        //Controller controllerInstance = pawnInstance.gameObject.AddComponent(spawnedControllerType.GetType()) as Controller;
        // ! is used to tell the compiler trust me bro, I just made the controller
        //controllerInstance!.pawn = pawnInstance;
        
        if (spawnedControllerType == ControllerType.Player)
        {
            pawnInstance.gameObject.tag = "Player";
            //cast to type
            PlayerController playerController = pawnInstance.gameObject.AddComponent<PlayerController>();
            playerController.TakeControl(pawnInstance);
            return playerController;
        }
        else if (spawnedControllerType == ControllerType.AI)
        {
            pawnInstance.gameObject.tag = "Enemy";
            //pick random enemy type
            EnemyTypes enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
            AIController aiController = pawnInstance.gameObject.AddComponent<AIController>();
            AiHearing aiHearing = pawnInstance.gameObject.AddComponent<AiHearing>();
            AiVision aiVision = pawnInstance.gameObject.AddComponent<AiVision>();
            AiSpherecaster aiSpherecaster = pawnInstance.gameObject.AddComponent<AiSpherecaster>();
            aiController.enemyType = enemyType;
            aiController.pawn = pawnInstance;
            return aiController;
        }

        return null;
    }
    
}
