using UnityEngine;
using System.Collections;

public class Tom : MonoBehaviour {

	public float moveSpeed = 10f;
	public float rotateSpeed = 10f;

	private float health = 100f;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void takeDamage (float damage) {
		health -= damage;

		Debug.Log ("Health: " + health.ToString());

		if (health <= 0f) {
			// DIE
			Debug.Log ("Dead.");
		}
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
		float xDistance = 0;
		float yDistance = 0;

		if (Input.GetAxis ("Horizontal") > 0) {
			xDistance = 1;
		} else if (Input.GetAxis ("Horizontal") < 0) {
			xDistance = -1;
		}
		
		if (Input.GetAxis ("Vertical") > 0) {
			yDistance = 1;
		} else if (Input.GetAxis ("Vertical") < 0) {
			yDistance = -1;
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

		Debug.Log (xDistance + ":" + yDistance + "\n");
		if (Input.GetMouseButtonDown(0))
		{
			anim.Play ("Strike");
		}
		
		if ((xDistance == 0) && (yDistance == 0)) {
						anim.Play ("Idle2");
				}

		if ((xDistance != 0) || (yDistance != 0)) 
		{
			anim.Play("Run2");
		}
		else
		{
			anim.Play("Idle2");
		}

		// rotate
		Vector3 moveVec = new Vector3 (xDistance, 0f, yDistance);
		moveVec.Normalize();
		moveVec *= moveSpeed * Time.deltaTime;
		transform.Translate (moveVec, Space.World);
	}
}
