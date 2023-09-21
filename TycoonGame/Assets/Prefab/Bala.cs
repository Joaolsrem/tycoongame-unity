using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public int speedBullet;
    void Update()
    {
        
        Destroy(gameObject, 1f);
    }
}
