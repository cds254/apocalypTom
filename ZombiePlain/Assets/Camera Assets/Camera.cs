using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public float xOffset;
	public float yOffset;
	public float zOffset;

	public float moveSpeed = 10;
	public float rotateSpeed = 10;

	private Vector3 startPoint;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		startPoint += new Vector3 (-xOffset, -yOffset, -zOffset);
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

		/*
		float angle = Mathf.Atan2(Input.mousePosition.y - Screen.height/2, Input.mousePosition.x - Screen.width/2) * Mathf.Rad2Deg;
		Vector3 currAngles = transform.eulerAngles;
		Quaternion target = Quaternion.Euler(currAngles.x, 180f+angle, currAngles.z);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 15f);
		*/
		
		if (Input.GetAxis ("Horizontal") > 0) {
			startPoint += new Vector3 (moveSpeed * Time.deltaTime, 0f, 0f);
		} else if (Input.GetAxis ("Horizontal") < 0) {
			startPoint += new Vector3 (-moveSpeed * Time.deltaTime, 0f, 0f);
		}
		
		if (Input.GetAxis ("Vertical") > 0) {
			startPoint += new Vector3 (0f, 0f, moveSpeed * Time.deltaTime);
		} else if (Input.GetAxis ("Vertical") < 0) {
			startPoint += new Vector3 (0f, 0f, -moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.X)) {
			startPoint += new Vector3 (0f, moveSpeed * Time.deltaTime, 0f);
		} else if (Input.GetKey (KeyCode.Z)) {
			startPoint += new Vector3 (0f, -moveSpeed * Time.deltaTime, 0f);
		}

		transform.position = startPoint + new Vector3(xOffset, yOffset, zOffset);
	}
}
