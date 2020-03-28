using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSegue : MonoBehaviour
{
    public GameObject alvo;

    public GameObject cameraposicao;
    //public GameObject[] posicoes;
    //private int indice = 0;
    //public float velocidadeMovimento = 2;
    //private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(alvo.transform);
        //transform.position = Vector3.Lerp(transform.position,posicoes[indice].transform.position,velocidadeMovimento);        
        transform.position = cameraposicao.transform.position;
    }
    
}
