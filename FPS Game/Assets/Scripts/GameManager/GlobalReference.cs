using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReference : MonoBehaviour
{
  public static GlobalReference Instance {  get; set; }

    public GameObject bulletImpactEffectPrefab;

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
