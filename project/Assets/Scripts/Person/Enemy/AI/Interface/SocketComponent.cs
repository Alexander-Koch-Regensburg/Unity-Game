using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

public class SocketComponent : MonoBehaviour
{
    /// <summary>
	/// Whether socket should be opened when game starts
	/// </summary>
    private bool openSocket;

    public int port = 3000;

    public int backlog = 10;

    /// <summary>
	/// Interval for sending data in seconds
	/// </summary>
    public float sendInterval = 0.1f;

    private Socket socket;

	private Queue<string> _commandQueue = new Queue<string>();
    private object _asyncLock = new object();

    public Socket Socket {
        get {
            return this.socket;
        }
    }

    private IPAddress ip;
    public IPAddress IP
	{
		get
		{
			if (ip == null)
			{
				ip = (
					from entry in Dns.GetHostEntry (Dns.GetHostName ()).AddressList
						where entry.AddressFamily == AddressFamily.InterNetwork
							select entry
				).FirstOrDefault ();
			}

			return ip;
		}
	}

	private List<Socket> clients = new List<Socket> ();

	private static int bufferSize = 256;
	private byte[] buffer = new byte[bufferSize];

    void Awake() {
        if (MainMenuPlayerPreferences.OpenSocket) {
            OpenSocket();
        }        
    }

    public void OpenSocket() {
		Debug.Log ("Hosting at " + IP + ":" + port);
		socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			socket.Bind (new IPEndPoint (IP, port));
			socket.Listen (backlog);
			socket.BeginAccept (new System.AsyncCallback (OnClientConnect), socket);
			StartCoroutine(SendEnvironmentInfo());
		}
		catch (System.Exception e)
		{
			Debug.LogError ("Exception when attempting to host (" + port + "): " + e);

			socket = null;
		}
	}

    void OnClientConnect (System.IAsyncResult result)
	{
		try
		{
			Socket client = socket.EndAccept (result);
			clients.Add(client);
			BeginReceive(client);
		}
		catch (System.Exception e)
		{
			Debug.LogError ("Exception when accepting incoming connection: " + e);
		}

		try
		{
			socket.BeginAccept (new System.AsyncCallback (OnClientConnect), socket);
		}
		catch (System.Exception e)
		{
			Debug.LogError ("Exception when starting new accept process: " + e);
		}
	}

	void OnReceive (IAsyncResult result)
	{
		try
		{
			if (result.IsCompleted)
			{
				Socket client = (Socket) result.AsyncState;
				int bytesRead = client.EndReceive (result);

				if (bytesRead > 0)
				{
					byte[] read = new byte[bytesRead];
					Array.Copy (buffer, 0, read, 0, bytesRead);
					HandleInput(read, client);
					BeginReceive(client);
				}
				else
				{
					// Disconnect
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError ("Exception when receiving : " + e);
		}
	}

	void BeginReceive(Socket client) {
		client.BeginReceive (buffer, 0, bufferSize, SocketFlags.None, new AsyncCallback (OnReceive), client);
	}

	void HandleInput (byte[] data, Socket client) {
		string input = Encoding.ASCII.GetString (data, 0, data.Length);
		lock(_asyncLock)
        {
            _commandQueue.Enqueue(input);
        }
	}

	void SendDataToAll(string data) {
		foreach (Socket client in clients)
		{
			SendDataToOne(client, data);
		}
	}

	void SendDataToOne(Socket client, string data) {
		client.Send (Encoding.ASCII.GetBytes (data));
	}

    /// <summary>
	/// Routine for sending data
	/// </summary>
	private IEnumerator SendEnvironmentInfo() {
		while (true) {
			yield return new WaitForSeconds(sendInterval);
			string data = AIInterfaceComponent.instance.GetEnvironmentData();
			SendDataToAll(data);
		}
	} 
   
    private void Update()
    {
        if(_commandQueue.Count == 0) return;
       
        lock(_asyncLock)
        {
            foreach(var command in _commandQueue)
            {
                AIInterfaceComponent.instance.HandleCommand(command);
            }
            _commandQueue.Clear();
        }
    }
}
