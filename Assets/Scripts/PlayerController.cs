using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
public class PlayerController : Controller
{
    [field:SerializeField]
    public int Lives { get; private set; } = 3;
    public int Score { get; private set; }

    public int PlayerNumber;
    public Transform Camera { get; private set; }
    public void TakeControl(Pawn controlledPawn, int playerNum = 0)
    {
        Camera = playerNum == 0 ? GameManager.Instance.P1Camera.transform : GameManager.Instance.p2Camera.transform;

        pawn = controlledPawn;
        Camera.SetParent(pawn.cameraMount);
        Camera.localPosition = Vector3.zero;
        Camera.localRotation = Quaternion.identity;
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void Respawn()
    {
        Lives--;

        if(Lives == 0)
        {
            pawn.Movement.enabled = false;
            GameManager.Instance.GameOver(this);
            return;
        }

        GameManager.Instance.Respawn(pawn);
        pawn.Health.Heal(100);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        pawn.Health.DeathEvent += OnDeath;
    }

    private void OnDestroy()
    {
        if (Camera != null)
            Camera.transform.parent = null;

        pawn.Health.DeathEvent -= OnDeath;
    }

    private void OnDeath()
    {
        Respawn();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (pawn == null) return;
        ProcessInputs();
        
        base.Update();
    }

    public string VerticalAxis => PlayerNumber == 0 ? "Vertical" : "VerticalP2";
    public string HorizontalAxis => PlayerNumber == 0 ? "Horizontal" : "HorizontalP2";
    public string Fire => PlayerNumber == 0 ? "Fire1" : "Fire1P2";

    public string Item1 => PlayerNumber == 0 ? "Item1" : "Item1P2";
    public string Item2 => PlayerNumber == 0 ? "Item2" : "Item2P2";
    public string Item3 => PlayerNumber == 0 ? "Item3" : "Item3P2";

    bool Dead => !pawn.Movement.enabled;

    public override void ProcessInputs()
    {
        base.ProcessInputs();
        
        float verticalInput = Input.GetAxis(VerticalAxis);
        float horizontalInput = Input.GetAxis(HorizontalAxis);
        if (verticalInput < 0)
        {
            horizontalInput = -Input.GetAxis(HorizontalAxis);
        }
        
        if (Dead)
        {
            verticalInput = 0;
            horizontalInput = 0;
        }
        
        pawn.Move(verticalInput);
        pawn.Rotate(horizontalInput);

        if (Input.GetButton(Fire) && !Dead)
        {
            pawn.Shoot();
        }

        if (Dead) return;

        if (Input.GetButtonDown(Item1))
        {
            pawn.ActivatePowerup(PowerupType.HealthBoost);
        }
        
        if (Input.GetButtonDown(Item2))
        {
            pawn.ActivatePowerup(PowerupType.SpeedBoost);
        }
        
        if (Input.GetButtonDown(Item3))
        {
            pawn.ActivatePowerup(PowerupType.DamageBoost);
        }
        
    }
}
