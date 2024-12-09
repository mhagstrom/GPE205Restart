using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AiSense : MonoBehaviour
{
    public TankPawn MyPawn;

    private void Awake()
    {
        MyPawn = GetComponent<TankPawn>();
    }
}

public class AiHearing : AiSense
{
    [SerializeField] private float tickRate = 0.1f;
    private float timeSinceLastTick = 0;
    [Space(10)]
    
    //hearing distance for the AI
    //need to remove destroyed noisemakers from this array to prevent null exceptions
    [SerializeField] private float hearingDistance = 10;
    private float _lastListenedTime;
    private bool _hearsTarget;

    HashSet<TankPawn> heardTargets = new HashSet<TankPawn>();
    
    //event that gets raise when istargeting changes
    public delegate void TargetingChanged(bool isTargeting, HashSet<TankPawn> target);
    public event TargetingChanged OnStatusChanged;

    public bool HearsTarget
    {
        get
        {
            return _hearsTarget;
        }
        private set
        {
            _hearsTarget = value;
        }
    }
    
    public bool CanHear(NoiseMaker noise)
    {
        _lastListenedTime = Time.time;
        //this throws an error when tanks are destroyed because the list of noise makers is not updated in the game manager and its dependencies
        if (Vector3.Distance(noise.transform.position, transform.position) < noise.volumeDistance + hearingDistance)
        {
            return true;
        }
        return false;
    }

    private void Update()
    {
        timeSinceLastTick += Time.deltaTime;
        if (timeSinceLastTick > tickRate)
        {
            timeSinceLastTick = 0;

            for (int i = 0; i < GameManager.Instance.AllPawns.Count; i++)
            {
                var pawn = GameManager.Instance.AllPawns[i];
                if (pawn == null) continue;

                bool changed = false;
                if (pawn == MyPawn) continue;

                var heard = CanHear(pawn.NoiseMaker);
                
                if (heard && !heardTargets.Contains(pawn))
                {
                    heardTargets.Add(pawn);
                    changed = true;
                }
                else if (!heard && heardTargets.Contains(pawn))
                {
                    heardTargets.Remove(pawn);
                    changed = true;
                }

                HearsTarget = heardTargets.Count > 0;
                if(changed) 
                {
                    Debug.Log("Change!");
                    OnStatusChanged?.Invoke(HearsTarget, heardTargets);
                }
            }
        }
        //Debug.Log($"Hears target: {HearsTarget}");
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingDistance);
        Gizmos.color = Color.red;
    }
    #endif
}
