using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    

    //Shooting
    public bool isShooting, readyToShoot;
    bool allowRest = true;
    public float shootingDealy = 2f;

    //Brust
    public int bulletsPerBurst = 3;
    public int BurstBulletsLeft;

    //Spread
    public float spreadIntensity;

    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpwanPoint;
    public float bulletvelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzelEffect;

    Animator animator;

    public enum ShootingMode
    {
        Single,
        Brust,
        Auto
    }
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        BurstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
       if(currentShootingMode == ShootingMode.Auto)
        {
            //Holding Down left Mouse Button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
       else if(currentShootingMode == ShootingMode.Single ||
            currentShootingMode == ShootingMode.Brust)
        {
            // clicking left mouse once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
       if(readyToShoot && isShooting)
        {
            BurstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        animator.SetTrigger("Recoil");
        muzzelEffect.GetComponent<ParticleSystem>().Play();
        SoundManager.Instance.audioSource.Play();
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab,bulletSpwanPoint.position,Quaternion.identity);
       
        //Pointing the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;
       
        // Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletvelocity,ForceMode.Impulse);
        
        //Destroy the bullet after some time.
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

        //check if we are done shooting
        if (allowRest)
        {
            Invoke("ResetShot", shootingDealy);
            allowRest = false;
        }
        if(currentShootingMode == ShootingMode.Brust && BurstBulletsLeft > 1)
        {
            BurstBulletsLeft--;
            Invoke("FireWeapon",shootingDealy);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowRest =true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if(Physics.Raycast(ray, out  hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpwanPoint.position;

        float x = UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // returing the shooting direction to spread
        return direction + new Vector3(x, y, 0);
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
       yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
