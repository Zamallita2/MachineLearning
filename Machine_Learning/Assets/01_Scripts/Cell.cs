using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private float time=10f;
    public void Setup(float timeToDes)
    {
        time = timeToDes;
    }
    void Start()
    {
        Destroy(gameObject, time);
    }

    void Update()
    {
        
    }
}
