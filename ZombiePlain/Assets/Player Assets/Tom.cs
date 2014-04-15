using UnityEngine;
using System.Collections;

public class Tom : MonoBehaviour {

	public float moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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

		if (Input.GetKey (KeyCode.X)) {
			transform.Translate (0f, 0f, moveSpeed * Time.deltaTime);
		} else if (Input.GetKey (KeyCode.Z)) {
			transform.Translate (0f, 0f, -moveSpeed * Time.deltaTime);
		}
	}
}
