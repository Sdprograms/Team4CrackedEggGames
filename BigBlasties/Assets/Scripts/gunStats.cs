using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public GameObject gunModel;
    public GameObject projectile;
    public AudioClip gunSound;
    public float shootRate;
    public int ammoCurrent;
    public int ammoMax;
    public int ammoReserve;
}