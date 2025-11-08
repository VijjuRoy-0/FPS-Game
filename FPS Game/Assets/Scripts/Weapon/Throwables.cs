using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    [SerializeField] float dealy = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionFroce = 1200f;

    float countDown;

    bool hasExploded = false;
    public bool hasbeenThrown= false;

    public enum ThrowableType { 
    
        None,
        Grenade,
        SmokeGrenade
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countDown = dealy;
    }

    private void Update()
    {
        if (hasbeenThrown)
        {
            countDown -= Time.deltaTime;

            if(countDown <=0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
         case ThrowableType.Grenade:
                GrenadeEffect();
                break;
         case ThrowableType.SmokeGrenade:
                SmokeGrenadeEffect();
                break;
        }

    }

    private void SmokeGrenadeEffect()
    {
        GameObject smokeEffect = GlobalReference.Instance.smokeGrenadeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        //Sound
        SoundManager.Instance.GrenadeSound();
        //PhysicalEffect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                
            }
        }
    }

    private void GrenadeEffect()
    {
        //VisualEffect
        GameObject explosionEffect = GlobalReference.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect,transform.position,transform.rotation);

        //Sound
        SoundManager.Instance.GrenadeSound();
        //PhysicalEffect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionFroce,transform.position,damageRadius);
            }
        }
    }
}
