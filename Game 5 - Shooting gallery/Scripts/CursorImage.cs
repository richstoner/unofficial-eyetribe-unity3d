using UnityEngine;
using System.Collections;

public class CursorImage : MonoBehaviour {
	
	public Texture currentEyePosition;
	
	// Use this for initialization
	void Start () {
		
		Screen.showCursor = true;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		
				
		EyeTribeClient trackerScript = (EyeTribeClient )FindObjectOfType(typeof(EyeTribeClient ));
		
		Vector3 currentPosition= trackerScript.gazePosNormalY;

		Rect curpos = new Rect(currentPosition.x, currentPosition.y, currentEyePosition.width, currentEyePosition.height);
	
		if(currentPosition.z > 0){
		
			GUI.Label (curpos, currentEyePosition);
		}
	}
	
}
