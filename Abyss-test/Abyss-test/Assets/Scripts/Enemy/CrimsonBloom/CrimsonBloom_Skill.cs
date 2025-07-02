using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloom_Skill : MonoBehaviour
{
    [Header("Shooting star info")]
    public float starSpeed;
    public GameObject crimsonBullet1Prefab;
    public float bulletDamagePercent;

    [Header("Fire info")]
    public GameObject firePrefab;
    public float burnDelay;
    public float burnRadius;
    public float burnDamagePercent;

    [Header("Flash info")]
    [SerializeField] float flashCooldown;
    public GameObject flashScreen;

    [Header("Dark Orb info")]
    public float damage;
    public GameObject darkOrbPrefab;
    public float darkOrbDamagePercent;
    public float darkOrbSpeed;


    [Header("Healing info")]
    public float healCooldown;
    public float healCooldownTimer;

    [Header("AirFire info")]
    public float airFireCooldown;
    public float airFireCooldownTimer;

    [Header("Ground Fire info")]
    public float groundFireCooldown;
    public float groundFireCooldownTimer;


}
