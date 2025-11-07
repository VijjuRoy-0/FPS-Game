using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{


    [Header("Shooting")]
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowRest = true;
    public float shootingDealy = 2f;
    public bool isWeaponActive;
    bool isADS;

    [Header("Burst")]
    //Brust
    public int bulletsPerBurst = 3;
    public int BurstBulletsLeft;

    [Header("Spread")]
    //Spread
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpwanPoint;
    public float bulletvelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzelEffect;
    internal Animator animator;

    [Header("Loading")]
    public float reloadTime;
    public int magazinesize, bulletsLeft;
    public bool isReloading;

    public Vector3 spwanPosition;
    public Vector3 spwanRotation;

    public enum ShootingMode
    {
        Single,
        Brust,
        Auto
    }
    public enum WeaponModel
    {
        Pistol,
        M16
    }
    public WeaponModel thisWeaponmodel;

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        BurstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazinesize;

        spreadIntensity = hipSpreadIntensity;
    }
    void Update()
    {
        if (isWeaponActive)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }


            GetComponent<Outline>().enabled = false;
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.EmptySoundClip();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //Holding Down left Mouse Button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single ||
                 currentShootingMode == ShootingMode.Brust)
            {
                // clicking left mouse once
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
            if (Input.GetKey(KeyCode.R) && bulletsLeft < magazinesize && !isReloading && WeaponManager.Instance.CheckAmmoLeftfor(thisWeaponmodel)>0)
            {
                Reload();
            }
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                // Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                BurstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
           
        }
    }

    public void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }
    public void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }



    private void FireWeapon()
    {
        bulletsLeft--;

       

        muzzelEffect.GetComponent<ParticleSystem>().Play();

        if (isADS)
        {
            animator.SetTrigger("ADS_Recoil");
        }
        else
        {
            animator.SetTrigger("Recoil");
        }

        SoundManager.Instance.PistolShotSound(thisWeaponmodel);
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
    private void Reload()
    {
        animator.SetTrigger("Reload");
        SoundManager.Instance.ReloadSound(thisWeaponmodel);
        isReloading = true;
        Invoke("ReloadCompleted",reloadTime);
    }
    private void ReloadCompleted()
    {
       if(WeaponManager.Instance.CheckAmmoLeftfor(thisWeaponmodel)>magazinesize)
        {
            bulletsLeft = magazinesize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponmodel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftfor(thisWeaponmodel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft,thisWeaponmodel);
        }

        isReloading = false;
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

        float z = UnityEngine.Random.Range(-spreadIntensity,spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // returing the shooting direction to spread
        return direction + new Vector3(0, y, z);
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
       yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
