/*
* ==============================================================================
*
* Filename: UDPCom
* Description: 
*
* Version: 1.0
* Created: 2017 05
* Compiler: Visual Studio 2010
*
* Author: RuiZhao
* Company: game95
*
* ==============================================================================
*/

using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

class UdpState
{
    public IPEndPoint e;
    public UdpClient u;
}


public class UDPCom : MonoBehaviour
{
    private static UDPCom _instance;
    public static UDPCom Instance { get { return _instance; } }

    private UdpState _s = new UdpState();

    private IPAddress _ip;
    private int _port;

    private UdpClient _client = null;

    void Awake()
    {
        _instance = this;
    }

    private bool _v6 = false;
    public void SetUDPInfo(string ip, int port)
    {
        IPAddress temp = null;
        try
        {
            if (!IPAddress.TryParse(ip, out temp))
            {
                IPAddress[] addresses = Dns.GetHostEntry(ip).AddressList;
                foreach (var item in addresses)
                {
                    if (item.AddressFamily == AddressFamily.InterNetwork)
                    {
                        temp = item;
                        Debug.Log("U1 V4");
                        _v6 = false;
                        _client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
                        break;
                    }
                    else if (item.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        temp = item;
                        Debug.Log("U1 V6");
                        _v6 = true;
                        _client = new UdpClient(new IPEndPoint(IPAddress.IPv6Any, 0));
                        break;
                    }
                }
            }
            else
            {
                if (temp.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.Log("U2 V4");
                    _v6 = false;
                    _client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
                }
                else if (temp.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    Debug.Log("U2 V6");
                    _v6 = true;
                    _client = new UdpClient(new IPEndPoint(IPAddress.IPv6Any, 0));
                }
            }
        }
        catch (Exception e)
        {
            return;
        }
        _ip = temp;
        _port = port;
        _client.Connect(_ip, port);

        ReceiveMessages();
    }

    public void Send(string info)
    {
        if (Logic.Instance != null && Logic.Instance.IsGameEnd)
        {
            return;
        }
        byte[] buffer = Encoding.ASCII.GetBytes(info);
        _client.Send(buffer, buffer.Length);
    }

    void FixedUpdate()
    {
        int count = _queueUDP.Count;
        if(count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Byte[] receiveBytes = (Byte[])(_queueUDP.Dequeue());
                // here change these code to you wanted

                if (receiveBytes != null && receiveBytes.Length > 10)
                {
                    string receiveString = Encoding.ASCII.GetString(receiveBytes);

                    //Debug.Log("Receive UDP:" + receiveString.Length + " | " + receiveString);
                    string uid = receiveString.Substring(5, receiveString.IndexOf(",") - 5);

                    int begin = receiveString.IndexOf(",\"x\":\"");
                    if (begin > 0)
                    {
                        int end = receiveString.LastIndexOf("\"");
                        if (end > 0 && end > begin)
                        {
                            string info = receiveString.Substring(begin + 6, end - begin - 6);
                            if (info != null && info.Length > 0)
                            {
                                Logic.AddInfo(uid, info);
                            }
                        }
                    }
                }
            }
        }
    }

    private static Queue _queueUDP = new Queue();

    public void ReceiveCallback(IAsyncResult ar)
    {
        UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
        IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

        Byte[] receiveBytes = u.EndReceive(ar, ref e);

        _queueUDP.Enqueue(receiveBytes);

        ReceiveMessages();
    }

    public void ReceiveMessages()
    {
        // Receive a message and write it to the console.
        IPEndPoint e = new IPEndPoint(_v6? IPAddress.IPv6Any : IPAddress.Any, 0);
        UdpClient u = new UdpClient(e);

        UdpState s = new UdpState();
        s.e = e;
        s.u = _client;

        _client.BeginReceive(new AsyncCallback(ReceiveCallback), s);
    }

    void OnDestroy()
    {
        if(_client != null)
            _client.Close();
    }
}
