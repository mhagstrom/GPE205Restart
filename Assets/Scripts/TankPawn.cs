using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(NoiseMaker))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(BoxCollider))]
public class TankPawn : Pawn
{
    private TankMovement _movement;
    private TankShooter _shooter;
    private NoiseMaker _noiseMaker;
    
    //serializefields instead of getcomponent in awake
    private float _timeSinceLastAttack = 0;
    
    public override void Awake()
    {
        _movement = GetComponent<TankMovement>();
        _shooter = GetComponent<TankShooter>();
        _noiseMaker = GetComponent<NoiseMaker>();
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;
        MakeNoise();
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

    public override void Move(float verticalInput)
    {
        movement.Move(verticalInput);
    }

    public override void Rotate(float horizontalInput)
    {
        movement.Rotate(horizontalInput);
    }
    
    public override void MakeNoise()
    {
        //in the future we could apply a curve to smooth out the noise
        float noiseVolume = _movement.movingVolume + _movement.rotatingVolume + _shooter.shootingVolume;
        _noiseMaker.volumeDistance = noiseVolume;
        //Debug.Log ($"movingVolume: {_movement.movingVolume}, rotatingVolume: {_movement.rotatingVolume}, shootingVolume: {_shooter.shootingVolume}");
    }
    
}
