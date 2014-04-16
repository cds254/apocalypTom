using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public float xOffset;
	public float yOffset;
	public float zOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		GameObject tom = GameObject.FindGameObjectWithTag ("Player");
		Vector3 tomPos = tom.transform.position;
		Debug.Log (tomPos.ToString());
		tomPos.Set (tomPos.x + xOffset, tomPos.y + yOffset, tomPos.z + zOffset);
		transform.position = tomPos;
		*/

		float angle = Mathf.Atan2(Input.mousePosition.y - Screen.height/2, Input.mousePosition.x - Screen.width/2) * Mathf.Rad2Deg;
		Vector3 currAngles = transform.eulerAngles;
		Quaternion target = Quaternion.Euler(currAngles.x, 180f+angle, currAngles.z);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 15f);
	}
}
