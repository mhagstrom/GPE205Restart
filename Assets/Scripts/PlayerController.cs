using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
public class PlayerController : Controller
{
    public AimCursor cursor;
    
    public void TakeControl(Pawn controlledPawn)
    {
        pawn = controlledPawn;
        //cursor = controlledPawn.GetComponentInChildren<AimCursor>();
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (pawn == null) return;
        ProcessInputs();
        
        base.Update();
    }
    
    public override void ProcessInputs()
    {
        base.ProcessInputs();
        
        //cursor.UpdateCursorPosition(Input.mousePosition);
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        cursor.UpdateCursorPosition(Input.mousePosition);
        
        if (verticalInput < 0)
        {
            horizontalInput = -Input.GetAxis("Horizontal");
        }

        void UpdateCursorPosition(Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);

            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
            {
                transform.position = hit.point;
            }
        }
        
        pawn.Move(verticalInput);
        pawn.Rotate(horizontalInput);
        if (Input.GetMouseButtonDown(0))
        {
            pawn.Shoot();
            pawn.MakeNoise();
        }
    }
}
