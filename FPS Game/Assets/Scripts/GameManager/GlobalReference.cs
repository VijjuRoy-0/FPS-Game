using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReference : MonoBehaviour
{
  public static GlobalReference Instance {  get; set; }

    public GameObject bulletImpactEffectPrefab;

    public GameObject grenadeExplosionEffect;

    public GameObject smokeGrenadeEffect;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
         else{
            Destroy(gameObject);
        }
    }
}
