using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public Movement movement;
    public Shooter shooter;
    public Health health;

    public float hullMoveSpeed;
    public float hullRotateSpeed;
    public float attackRate;
    public Transform cameraMount;

    public abstract void Awake();
    
    public abstract void Start();

    public abstract void Update();
    
    public abstract void Shoot();
    
    public abstract void Move(float verticalInput);

    public abstract void Rotate(float horizontalInput);
    
    public abstract void MakeNoise();
    
}
