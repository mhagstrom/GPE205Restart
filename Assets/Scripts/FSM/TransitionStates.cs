using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIStateMachine
{
    public enum AIState
    {
        Stunned,
        Patrol,
        Chase,
        Guard,
        Flee,
    }
    
    private static List<AIController> AgentList = new List<AIController>();
    
    public static void RegisterAgent(AIController agent)
    {
        AgentList.Add(agent);
        
        //ForcePatrolState(); //temporary for debugging
    }
    
    public static void UnregisterAgent(AIController agent)
    {
        AgentList.Remove(agent);
    }
    
    public static void RequestStateChange(AIController agent, HashSet<TankPawn> targets)
    {
        List<AIState> possibleStates = AssessState(agent, targets);
        AIState newState = GetNewState(agent, possibleStates, agent.enemyType);

        if (agent.CurrentState.StateType == newState)
        {
            return;
        }
        
        BaseAIState state = null;
        switch (newState)
        {
            case AIState.Patrol:
                state = new PatrolState(agent);
                break;
            case AIState.Chase:
                state = new ChaseState(agent);
                break;
            case AIState.Flee:
                state = new FleeState(agent);
                break;
            case AIState.Stunned:
                state = new StunnedState(agent);
                break;
            case AIState.Guard:
                state = new GuardState(agent);
                break;
        }
        agent.CurrentState.Exit();
        agent.CurrentState = state;
        agent.CurrentState.Enter();
    }
    
    private static List<AIState> AssessState(AIController agent, HashSet<TankPawn> targets)
    {
        AIState currentState = agent.CurrentState.StateType;
        List<AIState> possibleStates = new List<AIState>();
        switch (currentState)
        {
            case AIState.Patrol:
                possibleStates.Add(AIState.Chase);
                possibleStates.Add(AIState.Guard);
                possibleStates.Add(AIState.Flee);
                possibleStates.Add(AIState.Stunned);
                break;
                
            case AIState.Chase:
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Flee);
                possibleStates.Add(AIState.Stunned);
                possibleStates.Add(AIState.Chase);
                break;

            case AIState.Flee:
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Chase);
                break;
                
            case AIState.Stunned:
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Chase);
                break;
            
            case AIState.Guard:
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Chase);
                possibleStates.Add(AIState.Flee);
                possibleStates.Add(AIState.Stunned);
                break;
        }

        bool enemyDetected = false;

        if (agent.SeesTarget || agent.IsTargeting)
        {
            enemyDetected = true;
        }

        if (!agent.SeesTarget && agent.HearsTarget)
        {
            if(possibleStates.Contains(AIState.Chase))
            {
                possibleStates.Remove(AIState.Chase);
            }
        }

        if (!enemyDetected)
        {
            possibleStates.Remove(AIState.Chase);
            possibleStates.Remove(AIState.Flee);
            possibleStates.Remove(AIState.Stunned);
        }
        else
        {
            possibleStates.Remove(AIState.Patrol);

            if (agent.GetComponent<Health>().CurrentHealth <= 30)
            {
                if(!possibleStates.Contains(AIState.Flee))
                    possibleStates.Add(AIState.Flee);
            }
        }

        return possibleStates;
    }
    
    private static AIState GetNewState(AIController agent, List<AIState> possibleStates, EnemyTypes personality)
    {
        if (possibleStates.Count == 0)
        {
            return AIState.Stunned;
        }

        if (possibleStates.Count == 1)
        {
            return possibleStates[0];
        }

        // where are we putting into account the other 
        // game factors ie: health, sensed enemies/friendlies?

        var health = agent.GetComponent<Health>();

        float healthWeight = 0;
        if (health.CurrentHealth < 30)
        {
            var chance = UnityEngine.Random.Range(0, 100) > 50;

            if(chance)
            {
                healthWeight = 1f;
            }
        }



        float highestScore = 0;
        Dictionary<AIState, float> scores = new Dictionary<AIState, float>();
        foreach (AIState state in possibleStates)
        {
            switch (state)
            {
                case AIState.Patrol:
                    scores.Add(AIState.Patrol, GetStateScore(PatrolState.teamwork, PatrolState.aggro, PatrolState.cowardice, personality));
                    break;
                case AIState.Chase:
                    scores.Add(AIState.Chase, GetStateScore(ChaseState.teamwork, ChaseState.aggro, ChaseState.cowardice, personality));
                    break;
                case AIState.Flee:
                    scores.Add(AIState.Flee, GetStateScore(FleeState.teamwork, FleeState.aggro, FleeState.cowardice, personality) + healthWeight);
                    break;
                case AIState.Stunned:
                    scores.Add(AIState.Stunned, GetStateScore(StunnedState.teamwork, StunnedState.aggro, StunnedState.cowardice, personality));
                    break;
                case AIState.Guard:
                    scores.Add(AIState.Guard, GetStateScore(GuardState.teamwork, GuardState.aggro, GuardState.cowardice, personality));
                    break;
            }
            
        }

        //TODO: Damage taken calculations here

        //TODO: Idle timer

        //TODO: Safe timer

        //TODO: Clear currentTarget when not seen in vision component for x amount of time

        foreach (float value in scores.Values)
        {
            if (value > highestScore)
            {
                highestScore = value;
            }
        }
        
        foreach (KeyValuePair<AIState, float> score in scores)
        {
            if (score.Value == highestScore)
            {
                return score.Key;
            }
        }
        return AIState.Stunned;
    }
    
    private static float GetStateScore(float teamwork, float aggro, float cowardice, EnemyTypes personality)
    {
        return teamwork * personality.teamwork + cowardice * personality.cowardice + aggro * personality.aggro;
    }
    
    /*
    //temporary force current state to PatrolState is not working
    public static void ForcePatrolState()
    {
        //this will be removed later
        foreach (AIController agent in AgentList)
        {
            agent.CurrentState = new PatrolState(agent);
            agent.CurrentState.Enter();
        }
    }
    */
}
