using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float damage;

    protected float expRadius;
    protected GameObject owner;

    public static T Fire<T>(float damage, Vector3 position, Quaternion rotation, GameObject owner) where T : Projectile
    {
        Projectile instance = (T)Instantiate(PrefabAccessor.GetPrefabByType<T>());

        instance.damage = damage;
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.owner = owner;
        instance.StartCoroutine(WaitForD)

        return (T)instance;
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
        AfterDestroy();
    }

    protected virtual void AfterDestroy()
    {
        //to override: explosion
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner) { return; }

        if (expRadius > 0)
        {
            Array.ForEach(Physics.OverlapSphere(transform.position, expRadius), col =>
            {
                float currentPower = damage * ((expRadius - Vector3.Distance(transform.position, col.transform.position)) / expRadius);

                Player player = col.GetComponent<Player>();
                if (player != null) { player.ApplyShot(currentPower); }

                Enemy enemy = col.GetComponent<Enemy>();
                if (enemy != null) { enemy.ApplyShot(currentPower); }

                Rigidbody rigid = col.GetComponent<Rigidbody>();
                if (rigid != null) { rigid.AddExplosionForce(500f, transform.position, expRadius); }
            });
        }
        else
        {
            Player player = collision.collider.GetComponent<Player>();
            if (player != null) { player.ApplyShot(damage); }

            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null) { enemy.ApplyShot(damage); }
        }
        Destroy(gameObject);
        AfterDestroy();
    }
}
