using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
public class PlayerController : Controller
{
    public Pawn playerPawn;
    public AimCursor cursor;
    
    public void TakeControl(Pawn pawn)
    {
        playerPawn = pawn;
        cursor = playerPawn.GetComponentInChildren<AimCursor>();
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (playerPawn == null) return;
        ProcessInputs();
        
        base.Update();
    }
    
    public override void ProcessInputs()
    {
        base.ProcessInputs();
        
        cursor.UpdateCursorPosition(Input.mousePosition);
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput < 0)
        {
            horizontalInput = -Input.GetAxis("Horizontal");
        }

        playerPawn.Movement(verticalInput);
        if (Input.GetMouseButtonDown(0))
        {
            playerPawn.Shoot();
            playerPawn.MakeNoise();
        }
    }
}
