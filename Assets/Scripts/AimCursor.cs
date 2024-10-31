using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    //compile time cache reference
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    [Header("Aim Data")]
    [SerializeField] private LayerMask aimMask;
    //takes mouse cursor position from player controller

    private Renderer cursorRenderer;
    private MaterialPropertyBlock propertyBlock; //material property block for the aimSphere

    private void Awake()
    {
        //grab the renderer and material property block
        cursorRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        //fill a new property block on the aimSphere object to interact with
    }
    
    public void UpdateCursorPosition(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, aimMask))
        {
            transform.position = hit.point;
        }
    }
    
    //colorchange of aimsphere stopped working with updates to turret and guntube rotations, not sure why yet, probably because the guntube rotation hasn't been fixed yet so it's not aligning vertically
    //set the color of the aimSphere to red if the player is aiming at an enemy based on the show bool
    public void ShowTargetLock(bool show)
    {
        //get the material properties from the renderer
        cursorRenderer.GetPropertyBlock(propertyBlock);
        //set the color of the aimSphere to green if the player is aiming at an enemy based on the show bool
        //this is using ternary operator to set the color to green if show is true, otherwise set it to red
        //ternary operators are like mini functions, this one is a true false check
        propertyBlock.SetColor(Color1, show ? Color.green : Color.red);
        
        //apply the property block to the renderer
        cursorRenderer.SetPropertyBlock(propertyBlock);
    }
}
