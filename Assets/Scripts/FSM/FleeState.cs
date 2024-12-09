using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : BaseAIState
{
    //qualifiers for state based on personality
    public new static float teamwork = 0.2f;
    public new static float cowardice = 1;
    public new static float aggro = 0.5f;
    public FleeState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 4;

    public new AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }
    
    public override void Enter()
    {
        Debug.Log("Entering Idle State");
    }
    
    public override void Execute()
    {
        //negate movement vector toward player, once reached distance allow statechange

        verticalInput = -.3f;

        //Debug.Log("Executing Idle State");
    }
    
    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
