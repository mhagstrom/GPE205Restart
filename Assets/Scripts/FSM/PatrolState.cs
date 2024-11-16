using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseAIState
{
    public PatrolState(AIController controller) : base(controller){}
    
    public override int stateType { get; protected set; } = 1;

    public AIStateMachine.AIState StateType
    {
        get { return (AIStateMachine.AIState)stateType; }
    }

    private PatrolPath path;
    private Transform currentWaypoint;
    
    //qualifiers for state based on personality
    public static float teamwork = 0.8f;
    public static float cowardice = 1;
    public static float aggro = 0.5f;
    
    public override void Enter()
    {
        if (path == null)
        {
            path = GameManager.Instance.GetClosestPatrolPath(controller.transform.position);
        }
        //Debug.Log("Entering Patrol State");
    }
    
    public override void Execute()
    {
        Debug.Log($"Current state: {(int)StateType}");
        if (currentWaypoint == null)
        {
            currentWaypoint = path.GetNearestWaypoint(controller.transform.position);
        }
        if(Vector3.Distance(currentWaypoint.position, controller.transform.position) < 1) //change hardcode value 1 to a variable
            //line 31 breaks pawn controller pattern, controller should communicate data to from pawn 
            //controller object with variable for pawn to control instead of controller as component on tank object
        {
            currentWaypoint = path.GetNextWaypoint(currentWaypoint);
        }
        //if angle is more than 5 degrees set the horizontal input to 1
        float angle = Vector3.Angle(controller.transform.forward, (currentWaypoint.position - controller.transform.position).normalized);
        if (angle > 3)
        {
            horizontalInput = 1;
        }

        if (angle < -3)
        {
            horizontalInput = -1;
        }
        if (angle > -3 && angle < 3)
        {
            horizontalInput = 0;
        }
        verticalInput = 0.33f;
    }
    
    public override void Exit()
    {
        //Debug.Log("Exiting Patrol State");
    }
}
