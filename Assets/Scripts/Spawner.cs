using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Pawn pawn;
    public Controller controller;

    public Controller Spawn()
    {
        Pawn pawnInstance = Instantiate<Pawn>(pawn, transform.position, transform.rotation);
        //This needs to instantiate either a player controller or an AI controller respective to the tank being spawned
        Controller controllerInstance = pawnInstance.gameObject.AddComponent(controller.GetType()) as Controller;
        // ! is used to tell the compiler trust me bro, I just made the controller
        controllerInstance!.pawn = pawnInstance;
        
        if (controllerInstance is PlayerController)
        {
            //cast to type
            PlayerController playerController = controllerInstance as PlayerController;
            playerController.TakeControl(pawnInstance);
        }
        return controllerInstance;
    }
}
