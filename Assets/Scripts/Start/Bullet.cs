using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    Material[] bulletColors;
    Vector3 direction;
    NetworkRigidbody rb;
   

    public override void Spawned()
    {
        base.Spawned();
        rb = GetComponent<NetworkRigidbody>();
        GetComponentInChildren<MeshRenderer>().material = bulletColors[Object.InputAuthority.PlayerId];
        if (HasStateAuthority &&HasInputAuthority)
        {
            rb.Rigidbody.AddForce(direction*100);
        }
    }
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
}
