using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    [SerializeField] private Rigidbody bulletrb;

    public int damage = 10;
    [SerializeField] private float velocity = 20;
    //[SerializeField] private int bulletLifetimeSecs = 10;
    [SerializeField] private GameObject bulletVFX;
    private Pawn _owner;

    public float lifeTime = 5f;

    public void InitBullet(Pawn firingowner, Vector3 direction)
    {
        _owner = firingowner;
        bulletrb.velocity = direction * velocity;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Health health))
        {
            health.SetLastDamager(_owner.GetComponent<TankPawn>());
            health.OnDamageTaken(damage);
        }

        Instantiate(bulletVFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
