using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager SP;

    private ArrayList objectsList;
    private float moveSpeed = 1.5f;
    private int spawnedObjects = 0;
    private int score;
	
	
	void Awake () {
        SP = this;

        objectsList = new ArrayList();
        spawnedObjects = score =0;
		
		
	}

    void Update()
    {
        //Move objects
        for (int i = objectsList.Count - 1; i >= 0; i--)
        {
            float farLeft = -10;
            float farRight = 10;

            MovingObject movObj = (MovingObject)objectsList[i];
			
			float newR = (float)(movObj.hitPoints / 25.0);
			
			movObj.transform.gameObject.renderer.materials[0].color = new Color(newR,0,0);
			movObj.transform.gameObject.renderer.materials[1].color = new Color(newR,newR,newR);
			Debug.Log(movObj.transform.gameObject);
			
            Transform trans = movObj.transform;
            trans.Translate((int)movObj.direction * Time.deltaTime * moveSpeed, 0, 0);
			
            if (trans.position.x < farLeft || trans.position.x > farRight)
            {
                Destroy(trans.gameObject);
                objectsList.Remove(movObj);
            }
        }
    }

    void OnGUI(){
        if(GUILayout.Button("Restart")){
            Application.LoadLevel(Application.loadedLevel);
        }
        GUILayout.Label(" Hit: " + score + "/" + spawnedObjects);
    }

    public void AddTarget(MovingObject newObj){
        spawnedObjects++;
        objectsList.Add(newObj);
    }

    public bool RemoveObject(Transform trans)
    {
        
        foreach (MovingObject obj in objectsList)
        {
            if (obj.transform == trans)
            {
				if (obj.hitPoints == 0) {
					
					score++;
	                objectsList.Remove(obj);
	                Destroy(obj.transform.gameObject); 
	
				}
				else
				{
					obj.hitPoints --;
					Debug.Log(obj.hitPoints);
				}
				
                return true; 
            }
        }
        Debug.LogError("ERROR: Couldn't find target!");
        return false;
    }
	
	

}
