using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseAIState
{ 
    public ChaseState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 2;

    public AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }
    
    //qualifiers for state based on personality
    public static float teamwork = 0.8f;
    public static float cowardice = 1;
    public static float aggro = 0.5f;
    
    
    public override void Enter()
    {
        Debug.Log("Entering Chase State");
    }
    
    public override void Execute()
    {
        //check if AIController has an active target stored in its activeTarget variable to chase and if not return to patrol state
        
        if (!controller.SeesTarget)
        {
            
        }
        {
            // send inputs to controller.ProcessInputs(); to move towards the active target
        }

        Debug.Log("Executing Chase State");
        
        //use horizontalInput and verticalInput using ProcessInputs() to chase the active target
        //the code below needs to make use of the Hearing and Seeing provided in the AIController
        
        
        Debug.Log("Executing Chase State");
    }
    
    public override void Exit()
    {
        Debug.Log("Exiting Chase State");
    }
}
