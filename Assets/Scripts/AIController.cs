using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : Controller
{
    public bool HearsTarget => _aiHearing.HearsTarget;
    public bool SeesTarget => _aiVision.SeesTarget;
    public bool IsTargeting => _aiSphereCasting.IsTargeting;

    //variables for AI state machine
    public BaseAIState CurrentState;
    public EnemyTypes enemyType;

    public AiVision _aiVision;
    public AiHearing _aiHearing;
    public AiSpherecaster _aiSphereCasting;

    public TankPawn currentTarget;

    //Spherecast component is for shooting
    //Vision is for chasing
    //Hearing guides states, and hints to move to nearest waypoint

    public void Awake()
    {
        AIStateMachine.RegisterAgent(this);
        CurrentState = new PatrolState(this);
    }

    private void OnDeath()
    {
        GameManager.Instance.OnEnemyDeath(GetComponent<TankPawn>());
    }

    private void AIController_OnStatusChanged(bool seesTarget, HashSet<TankPawn> pawns)
    {
        if(seesTarget && currentTarget == null && pawns.Count > 0)
        {
            currentTarget = pawns.First();
        }
        AIStateMachine.RequestStateChange(this, pawns);
    }

    // Start is called before the first frame update
    public override void Start()
    {
        _aiVision = GetComponent<AiVision>();
        _aiHearing = GetComponent<AiHearing>();
        _aiSphereCasting = GetComponent<AiSpherecaster>();

        _aiVision.OnStatusChanged += AIController_OnStatusChanged;
        _aiHearing.OnStatusChanged += AIController_OnStatusChanged;
        
        CurrentState.Enter();

        pawn.Health.DeathEvent += OnDeath;

        // Run the parent (base) Start
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {

        DebugPlus.LogOnScreen($"Hears: {HearsTarget}");
        DebugPlus.LogOnScreen($"Sees: {SeesTarget}");
        DebugPlus.LogOnScreen($"Targeting: {IsTargeting}");
        string stateName = Enum.GetName(typeof(AIStateMachine.AIState), CurrentState.StateType);
        DebugPlus.LogOnScreen($"{name} State: {stateName}");
        CurrentState.Execute();
        
        ProcessInputs();
        
        // Run the parent (base) Update
        base.Update();
    }
    
    private void OnDestroy()
    {
        AIStateMachine.UnregisterAgent(this);
        _aiVision.OnStatusChanged -= AIController_OnStatusChanged;
        pawn.Health.DeathEvent -= OnDeath;
    }

    /*
    public void SetDestination(Transform)
    {
        
    }
    */
    
    public override void ProcessInputs()
    {
        base.ProcessInputs();
        
        float verticalInput = CurrentState.verticalInput;
        float horizontalInput = CurrentState.horizontalInput;
        
        if (verticalInput < 0)
        {
            horizontalInput = -CurrentState.horizontalInput;
        }
        
        pawn.Move(verticalInput);
        pawn.Rotate(horizontalInput);
        if (CurrentState.ShootInput)
        {
            pawn.Shoot();
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (CurrentState == null)
        {
            return;
        }

        Gizmos.color = Color.white;
        string stateName = Enum.GetName(typeof(AIStateMachine.AIState), CurrentState.StateType);
        UnityEditor.Handles.Label(transform.position + Vector3.up, stateName);
    }
#endif
}
