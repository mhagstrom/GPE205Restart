using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : BaseAIState
{
    //qualifiers for state based on personality
    public static float teamwork = 0.7f;
    public static float cowardice = 0.5f;
    public static float aggro = 0.3f;
    
    
    public StunnedState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 0;

    public AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }
    
    private float idleTimer = 5.0f;
    private float elapsedTime = 0.0f;
    public override void Enter()
    {
        Debug.Log("Entering Stunned State");
    }

    public override void Execute()
    {
        //needs a timer during which no state change or action will be taken after which the AIController must choose a new state
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= idleTimer)
        {
            
        }
        Debug.Log("Executing Stunned State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Stunned State");
    }
}

