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
        Attack,
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
    
    public static void RequestStateChange(AIController agent)
    {
        List<AIState> possibleStates = AssessState(agent);
        AIState newState = GetNewState(possibleStates, agent.enemyType);

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
            case AIState.Attack:
                state = new AttackState(agent);
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
    
    private static List<AIState> AssessState(AIController agent)
    {
        AIState currentState = agent.CurrentState.StateType;
        List<AIState> possibleStates = new List<AIState>();
        switch (currentState)
        {
            case AIState.Patrol:
                possibleStates.Add(AIState.Chase);
                possibleStates.Add(AIState.Attack);
                possibleStates.Add(AIState.Flee);
                possibleStates.Add(AIState.Stunned);
                break;
                
            case AIState.Chase:
                possibleStates.Add(AIState.Attack);
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Flee);
                possibleStates.Add(AIState.Stunned);
                break;
                
            case AIState.Attack:
                possibleStates.Add(AIState.Chase);
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Flee);
                possibleStates.Add(AIState.Stunned);
                break;
                
            case AIState.Flee:
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Chase);
                possibleStates.Add(AIState.Attack);
                break;
                
            case AIState.Stunned:
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Chase);
                possibleStates.Add(AIState.Attack);
                break;
            
            case AIState.Guard:
                possibleStates.Add(AIState.Patrol);
                possibleStates.Add(AIState.Chase);
                possibleStates.Add(AIState.Attack);
                possibleStates.Add(AIState.Flee);
                possibleStates.Add(AIState.Stunned);
                break;
        }

        bool enemyDetected = false;

        if (agent.HearsTarget || agent.SeesTarget || agent.IsTargeting)
        {
            enemyDetected = true;
        }

        if (!enemyDetected)
        {
            possibleStates.Remove(AIState.Chase);
            possibleStates.Remove(AIState.Attack);
            possibleStates.Remove(AIState.Flee);
            possibleStates.Remove(AIState.Stunned);
        }
        return possibleStates;
    }
    
    private static AIState GetNewState(List<AIState> possibleStates, EnemyTypes personality)
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
                case AIState.Attack:
                    scores.Add(AIState.Attack, GetStateScore(AttackState.teamwork, AttackState.aggro, AttackState.cowardice, personality));
                    break;
                case AIState.Flee:
                    scores.Add(AIState.Flee, GetStateScore(FleeState.teamwork, FleeState.aggro, FleeState.cowardice, personality));
                    break;
                case AIState.Stunned:
                    scores.Add(AIState.Stunned, GetStateScore(StunnedState.teamwork, StunnedState.aggro, StunnedState.cowardice, personality));
                    break;
                case AIState.Guard:
                    scores.Add(AIState.Guard, GetStateScore(GuardState.teamwork, GuardState.aggro, GuardState.cowardice, personality));
                    break;
            }
            
        }

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
