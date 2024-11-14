using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TankShooter : Shooter
{
    public TankPawn tankPawn;

    public override void Awake()
    {
        tankPawn = GetComponent<TankPawn>();
    }

    public void Update()
    {
        shootingVolume = Mathf.Clamp(shootingVolume-0.1f, 0, shootingNoiseMultiplier);
    }
    
    public float shootingVolume { get; private set; }
    private float shootingNoiseMultiplier = 10f;
    public override void Shoot()
    {
        Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        newBullet.InitBullet(tankPawn, firePoint.forward);
        shootingVolume = shootingNoiseMultiplier;
    }
}
