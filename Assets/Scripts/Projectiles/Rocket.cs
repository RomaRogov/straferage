using UnityEngine;
using System;
using System.Collections;

public class Rocket : Projectile
{
    private float power;
    private Rigidbody myBody;

    public void SetPower(float p)
    {
        power = p;
    }

    private void Start()
    {
        expRadius = 20f;
        damage *= power;

        myBody = GetComponent<Rigidbody>();
        myBody.AddForce(transform.forward * 50f * power, ForceMode.Impulse);

        StartCoroutine(WaitForGravity(power));
    }

    IEnumerator WaitForGravity(float power)
    {
        yield return new WaitForSeconds(power * 2f);
        myBody.useGravity = true;
    }

    protected override void AfterDestroy()
    {
        Instantiate(PrefabAccessor.Instance.RocketExplosion, transform.position, Quaternion.identity);
    }
}
