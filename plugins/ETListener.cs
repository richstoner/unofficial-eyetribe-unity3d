using UnityEngine;
using System;

using TETCSharpClient;
using TETCSharpClient.Data;

public class ETListener : IGazeUpdateListener
{
	public GazeData lastGazeData;
	
	// z value for lastgaze point indicates a valid gaze location is present (0 no, 1 yes);
	public Vector3 lastGazePoint = new Vector3(0,0,0);
	
	public ETListener ()
	{
		Debug.Log("Launch ET listener");
		
		var connectedOk = true;
		GazeManager.Instance.Activate(1, GazeManager.ClientMode.Push);
		GazeManager.Instance.AddGazeListener(this);
		
		if (!GazeManager.Instance.IsConnected)
		{
			Debug.Log("Eyetracking Server not started");
			
			//Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("EyeTracking Server not started")));
			connectedOk = false;
		}
		else if (!GazeManager.Instance.IsCalibrated)
		{
			Debug.Log("User is not calibrated");
			
			//Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("User is not calibrated")));
			connectedOk = false;
		}
		if (!connectedOk)
		{
			Debug.Log("Connection not ready");
			
			
			return;
		}
		
	}
	
	#region Undefined methods 
	
	public void startCalibration()
	{
		//GazeManager.Instance.CalibrationStart();
	}
	
	
	#endregion
	
	
	#region Listener methods

	public void OnScreenIndexChanged(int number)
	{
	}

	public void OnCalibrationStateChanged(bool val)
	{
		
		
	}
	
	public void OnGazeUpdate(GazeData gazeData)
	{
		var x = (int) Math.Round(gazeData.SmoothedCoordinates.X, 0);
		var y = (int) Math.Round(gazeData.SmoothedCoordinates.Y, 0);
		
		if (x == 0 & y == 0)
		{ 	
			lastGazePoint = new Vector3(x,y,0);
			return;
		}
		
		lastGazeData = gazeData;
		lastGazePoint = new Vector3(x,y,1);
		
		//Debug.Log(String.Format("{0} - {1}", x, y));
	}

#endregion
}
