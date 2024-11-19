using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAIState //no monobehavior complexity here
{
    //constructor runs when object is created
    public BaseAIState(AIController controller)
    {
        this.controller = controller;
    }
    
    public float teamwork;
    public float cowardice;
    public float aggro;
    
    protected AIController controller;
    public float verticalInput = 0;
    public float horizontalInput = 0;
    public bool ShootInput = false;

    public AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }

    public abstract int stateType { get; protected set; }
    public virtual void Enter()
    {
        
    }
    
    public virtual void Execute()
    {
        
    }
    
    public virtual void Exit()
    {
        
    }
}
