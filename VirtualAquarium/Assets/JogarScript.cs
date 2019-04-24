using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class JogarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UdpClient udpclient = new UdpClient();

        IPAddress multicastaddress = IPAddress.Parse("227.55.77.99");
        udpclient.JoinMulticastGroup(multicastaddress);
        IPEndPoint remoteep = new IPEndPoint(multicastaddress, 5555);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
