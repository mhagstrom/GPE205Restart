using System;
using UnityEngine;

public class AiHearing : MonoBehaviour
{
    [SerializeField] private float tickRate = 0.1f;
    private float timeSinceLastTick = 0;
    [Space(10)]
    
    //hearing distance for the AI
    private NoiseMaker[] noiseMakers;
    private NoiseMaker _noiseMaker;
    [SerializeField] private float hearingDistance = 10;
    private float _lastListenedTime;
    private bool _hearsTarget;
    
    //event that gets raise when istargeting changes
    public delegate void TargetingChanged(bool isTargeting);
    public event TargetingChanged OnTargetingChanged;

    private void Start()
    {
        noiseMakers = FindObjectsOfType<NoiseMaker>();
    }

    public bool HearsTarget
    {
        get
        {
            //check if time is greather than the cached time + the rate
            if (_lastListenedTime + tickRate < Time.time)
            {
                _hearsTarget = CanHear(_noiseMaker);
            }
            return _hearsTarget;
        }
        private set
        {
            if (_hearsTarget != value) OnTargetingChanged?.Invoke(value);
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
            foreach (var noiseMaker in noiseMakers)
            {
                _noiseMaker = noiseMaker;
                HearsTarget = CanHear(_noiseMaker);
            }
        }
        Debug.Log($"Hears target: {HearsTarget}");
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingDistance);
        Gizmos.color = Color.red;

        if (_noiseMaker == null) return;
        Gizmos.DrawWireSphere(transform.position, _noiseMaker.volumeDistance);
    }
    #endif
}
