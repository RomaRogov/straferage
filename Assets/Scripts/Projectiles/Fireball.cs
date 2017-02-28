using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{

    private Rigidbody myBody;

    private void Start()
    {
        expRadius = 2f;
        myBody = GetComponent<Rigidbody>();
        myBody.AddForce(transform.forward * 50f, ForceMode.Impulse);
    }

    protected override void AfterDestroy()
    {
        Instantiate(PrefabAccessor.Instance.FireballExplosion, transform.position, Quaternion.identity);
    }
}
