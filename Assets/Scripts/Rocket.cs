using UnityEngine;
using System;
using System.Collections;

public class Rocket : MonoBehaviour
{
    private GameObject expFab;
    private float power;
    private float maxDamage;
    private Rigidbody myBody;

    public void Init(float power, float maxDamage, Vector3 position, Quaternion rotation, GameObject expFab)
    {
        this.expFab = expFab;
        this.power = power;
        this.maxDamage = maxDamage;
        transform.position = position;
        transform.rotation = rotation;

        myBody = GetComponent<Rigidbody>();
        myBody.AddForce(transform.forward * 50f * power, ForceMode.Impulse);

        StartCoroutine(WaitForGravity(power));
    }

    IEnumerator WaitForGravity(float power)
    {
        yield return new WaitForSeconds(power * 2f);
        //myBody.velocity = Vector3.zero;
        myBody.useGravity = true;
    }

    void OnCollisionEnter(Collision collider)
    {
        Array.ForEach(Physics.OverlapSphere(transform.position, 10f), col =>
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyShot(power * maxDamage * ((10f - Vector3.Distance(transform.position, col.transform.position)) / 10f));
            }

            Rigidbody rigid = col.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.AddExplosionForce(500f, transform.position, 10f);
            }
        });
        Destroy(gameObject);
        Instantiate(expFab, transform.position, Quaternion.identity);
    }
}
