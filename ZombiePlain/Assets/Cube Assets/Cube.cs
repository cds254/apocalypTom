using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public float moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Horizontal") > 0) {
			transform.Translate (moveSpeed * Time.deltaTime, 0f, 0f);
		} else if (Input.GetAxis ("Horizontal") < 0) {
			transform.Translate (-moveSpeed * Time.deltaTime, 0f, 0f);
		}

		if (Input.GetAxis ("Vertical") > 0) {
			transform.Translate (0f, 0f, moveSpeed * Time.deltaTime);
		} else if (Input.GetAxis ("Vertical") < 0) {
			transform.Translate (0f, 0f, -moveSpeed * Time.deltaTime);
		}
	}
}
