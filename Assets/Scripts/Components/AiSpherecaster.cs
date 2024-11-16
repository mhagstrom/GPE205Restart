using UnityEngine;

public class AiSpherecaster : MonoBehaviour
{
    [SerializeField] private float tickRate = 0.1f;
    private float timeSinceLastTick = 0;
    [Space(10)]
    
    //targeting distance and spherecast radius for the AI for direct line of sight
    [SerializeField] float targetingDistance = 40f;
    [SerializeField] float spherecastRadius = 0.15f;
    private float _lastSphereCheckTime;
    private bool _isTargeting;
    
    //event that gets raise when istargeting changes
    public delegate void TargetingChanged(bool isTargeting);
    public event TargetingChanged OnTargetingChanged;
    
    public bool IsTargeting
    {
        get
        {
            if (_lastSphereCheckTime + tickRate < Time.time)
            {
                _isTargeting = SpherecastCheck();
            }
            return _isTargeting;
        }
        private set
        {
            if (_isTargeting != value) OnTargetingChanged?.Invoke(value);
            _isTargeting = value;
        }
    }
    
    private bool SpherecastCheck()
    {
        _lastSphereCheckTime = Time.time;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, spherecastRadius, transform.forward, out hit, targetingDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
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
            IsTargeting = SpherecastCheck();
        }
    }
}