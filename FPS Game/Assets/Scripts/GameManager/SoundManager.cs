using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance { get; set; }

    public AudioSource audioSource;
    [Header("Pistol")]
    public AudioClip pistolShotfire;
    public AudioClip pistolreloadClip;
    public AudioClip emptySoundClip;

    [Header("M14")]
    public AudioClip M14fireClip;
    public AudioClip M14ReloadClip;

    [Header("Throwable")]
    public AudioClip grenadeExplosionClip;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PistolShotSound(WeaponModel weapon )
    {
        
        switch (weapon)
        {
            case WeaponModel.Pistol:
                audioSource.PlayOneShot(pistolShotfire);
                break;
            case WeaponModel.M16:
                audioSource.PlayOneShot(M14fireClip);
                break;
        }

    }
    public void ReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                audioSource.PlayOneShot(pistolreloadClip);
                break;
            case WeaponModel.M16:
                audioSource.PlayOneShot(M14ReloadClip);
                break;
        }
    }
    public void EmptySoundClip()
    {
        audioSource.PlayOneShot(emptySoundClip);
    }

    public void GrenadeSound()
    {
        audioSource.PlayOneShot(grenadeExplosionClip);
    }

   
}
