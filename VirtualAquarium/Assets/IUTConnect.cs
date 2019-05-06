using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class IUTConnect
{
    private UdpClient udpClient;
    IPEndPoint remoteEndPoint;
    IPEndPoint localEndPoint;
    Socket client;
   private const string CONNECTION_SUCCESSFULL_MESSAGE = "HELLO_DEVICE";

    private string multicastIP { get; set; }
    private int multicastPort { get; set; }
    private string tokenID { get; set; }
    private string moduleIP;

    public IUTConnect()
    {
        this.multicastIP = "227.55.77.99";
        this.multicastPort = 5000;
    }

    private void multicastCallback(IAsyncResult ar)
    {
        IPEndPoint sender = new IPEndPoint(0, 0);
        Byte[] receivedBytes = udpClient.EndReceive(ar, ref sender);
        Debug.Log(Encoding.ASCII.GetString(receivedBytes));        
        if (!CONNECTION_SUCCESSFULL_MESSAGE.Equals(Encoding.ASCII.GetString(receivedBytes)))
        {             
            udpClient.BeginReceive(new AsyncCallback(multicastCallback), null);
        }
        else
        {
            this.moduleIP = sender.Address.ToString();
            IUTModuleProperties.ModuleAddress = this.moduleIP;
            this.startSocket();
        }
    }

    private void sendMulticast(byte[] bufferToSend)
    {
        udpClient.Send(bufferToSend, bufferToSend.Length, remoteEndPoint);
    }

    private void startMulticast()
    {
        udpClient = new UdpClient();
        IPAddress multicastIPAddress = IPAddress.Parse(this.multicastIP);
        IPAddress localIPAddress = IPAddress.Any;
        localEndPoint = new IPEndPoint(localIPAddress, this.multicastPort);
        remoteEndPoint = new IPEndPoint(multicastIPAddress, this.multicastPort);

        udpClient.ExclusiveAddressUse = false;
        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udpClient.ExclusiveAddressUse = false;

        udpClient.Client.Bind(localEndPoint);
        udpClient.JoinMulticastGroup(multicastIPAddress, localIPAddress);

        udpClient.BeginReceive(new AsyncCallback(multicastCallback), null);
        this.sendMulticast(Encoding.ASCII.GetBytes(this.tokenID));
    }

    private void socketCallback(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
        Socket client = state.workSocket;

        int bytesRead = client.EndReceive(ar);        

        if (bytesRead > 0)
        {            
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

            Debug.Log(state.sb.ToString());

            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(socketCallback), state);
        }
        else
        {            
            if (state.sb.Length > 1)
            {
                Debug.Log(state.sb.ToString());
            }
            
        }
    }

    private void receiveSocket()
    {
        StateObject state = new StateObject();
        state.workSocket = client;
        
        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(socketCallback), state);
    }

    public void sendSocket(string message)
    {
        byte[] byteMessage = Encoding.ASCII.GetBytes(message);
        client.Send(byteMessage);
    }

    private void startSocket()
    {
        IPAddress moduleIPAddress = IPAddress.Parse(this.moduleIP);
        IPEndPoint moduleEndPoint = new IPEndPoint(moduleIPAddress, 8080);
        client = new Socket(moduleIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        client.Connect(moduleEndPoint);
        this.receiveSocket();        
        sendSocket("TESTE");
    }

    public void start(string multicastIP, int multicastPort, string tokenID)
    {
        this.multicastIP = multicastIP;
        this.multicastPort = multicastPort;
        this.start(tokenID);
    }

    public void start(string tokenID)
    {
        this.tokenID = tokenID;
        this.start();
    }

    public void start()
    {
        if (this.multicastIP != null && !this.multicastIP.Equals("") 
            && this.multicastPort > 0
            && this.tokenID != null && !this.tokenID.Equals(""))
        {
            this.startMulticast();            
        }        
    }
}