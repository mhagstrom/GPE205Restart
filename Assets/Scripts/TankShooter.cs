using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TankShooter : Shooter
{
    private TankPawn tankPawn;
    private NoiseMaker noiseMaker;

    public override void Awake()
    {
        tankPawn = GetComponent<TankPawn>();
        noiseMaker = GetComponent<NoiseMaker>();
    }
    
    public override void Shoot()
    {
        Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        newBullet.InitBullet(tankPawn, firePoint.forward);
        newBullet.damage = (int)(tankPawn.PowerupManager.damageMultiplier * newBullet.damage);
        noiseMaker.shootingVolume = noiseMaker.shootingNoiseMultiplier;
    }
}
