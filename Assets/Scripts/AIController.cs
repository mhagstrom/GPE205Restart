using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    [SerializeField] private float tickRate = 0.1f;
    private float lastTickTime;
    
    private Transform[] Targets; //will be used later to store all the targets in the scene
    
    private Transform activeTarget; //stores which pawn the AI wants to chase or shoot at
    
    public bool HearsTarget { get; set; }
    public bool SeesTarget { get; set; }
    public bool IsTargeting { get; set; }
    
    //variables for AI state machine
    public BaseAIState CurrentState;
    public EnemyTypes enemyType;
    
    public void Awake()
    {
        AIStateMachine.RegisterAgent(this);
        CurrentState = new GuardState(this);
        
    }
    
    // Start is called before the first frame update
    public override void Start()
    {
        CurrentState.Enter();
        // Run the parent (base) Start
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        Debug.Log($"Current state: {(int)CurrentState.StateType}");
        //Debug.Log($"Horizontal: {CurrentState.horizontalInput}");
        //Debug.Log($"Vertical: {CurrentState.verticalInput}");
        lastTickTime += Time.deltaTime;
        if (lastTickTime > tickRate)
        {
            lastTickTime = 0;
            //might change state every 10th of a second :( 
            /* I want to update these values here to trigger state change but idk implementation
             * 
             * _hearsTarget = CanHear(_noiseMaker);
             * _seesTarget = CanSee(activeTarget);
             * _isTargeting = SpherecastCheck();
            */
            
            // These sort of senses should be separated out into their own components 
            // and raise events when their own state changes. Ai controller should add those components
            // and listen for thier events to then request a state change

            AIStateMachine.RequestStateChange(this);
        }
        
        CurrentState.Execute();
        
        ProcessInputs();
        
        // Run the parent (base) Update
        base.Update();
    }
    
    private void OnDestroy()
    {
        AIStateMachine.UnregisterAgent(this);
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
