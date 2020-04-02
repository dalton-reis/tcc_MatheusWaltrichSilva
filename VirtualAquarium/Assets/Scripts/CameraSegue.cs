using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSegue : MonoBehaviour
{
    public GameObject alvo;

    public GameObject cameraposicao;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(alvo.transform);
        transform.position = cameraposicao.transform.position;
    }


    IEnumerator LoadVR()
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName("cardboard");
        yield return null;
        UnityEngine.XR.XRSettings.enabled = true;
    }
    
}
