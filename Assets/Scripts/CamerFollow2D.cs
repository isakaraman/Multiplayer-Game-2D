using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow2D : MonoBehaviour
{
    public float speed = 25f;
    public float interVelocity; 

    public GameObject target;
    
    public Vector3 offset;
    Vector3 targetPos;
    public Vector2 minBoundry;
    public Vector2 maxBoundry;
    
    
    void Start()
    {
        targetPos = transform.position;  
    }

   
    void Update()
    {
        if (target)
        {
            Vector3 posZ = transform.position;
            posZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posZ);
            interVelocity = targetDirection.magnitude * speed;

            targetPos = transform.position + (targetDirection.normalized * interVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset,0.25f);

            transform.position = new Vector3
                (
                Mathf.Clamp(transform.position.x, minBoundry.x, maxBoundry.x),
                Mathf.Clamp(transform.position.y, minBoundry.y, maxBoundry.y),
                transform.position.z
                );




        }
    }
}
