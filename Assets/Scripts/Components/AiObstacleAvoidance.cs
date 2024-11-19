using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiObstacleAvoidance : MonoBehaviour
{
    /*
    [SerializeField] private float tickRate = 0.1f;
    private float timeSinceLastTick = 0;
    [Space(10)]
    
    private float _lastWallCheckTime;
    public bool _isWallLeft;
    public bool _isWallRight;
    public float avoidanceDistance = 5f;
    public float leftWallDistance;
    public float rightWallDistance;
    
    //event that gets raise when Wall changes
    public delegate void WallChanged(bool isWall);
    public event WallChanged OnWallChanged;
    //in AIController we should listen for this event and change movement direction according to which side the wall is detected
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public bool IsWallLeft
    {
        get
        {
            if (_lastWallCheckTime + tickRate < Time.time)
            {
                _isWallLeft = LeftWallCheck();
            }
            return _isWallLeft;
        }
        private set
        {
            if (_isWallLeft != value) OnWallChanged?.Invoke(value);
            _isWallLeft = value;
        }
    }
    
    public bool IsWallRight
    {
        get
        {
            if (_lastWallCheckTime + tickRate < Time.time)
            {
                _isWallRight = RightWallCheck();
            }
            return _isWallRight;
        }
        private set
        {
            if (_isWallRight != value) OnWallChanged?.Invoke(value);
            _isWallRight = value;
        }
    }

    private float LeftWallCheck()
    {
        RaycastHit hit;
        if (Physics.RayCast(transform.position, transform.forward, out hit, avoidanceDistance))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                //get distance to wall and return a float value
                return;
            }
            return //distance to wall float value;
        }
        return 0;
    }
    
    private float RightWallCheck()
    {
        RaycastHit hit;
        if (Physics.RayCast(transform.position, transform.forward, out hit, avoidanceDistance))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                //get distance to wall and return a float value
                return //distance to wall float value;
            }
            return //distance to wall float value;
        }
        return 0;
    }

    private void ChooseDirection()
    {
        //if both walls are detected, choose the direction with the most space
        if (IsWallLeft && IsWallRight)
        {
            if (leftWallDistance > rightWallDistance)
            {
                //move right
                AIController.SetDestination(transform.position + transform.right * avoidanceDistance);
            }
            else
            {
                //move left
                AIController.SetDestination(transform.position - transform.right * avoidanceDistance);
            }
        }
    }
    */
}
