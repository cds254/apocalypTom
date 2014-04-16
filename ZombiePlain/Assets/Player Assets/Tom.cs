using UnityEngine;
using System.Collections;

public class Tom : MonoBehaviour {

	public float moveSpeed = 10;
	public float rotateSpeed = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetAxis ("Horizontal") > 0) {
			transform.Translate (-moveSpeed * Time.deltaTime, 0f, 0f);
		} else if (Input.GetAxis ("Horizontal") < 0) {
			transform.Translate (moveSpeed * Time.deltaTime, 0f, 0f);
		}

		if (Input.GetAxis ("Vertical") > 0) {
			transform.Translate (0f, moveSpeed * Time.deltaTime, 0f);
		} else if (Input.GetAxis ("Vertical") < 0) {
			transform.Translate (0f, -moveSpeed * Time.deltaTime, 0f);
		}
		*/

		if (Input.GetAxis ("Horizontal") > 0) {
			transform.Translate (0f, -moveSpeed * Time.deltaTime, 0f);
		} else if (Input.GetAxis ("Horizontal") < 0) {
			transform.Translate (0f, moveSpeed * Time.deltaTime, 0f);
		}
		
		if (Input.GetAxis ("Vertical") > 0) {
			transform.Translate (-moveSpeed * Time.deltaTime, 0f, 0f);
		} else if (Input.GetAxis ("Vertical") < 0) {
			transform.Translate (moveSpeed * Time.deltaTime, 0f, 0f);
		}

		if (Input.GetKey (KeyCode.X)) {
			transform.Translate (0f, 0f, moveSpeed * Time.deltaTime);
		} else if (Input.GetKey (KeyCode.Z)) {
			transform.Translate (0f, 0f, -moveSpeed * Time.deltaTime);
		}

		float angle = Mathf.Atan2(Input.mousePosition.y - Screen.height/2, Input.mousePosition.x - Screen.width/2) * Mathf.Rad2Deg;
		Vector3 currAngles = transform.eulerAngles;
		Quaternion target = Quaternion.Euler(currAngles.x, 180f-angle, currAngles.z);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 15f);
	}
}
