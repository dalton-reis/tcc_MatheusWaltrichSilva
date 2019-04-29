using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalPanel : MonoBehaviour {

    public Button jogarButton;

	// Use this for initialization
	void Start () {
        jogarButton.onClick.AddListener(jogarButtonFunc);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void jogarButtonFunc()
    {
        UDPConn udpServer = new UDPConn();
        udpServer.createUDPConn("227.55.77.99", 5000);
        udpServer.sendMessage("AQUARIUM_01");
        Debug.Log(udpServer.receiveMessage());
        Debug.Log("Enviado pelo Unity");
    }
}
