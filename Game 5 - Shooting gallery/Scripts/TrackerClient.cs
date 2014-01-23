using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class TrackerClient : MonoBehaviour {
	
	public string address = "127.0.0.1";
	public int port = 4242;
	public Vector2 gazePosNormalY = new Vector2(Screen.width/2, Screen.height/2);
	public Vector2 gazePosInvertY = new Vector2(Screen.width/2, Screen.height/2);
	public Vector2 gazePosNormalYAverage = new Vector2();
	public bool gazePosValid = false;
	public int width = Screen.width;
	public int height = Screen.height;
	
	private Queue<Vector2> gazePosNormalAveraging = new Queue<Vector2>();
	private Vector2 gazePosNormalYAveringTemp = new Vector2();
	
	private bool _calibrating = false;
	
	private TcpClient _client;
	private IPEndPoint _receivePoint;
	private string _connectionParams = "<SET ID=\"ENABLE_SEND_DATA\" STATE=\"1\"/>\r\n" +
										"<SET ID=\"ENABLE_SEND_POG_BEST\" STATE=\"1\"/>\r\n" +
										"<SET ID=\"ENABLE_SEND_POG_LEFT\" STATE=\"1\"/>\r\n" + 
										"<SET ID=\"ENABLE_SEND_POG_RIGHT\" STATE=\"1\"/>\r\n";
	private string _startCalib = "<SET ID=\"CALIBRATE_SHOW\" STATE=\"1\" />\r\n" +
									"<SET ID=\"CALIBRATE_START\" STATE=\"1\" />\r\n";
		private string _stopCalib = "<SET ID=\"CALIBRATE_SHOW\" STATE=\"0\" />\r\n" +
									"<SET ID=\"CALIBRATE_START\" STATE=\"0\" />\r\n";
	private string _checkCalib = "<GET ID=\"CALIBRATE_SHOW\" />\r\n" +
									"<GET ID=\"CALIBRATE_START\" />\r\n";
	
	// Use this for initialization
	void Start () {
		try
		{
			_client = new TcpClient(address,System.Convert.ToInt32(port));
			ASCIIEncoding  encoding = new ASCIIEncoding();
	    	byte[] myBytes = encoding.GetBytes(_connectionParams);
			_client.GetStream().Write(myBytes,0,sizeof(byte)*_connectionParams.Length);
		}
		catch( SocketException e )
		{
			print(e);
		}
		
	}
	
	public void StartCalib()
	{
		if( !CheckCalib() )
		{
			if(_client != null)
			{
				ASCIIEncoding  encoding = new ASCIIEncoding();
				byte[] myBytes = encoding.GetBytes(_startCalib);
				_client.GetStream().Write(myBytes,0,sizeof(byte)*_startCalib.Length);
				_calibrating = true;
			}
		}
		else {
			if(_client != null)
			{
				ASCIIEncoding  encoding = new ASCIIEncoding();
				byte[] myBytes = encoding.GetBytes(_stopCalib);
				_client.GetStream().Write(myBytes,0,sizeof(byte)*_stopCalib.Length);
				_calibrating = false;
			}
		}
	}
	
	public bool CheckCalib()
	{
		if(_client != null)
		{
			ASCIIEncoding  encoding = new ASCIIEncoding();
			byte[] myBytes = encoding.GetBytes(_checkCalib);
			_client.GetStream().Write(myBytes,0,sizeof(byte)*_checkCalib.Length);
		}
		return _calibrating;
	}
	
	// Update is called once per frame
	void Update () {
		width = Screen.width;
		height = Screen.height;
		gazePosNormalY = new Vector2(width/2, height/2);
		gazePosInvertY = gazePosNormalY;
		
		if(_client != null)
		{
			int size = _client.ReceiveBufferSize;
			byte[] received = new byte[size];
			_client.GetStream().Read(received,0,size*sizeof(byte));
			ASCIIEncoding  encoding = new ASCIIEncoding();
			string xmlString = encoding.GetString(received);
			Debug.Log(xmlString);
			xmlString = xmlString.Substring(0,xmlString.LastIndexOf(">")+1);
			xmlString = "<?xml version=\"1.0\"?><doc>" + xmlString;
			xmlString = xmlString.Insert(xmlString.LastIndexOf(">")+1,"</doc>");
			XmlDocument doc = new XmlDocument();
			
			try
			{
				doc.LoadXml(xmlString);
			}
			catch(XmlException e)
			{
				print(e);
				print(xmlString);
				print(xmlString.Length);
			}
			
			XmlNode trackerNode = doc.SelectSingleNode("doc");
			if(trackerNode != null && trackerNode.HasChildNodes)
			{
				trackerNode = trackerNode.LastChild;
			}
			
			if(trackerNode != null && trackerNode.Name == "ACK")
			{
				string id = trackerNode.Attributes["ID"].Value;
				string state =  trackerNode.Attributes["STATE"].Value;
				
				if(id == "CALIBRATE_START")
				{
					_calibrating = state.Equals("1");
				}
			}
			if(trackerNode != null && trackerNode.Name == "CALIB_RESULT_SUMMARY")
			{
				_calibrating = false;
			}
			
	
			if(trackerNode != null && trackerNode.Name == "REC")
			{	
				string pogX = trackerNode.Attributes["BPOGX"].Value;
				string pogY = trackerNode.Attributes["BPOGY"].Value;
				string pogV = trackerNode.Attributes["BPOGV"].Value;
				gazePosValid = pogV.Equals("1")?true:false;
				if(gazePosValid)
				{
					gazePosNormalY = new Vector2((float.Parse(pogX) * width), (float.Parse(pogY)*height)-25);
					gazePosInvertY = new Vector2((float.Parse(pogX) * width), (height - float.Parse(pogY)*height)-25);
					gazePosNormalAveraging.Enqueue(gazePosNormalY);
					gazePosNormalYAveringTemp += gazePosNormalY;
					if(gazePosNormalAveraging.Count > 10)
					{
						Vector2 old = gazePosNormalAveraging.Dequeue();
						gazePosNormalYAveringTemp-= old;
						gazePosNormalYAverage = gazePosNormalYAveringTemp/gazePosNormalAveraging.Count;
					}
				}
			}
		}
	}
}
