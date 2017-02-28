using System;
using UnityEngine;

public class PrefabAccessor : MonoBehaviour
{
    public GameObject DustParticle;
    public GameObject RocketExplosion;
    public GameObject FireballExplosion;
    public MonoBehaviour[] TypedFabs;

    public static PrefabAccessor Instance;

    public static MonoBehaviour GetPrefabByType<T>()
    {
        return Array.Find(Instance.TypedFabs, x => x.GetType() == typeof(T));
    }

    void Start()
    {
        Instance = this;
    }
}
