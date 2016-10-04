using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Should be on PlayerMover
public class PlayerShooter : MonoBehaviour
{
    public enum WeaponTypes { GUN = 0, SHOTGUN, MACHINE_GUN, SNIPER_GUN }

    public GameObject DustParticleFab;
    public WeaponTypes SelectedWeapon;
    public float ZoomFOV = 20f;

    private Dictionary<WeaponTypes, float> shootingDelays = new Dictionary<WeaponTypes, float>() {
        { WeaponTypes.GUN, .5f },
        { WeaponTypes.SHOTGUN, 1f },
        { WeaponTypes.MACHINE_GUN, .1f },
        { WeaponTypes.SNIPER_GUN, 0f }
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
                    ShootWithRaycast(false, 5f);
                    break;
                case WeaponTypes.SHOTGUN:
                    for (int i = 0; i < 5; i++) { ShootWithRaycast(true, 5f); }
                    break;
                case WeaponTypes.SNIPER_GUN:
                    ShootWithRaycast(false, 15f);
                    fire = false;
                    break;
            }
            lastShootTime = Time.time;
        }

        myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, targetFOV, Time.deltaTime * 10f);
    }

    private void StartShootInternal()
    {
        fire = true;
        
        switch (SelectedWeapon)
        {
            case WeaponTypes.MACHINE_GUN: lastShootTime = Time.time; break; //Fake the first shot for machine gun
            case WeaponTypes.SNIPER_GUN:
                targetFOV = ZoomFOV;
                fire = false;
                break;
        }
    }

    private void EndShootInternal()
    {
        fire = false;
        if (SelectedWeapon == WeaponTypes.SNIPER_GUN)
        {
            fire = true;
            targetFOV = normalFOV;
        }
    }

    private void ShootWithRaycast(bool randomizeDirection, float power)
    {
        Array.ForEach(Physics.RaycastAll(transform.position, transform.forward + (randomizeDirection ? UnityEngine.Random.onUnitSphere * .05f : Vector3.zero)), hit =>
        { Instantiate(instance.DustParticleFab, hit.point, Quaternion.identity); });
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

    public static void StartShoot() { instance.StartShootInternal(); }
    public static void EndShoot() { instance.EndShootInternal(); }
    public static void NextWeapon() { instance.ChangeWeapon(1); }
    public static void PrevWeapon() { instance.ChangeWeapon(-1); }
}
