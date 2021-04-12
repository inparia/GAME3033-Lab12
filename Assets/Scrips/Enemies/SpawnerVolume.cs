using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class SpawnerVolume : MonoBehaviour
{

    private BoxCollider Collider;


    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    
    public Vector3 GetPositionInBounds()
    {
        Bounds BoxBounds = Collider.bounds;
        return new Vector3(
            Random.Range(BoxBounds.min.x, BoxBounds.max.x),
            transform.position.y,
            Random.Range(BoxBounds.min.z, BoxBounds.max.z));
    }
}
