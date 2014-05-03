using UnityEngine;
using System.Collections;

public class lights : MonoBehaviour {
	public float xOffset;
	public float zOffset;

	private GameObject camera;
	
	// Update is called once per frame
	void LateUpdate () {
		if(camera) {
			transform.position = new Vector3(camera.transform.position.x + xOffset, 
			                                 transform.position.y,
			                                 camera.transform.position.z + zOffset);
		}
		else {
			camera = GameObject.FindGameObjectWithTag ("MainCamera");
		}
	}
}
