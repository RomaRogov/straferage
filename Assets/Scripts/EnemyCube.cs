using UnityEngine;
using System;
using System.Collections;

public class EnemyCube : Enemy {

    public Transform[] WayPoints;

    private Rigidbody rbd;
    private Transform currentPoint { get { return WayPoints[currentPointIndex]; } }
    private int currentPointIndex = 0;
	
	void Start ()
    {
        rbd = GetComponent<Rigidbody>();
	}

    void Update()
    {
        Vector3 dir = (currentPoint.position - transform.position).normalized;
        dir.y = 0f;
        rbd.AddForce(dir * 20f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col == currentPoint.GetComponent<Collider>())
        {
            currentPointIndex++;
            if (currentPointIndex >= WayPoints.Length)
            {
                currentPointIndex = 0;
            }
        }
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
