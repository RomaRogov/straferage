using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance; //4 GUI

    public float Health;
    public bool OwnedByUser;
	
	void Start ()
    {
        if (OwnedByUser)
        {
            Instance = this;
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

    private void OnDied()
    {

    }
}
