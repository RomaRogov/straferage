using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float Health;
	
	void Awake()
    {
        //myBody = GetComponent<Rigidbody>();
	}
	
	public void ApplyShot(float power, Vector3 position, Vector3 direction)
    {
        //myBody.AddForceAtPosition(direction * power * 30f, position);
        Health -= power;

        if (Health <= 0)
        {
            Health = 0;
            OnDied();
        }
    }

    public void ApplyShot(float power)
    {
        Health -= power;

        if (Health <= 0)
        {
            Health = 0;
            OnDied();
        }
    }

    protected virtual void OnDied()
    {

    }
}
