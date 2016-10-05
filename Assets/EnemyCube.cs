using UnityEngine;
using System;
using System.Collections;

public class EnemyCube : Enemy {

	
	void Start ()
    {
	
	}

    protected override void OnDied()
    {
        Destroy(myBody);
        Destroy(GetComponent<BoxCollider>());
        Array.ForEach(GetComponentsInChildren<BoxCollider>(), col => col.enabled = true);
        Array.ForEach(GetComponentsInChildren<Rigidbody>(), r => {
            r.isKinematic = false;
            r.AddExplosionForce(100f, transform.position, 10f);
        });
    }
}
