using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(NoiseMaker))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(BoxCollider))]
public class TankPawn : Pawn
{
    public TankAudio TankAudio;
    public NoiseMaker NoiseMaker;
    [FormerlySerializedAs("powerupManager")] [FormerlySerializedAs("powerupController")]
    public PowerupManager PowerupManager;
    
    //serializefields instead of getcomponent in awake
    private float _timeSinceLastAttack = 0;
    
    public override void Awake()
    {
        Health = GetComponent<Health>();
        Shooter = GetComponent<TankShooter>();
        Movement = GetComponent<TankMovement>();

        TankAudio = GetComponent<TankAudio>();
        NoiseMaker = GetComponent<NoiseMaker>();
        PowerupManager = GetComponent<PowerupManager>();
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
        Shooter.Shoot();
        TankAudio.ShootingNoise();
        _timeSinceLastAttack = 0;
    }

    public override void Move(float verticalInput)
    {
        Movement.Move(verticalInput);
    }

    public override void Rotate(float horizontalInput)
    {
        Movement.Rotate(horizontalInput);
    }
    
    public override void MakeNoise()
    {
        NoiseMaker.MakeNoise();
    }
    
    public override void ActivatePowerup(PowerupType powerup)
    {
        PowerupManager.ActivatePowerup(powerup);
    }
    
}
