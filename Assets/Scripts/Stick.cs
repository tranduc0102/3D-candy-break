using System;
using System.Collections;
using System.Collections.Generic;
using BreakCandy.Scripts;
using pooling;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private GameObject effect;
    public bool canDestroyCandy;
    private void Start()
    {
        effect = GameManager.instance.effectBreak;
    }
    private void OnTriggerEnter(Collider other)
    {
        Vector3 posSpawnEffect = other.gameObject.transform.position*1.2f;
        PoolingManager.Spawn(effect, posSpawnEffect, Quaternion.identity);
        Destroy(other.gameObject);
    }
}
