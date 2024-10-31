using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public abstract void Start();
    
    public abstract void Move(float verticalInput);

    public abstract void Rotate(float horizontalInput);
}
