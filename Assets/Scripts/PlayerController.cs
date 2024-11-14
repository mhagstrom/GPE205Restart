using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
public class PlayerController : Controller
{
    //we need to get the camera mount from the tank prefab that we are controlling
    public void TakeControl(Pawn controlledPawn)
    {
        pawn = controlledPawn;
        GameManager.Instance.mainCamera.transform.SetParent(pawn.cameraMount);
        GameManager.Instance.mainCamera.transform.localPosition = Vector3.zero;
        GameManager.Instance.mainCamera.transform.localRotation = Quaternion.identity;
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
        
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (verticalInput < 0)
        {
            horizontalInput = -Input.GetAxis("Horizontal");
        }
        
        pawn.Move(verticalInput);
        pawn.Rotate(horizontalInput);
        if (Input.GetMouseButtonDown(0))
        {
            pawn.Shoot();
        }
    }
}
