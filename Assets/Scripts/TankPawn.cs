using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(Turret))]
[RequireComponent(typeof(NoiseMaker))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(BoxCollider))]
public class TankPawn : Pawn
{

    private TankMovement _movement;
    private TankShooter _shooter;
    
    //serializefields instead of getcomponent in awake
    [SerializeField] private Turret turret;
    [SerializeField] private AimCursor aimCursor;
    private float _timeSinceLastAttack = 0;
    
    public override void Awake()
    {
        _movement = GetComponent<TankMovement>();
        _shooter = GetComponent<TankShooter>();
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;
        
    }
    
    public override void Shoot()
    { //check if able to attack again
        if (_timeSinceLastAttack < attackRate)
        {
            return;
        }
        shooter.Shoot();
        _timeSinceLastAttack = 0;
    }

    public override void Movement(float f)
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        _movement.Move(verticalInput);
        _movement.Rotate(horizontalInput);
    }

    public override void MakeNoise()
    {
        
    }
}