using UnityEngine;
using System;
using System.Collections;

public class plainZombie : MonoBehaviour {
	public float moveSpeed = 3f;
	public float attackRange = 1.5f;
	public float despawnRange = 300f;
	public float health = 10f;

	private GameObject tom;
	private bool attacking = false;

	// Use this for initialization
	void Start () {
		tom = GameObject.FindGameObjectWithTag ("Player");		// Get player object
	}

	void Attack () {
		tom.GetComponent<Tom>().takeDamage(5f);
	}

	public void takeDamage (float damage) {
		health -= damage;
		
		Debug.Log ("Health: " + health.ToString());
		
		if (health <= 0f) {
			die();
		}
	}

	private void die () {
		tom.GetComponent<Tom>().decreaseMobCount();
		Destroy(this);
	}

	// Update is called once per frame
	void Update () {
		if (tom) {

			if(GameObject.FindGameObjectWithTag("Player").GetComponent<Tom>().getHealth() <= 0)
			{
				die ();
			}

			Vector3 playerLocation = tom.transform.position;
			Vector3 moveVector = new Vector3(playerLocation.x - transform.position.x, 0f,
			                                 playerLocation.z - transform.position.z);
			float deltaX = moveVector.x;
			float deltaZ = moveVector.z;

			float angle = Mathf.Atan2(deltaX, deltaZ) * Mathf.Rad2Deg;
			Vector3 currAngles = transform.eulerAngles;
			Quaternion target = Quaternion.Euler(currAngles.x, angle+90f, currAngles.z);
			transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 15f);

			if (moveVector.magnitude > despawnRange) {			// Despawn if zombie is too far away
				die();
			}
			if (moveVector.magnitude > attackRange) {
				if (attacking) {
					Debug.Log ("Canceling attack");
					CancelInvoke("Attack");
					attacking = false;
				}

				moveVector.Normalize ();
				moveVector *= moveSpeed * Time.deltaTime;
				transform.Translate (moveVector, Space.World);
			}
			else {
				if (!attacking) {
					// Start attacking immediatly, repeating every 1 seconds
					InvokeRepeating("Attack", 0f, 1f);
					Debug.Log ("Attacking");
					attacking = true;
				}
			}


		}
		else {
			tom = GameObject.FindGameObjectWithTag ("Player");
		}
	}
}