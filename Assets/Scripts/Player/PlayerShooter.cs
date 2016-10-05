using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Should be on PlayerMover
public class PlayerShooter : MonoBehaviour
{
    public enum WeaponTypes { GUN = 0, SHOTGUN, MACHINE_GUN, ROCKET, SNIPER_GUN }

    public GameObject DustParticleFab;
    public Rocket RocketFab;
    public GameObject RocketExplosionFab;
    public Light WeaponSpark;
    public AudioSource PistolSound;
    public AudioSource ShotgunSound;
    public AudioSource RocketSound;
    public WeaponTypes SelectedWeapon;
    public float ZoomFOV = 20f;
    
    private Dictionary<WeaponTypes, float> shootingDelays = new Dictionary<WeaponTypes, float>() {
        { WeaponTypes.GUN, .5f },
        { WeaponTypes.SHOTGUN, 1f },
        { WeaponTypes.MACHINE_GUN, .2f },
        { WeaponTypes.ROCKET, 2f },
        { WeaponTypes.SNIPER_GUN, 2f }
    };
    private Dictionary<WeaponTypes, float> shootingPowers = new Dictionary<WeaponTypes, float>() {
        { WeaponTypes.GUN, 5f },
        { WeaponTypes.SHOTGUN, 5f },
        { WeaponTypes.MACHINE_GUN, 5f },
        { WeaponTypes.ROCKET, 50f },
        { WeaponTypes.SNIPER_GUN, 30f }
    };

    private static PlayerShooter instance;
    private Camera myCam;

    private bool fire = false;
    private float lastShootTime;
    private float normalFOV;
    private float targetFOV = 60f;

    void Awake()
    {
        instance = this;
        myCam = GetComponent<Camera>();
        normalFOV = myCam.fieldOfView;
    }

    void Update()
    {
        if (fire && ((lastShootTime + shootingDelays[SelectedWeapon]) < Time.time))
        {
            switch (SelectedWeapon)
            {
                case WeaponTypes.GUN:
                case WeaponTypes.MACHINE_GUN:
                    PistolSound.Play();
                    ShootWithRaycast(false, shootingPowers[SelectedWeapon]);
                    StartCoroutine(Spark());
                    lastShootTime = Time.time;
                    break;
                case WeaponTypes.SHOTGUN:
                    ShotgunSound.Play();
                    StartCoroutine(Spark());
                    for (int i = 0; i < 5; i++) { ShootWithRaycast(true, shootingPowers[SelectedWeapon]); }
                    lastShootTime = Time.time;
                    break;
                case WeaponTypes.ROCKET:
                    RocketSound.Play();
                    Instantiate(RocketFab).Init(shootingPowers[SelectedWeapon], transform.TransformPoint(Vector3.forward), transform.rotation, RocketExplosionFab);
                    lastShootTime = Time.time;
                    break;
                case WeaponTypes.SNIPER_GUN:
                    targetFOV = ZoomFOV;
                    fire = false;
                    break;
            }
        }

        myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, targetFOV, Time.deltaTime * 10f);
    }

    private void StartShootInternal()
    {
        fire = true;
        
        switch (SelectedWeapon)
        {
            case WeaponTypes.MACHINE_GUN: lastShootTime = Time.time; break; //Fake the first shot for machine gun
        }
    }

    private void EndShootInternal()
    {
        fire = false;
        if (SelectedWeapon == WeaponTypes.SNIPER_GUN)
        {
            targetFOV = normalFOV;
            if ((lastShootTime + shootingDelays[SelectedWeapon]) < Time.time)
            {
                PistolSound.Play();
                StartCoroutine(Spark());
                ShootWithRaycast(false, shootingPowers[SelectedWeapon]);
                lastShootTime = Time.time;
            }
        }
    }

    private void ShootWithRaycast(bool randomizeDirection, float power)
    {
        Vector3 direction = transform.forward + (randomizeDirection ? UnityEngine.Random.onUnitSphere * .05f : Vector3.zero);
        Array.ForEach(Physics.RaycastAll(transform.position, direction), hit =>
        {
            Enemy shotEnemy = (hit.rigidbody ? hit.rigidbody.GetComponent<Enemy>() : null);
            if (shotEnemy != null)
            {
                shotEnemy.ApplyShot(power, hit.point, direction);
            }
            Instantiate(instance.DustParticleFab, hit.point, Quaternion.identity);
        });
    }

    private void ChangeWeapon(int direction)
    {
        WeaponTypes[] arr = (WeaponTypes[])Enum.GetValues(typeof(WeaponTypes));
        if (((int)SelectedWeapon + direction) == (arr.Length))
        {
            SelectedWeapon = 0;
            return;
        }
        if (((int)SelectedWeapon + direction) < 0)
        {
            SelectedWeapon = arr[arr.Length - 1];
            return;
        }
        SelectedWeapon = arr[(int)SelectedWeapon + direction];
    }

    private IEnumerator Spark()
    {
        WeaponSpark.enabled = true;
        yield return new WaitForSeconds(.1f);
        WeaponSpark.enabled = false;
    }

    public static void StartShoot() { instance.StartShootInternal(); }
    public static void EndShoot() { instance.EndShootInternal(); }
    public static void NextWeapon() { instance.ChangeWeapon(1); }
    public static void PrevWeapon() { instance.ChangeWeapon(-1); }
}
