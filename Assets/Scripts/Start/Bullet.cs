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
   List<Collider> collisions= new List<Collider>();
    TickTimer timer;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (HasStateAuthority && timer.ExpiredOrNotRunning(Runner))
        {
            Runner.Despawn(Object);
        }
        else if (HasStateAuthority)
        {
        }
    }

    public override void Spawned()
    {
        base.Spawned();
        rb = GetComponent<NetworkRigidbody>();
        GetComponentInChildren<MeshRenderer>().material = bulletColors[Object.InputAuthority.PlayerId];
        if (HasStateAuthority &&HasInputAuthority)
        {
            rb.Rigidbody.AddForce(direction*100);
            timer = TickTimer.CreateFromSeconds(Runner, 3);
        }
    }
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (HasStateAuthority && other.gameObject.layer == 7
            && !other.gameObject.GetComponent<NetworkObject>().HasStateAuthority
             && !collisions.Contains(other))
        {
            collisions.Add(other);
            other.gameObject.GetComponent<ICanTakeDamage>().ApplyDamage(20, Object.InputAuthority,
                () =>
                {
                    Runner.Despawn(Object);
                });
        }
    }
    IEnumerator TimeEx()
    {
        yield return new WaitForSeconds(3f);
        Runner.Despawn(Object);
    }
}
