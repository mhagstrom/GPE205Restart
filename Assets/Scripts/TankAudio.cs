using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAudio : MonoBehaviour
{
    public AudioClip shootingClip;
    public AudioClip dyingClip;
    public AudioClip movingClip;

    public AudioSource oneShotSource;
    public AudioSource movingSource;

    public TankMovement tankMovement;
    public TankPawn tankPawn;
    public NoiseMaker noiseMaker;

    public float moveNoiseThresholdMax = 2f;

    private void Awake()
    {
        tankPawn = GetComponent<TankPawn>();
        noiseMaker = GetComponent<NoiseMaker>();

        movingSource.clip = movingClip;
        movingSource.Play();
    }

    private void Start()
    {
        tankPawn.Health.DeathEvent += Health_OnDeathEvent;
    }

    private void OnDestroy()
    {
        tankPawn.Health.DeathEvent -= Health_OnDeathEvent;
    }

    private void Health_OnDeathEvent()
    {
        DyingNoise();
    }

    private void Update()
    {
        movingSource.volume = Mathf.Lerp(0, moveNoiseThresholdMax, noiseMaker.movingVolume);
    }

    public void ShootingNoise()
    {
        oneShotSource.PlayOneShot(shootingClip);
    }

    public void DyingNoise()
    {
        oneShotSource.PlayOneShot(dyingClip);
    }

}
