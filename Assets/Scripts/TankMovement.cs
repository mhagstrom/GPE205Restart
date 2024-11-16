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
    public float movingVolume { get; private set; }
    public float rotatingVolume { get; private set; }
    
    //can be tweaked so rotating is quieter than moving
    private float moveNoiseMultiplier = 5;
    private float rotateNoiseMultiplier = 5;

    public override void Move(float verticalInput)
    {
        _rb.MovePosition(transform.position + transform.forward * (verticalInput * Time.deltaTime * _tankPawn.hullMoveSpeed));
        
        movingVolume = Mathf.Abs(verticalInput) * moveNoiseMultiplier;
    }
    
    public override void Rotate(float horizontalInput)
    {
        transform.Rotate(0, horizontalInput * Time.deltaTime * _tankPawn.hullRotateSpeed, 0);
        
        rotatingVolume = Mathf.Abs(horizontalInput) * rotateNoiseMultiplier;
    }
    
}
