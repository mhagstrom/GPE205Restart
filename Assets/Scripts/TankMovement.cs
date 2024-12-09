using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : Movement
{
    private TankPawn _tankPawn;
    private Rigidbody _rb;
    //store the rigidbody as a variable called rb
    
    private void Awake()
    {
        _tankPawn = GetComponent<TankPawn>();
        _rb = GetComponent<Rigidbody>();
        //associate the rigidbody component on the tank gameobject with the name rb
    }

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    //need to learn about namespaces so I can make these internal
    //movement is framerate independent with velocity

    public float verticalInput;
    public float horizontalInput;

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.forward * (verticalInput * Time.fixedDeltaTime * _tankPawn.hullMoveSpeed * _tankPawn.PowerupManager.speedMultiplier));
        transform.Rotate(0, horizontalInput * Time.fixedDeltaTime * _tankPawn.hullRotateSpeed * _tankPawn.PowerupManager.speedMultiplier, 0);
    }

    public override void Move(float verticalInput)
    {
        this.verticalInput = verticalInput;

    }
    
    public override void Rotate(float horizontalInput)
    {
        this.horizontalInput = horizontalInput;

    }
    
}
