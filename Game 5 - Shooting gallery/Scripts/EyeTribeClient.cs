using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class EyeTribeClient : MonoBehaviour {

	public Vector3 gazePosNormalY = new Vector3(Screen.width/2, Screen.height/2, 0);
	public Vector3 gazePosInvertY = new Vector3(Screen.width/2, Screen.height/2, 0);
	
	private ETListener listener;
	
	// Use this for initialization
	void Start () {
		try
		{
			listener = new ETListener();			
		}
		catch( SocketException e )
		{
			print(e);
		}
	}
	
	public void startCalibration() {
	
		Debug.Log("Sending Start calibration");
		
		try 
		{
			listener.startCalibration();
		}
		catch
		{
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 lastGazePoint = listener.lastGazePoint;	
		gazePosNormalY = lastGazePoint;
		gazePosInvertY = new Vector3(lastGazePoint.x, Screen.height - lastGazePoint.y, lastGazePoint.z);
		
	}
}
