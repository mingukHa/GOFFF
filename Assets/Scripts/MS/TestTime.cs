using System;
using UnityEngine;

public class TestTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(DateTime.Now);
        Debug.Log(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }
}
