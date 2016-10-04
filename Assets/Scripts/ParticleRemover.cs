using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]
public class ParticleRemover : MonoBehaviour
{
    private ParticleSystem system;

	void Start ()
    {
        system = GetComponent<ParticleSystem>();
	}

	void Update ()
    {
	    if (!system.IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
