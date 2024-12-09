using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiObstacleAvoidance : MonoBehaviour
{
    [SerializeField] private float tickRate = 0.1f;
    [Space(10)]
    
    private float _lastWallCheckTime;
    public bool _isWallLeft;
    public bool _isWallRight;
    public float avoidanceDistance = 5f;
    public float leftWallDistance;
    public float rightWallDistance;

    public Vector3 suggestedDirection;
    
    //event that gets raise when Wall changes
    public delegate void WallChanged(bool isWall);
    public event WallChanged OnWallChanged;
    //in AIController we should listen for this event and change movement direction according to which side the wall is detected

    private void Update()
    {
        if (_lastWallCheckTime + tickRate < Time.time)
        {
            IsWallRight = LeftWallCheck() > 0;
            IsWallLeft = RightWallCheck() > 0;
            _lastWallCheckTime = Time.time;
        }

        ChooseDirection();
    }

    public bool IsWallLeft
    {
        get
        {
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
        var left = (transform.forward - transform.right).normalized;

        float distance = -1f;
        if (Physics.Raycast(transform.position, left, out hit, avoidanceDistance))
        {
            distance = hit.distance;
        }
        return distance;
    }
    
    private float RightWallCheck()
    {
        RaycastHit hit;
        var right = (transform.forward + transform.right).normalized;

        float distance = -1f;
        if (Physics.Raycast(transform.position, right, out hit, avoidanceDistance))
        {
            distance = hit.distance;
        }
        return distance;
    }

    private void ChooseDirection()
    {
        //if both walls are detected, choose the direction with the most space
        if (IsWallLeft && IsWallRight)
        {
            if (leftWallDistance > rightWallDistance)
            {
                suggestedDirection = transform.right;
            }
            else
            {
                suggestedDirection = -transform.right;
            }
        }
    }

}
