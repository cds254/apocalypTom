using UnityEngine;
using System.Collections;

public class Tom : MonoBehaviour {

	public float moveSpeed = 10f;
	public float rotateSpeed = 10f;
	public float spawnRadius = 50f;
	public int mobCap = 100;
	public int itemCap = 10;

	public Transform plainZombie;
	public Transform forestZombie;
	public Transform desertZombie;

	public Rigidbody projectile;

	public float bulletSpeed;

	private float health = 100f;
	private int mobCount = 0;
	private int itemCount = 0;
<<<<<<< HEAD
	private int bulletCount = 100;
=======
	private string weapon = "gun";
	private int ammo = 500;
	private int time = 0;
	private int counter = 0;
	private int kills = 0;
>>>>>>> e089c57af0beb4ff928fa9dbfd1e04eff988a9a5

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void takeDamage (float damage) {
		health -= damage;

		Debug.Log ("Health: " + health.ToString());

		if (health <= 0f) {
			die();
		}
	}

	public float getHealth () {
		return health;
	}

	public int getMobCount() {
		return mobCount;
	}

	public string getWeapon() {
		return weapon;
	}

	public int getAmmo() {
		return ammo;
	}

	public int getKills() {
		return kills;
	}

	public int getTime(){
			return time;
	}

	public void decreaseMobCount() {
		mobCount--;
	}

	private void die () {
		Debug.Log ("Dead.");
		Destroy(this);
	}

	private void trySpawnZombie() {
		if (mobCount < mobCap) {
			// pick a random point on a circle r=spawnRadius
			int deg = UnityEngine.Random.Range (0, 360);
			Vector3 circVector = new Vector3(spawnRadius, 0f, 0f);

			circVector = Quaternion.AngleAxis (deg, Vector3.up) * circVector;

			float x = transform.position.x + circVector.x;
			float z = transform.position.z + circVector.z;

			// Holy Shit Batman, can we get more verbose?
			switch (GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<GridManager>().getBiome (new Vector3(x, 0f, z))) {
				case 1:
					GameObject.Instantiate (plainZombie, new Vector3(x, 0f, z), Quaternion.Euler(270, 90, 0));
					break;
				case 2:
					GameObject.Instantiate (forestZombie, new Vector3(x, 0f, z), Quaternion.Euler(270, 90, 0));
					break;
				case 3:
					GameObject.Instantiate (desertZombie, new Vector3(x, 0f, z), Quaternion.Euler(270, 90, 0));
					break;
			}

			mobCount++;
		}
	}

	private void trySpawnItem() {
		if (itemCount < itemCap) {
		}
	}

	// Update is called once per frame
	void Update () {
		if (UnityEngine.Random.Range (0, 1000) < 5) {		// 0.5% chance to spawn a zombie per frame.
			trySpawnZombie();
		}

		if (UnityEngine.Random.Range (0, 10000) < 5) {		// 0.05% chance to spawn an item per frame.
			trySpawnItem();
		}

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

		//I didn't know if there was a built in time function so I just came up with this
		counter = counter + 1;
		if (counter >= 58) {
			time = time + 1;
			counter = 0;
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

		if (Input.GetMouseButtonDown(0))
		{
			if (bulletCount > 0) {
				anim.Play ("Strike");

				bulletCount--;

				Rigidbody bullet = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
				bullet.velocity = transform.TransformDirection(new Vector3(0, 0, bulletSpeed));

			}
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
