using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool multi, iot;
    public string server;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
}
