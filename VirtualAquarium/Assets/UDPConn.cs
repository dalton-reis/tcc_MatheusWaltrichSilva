using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPConn {

    UdpClient udpClient;
    
    IPEndPoint remoteEP;


    public void createUDPConn(string ipAddress, int port)
    {
        udpClient = new UdpClient();

        IPAddress multicastAddress = IPAddress.Parse(ipAddress);
        udpClient.JoinMulticastGroup(multicastAddress);
        remoteEP = new IPEndPoint(multicastAddress, port);
    }

    public string receiveMessage()
    {
        int tries = 0;
        string result = "";
        do
        {
            result = Encoding.Unicode.GetString(udpClient.Receive(ref remoteEP));
        } while (tries < 10 || !"".Equals(result));
        return result;
    }

    public void sendMessage(string message)
    {
        udpClient.Ttl = 255;
        Byte[] buffer = Encoding.ASCII.GetBytes(message);
        udpClient.Send(buffer, buffer.Length, remoteEP);
    }
}
