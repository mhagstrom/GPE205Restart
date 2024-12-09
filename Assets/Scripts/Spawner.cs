using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    public Pawn pawn;
    //public Controller SpawnedController;
    public EnemyTypes[] enemyTypes;

    public PlayerController SpawnPlayer(int playerNum)
    {
        Pawn pawnInstance = Instantiate<Pawn>(pawn, transform.position, transform.rotation);

        pawnInstance.gameObject.tag = "Player";
        //cast to type
        PlayerController playerController = pawnInstance.gameObject.AddComponent<PlayerController>();
        playerController.TakeControl(pawnInstance, playerNum);
        return playerController;
    }

    public Controller SpawnEnemy()
    {
        Pawn pawnInstance = Instantiate<Pawn>(pawn, transform.position, transform.rotation);

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
    
}
