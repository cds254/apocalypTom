using UnityEngine;
using System;
using System.Collections;

public class gui : MonoBehaviour {

	// Update is called once per frame
	void OnGUI() {
		GridManager gm = GameObject.Find ("Main Camera").GetComponent<GridManager>();
		Tom tom = GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>();

		GUI.TextArea(new Rect(5, Screen.height - 120, 85, 20), "Health: " + tom.getHealth().ToString());
		GUI.TextArea(new Rect(Screen.width - 130, Screen.height - 30, 125, 20), "Mob Count: " + tom.getMobCount().ToString());
		GUI.TextArea (new Rect(5, Screen.height - 100, 85, 20), "Ammo: " + tom.getAmmo().ToString());
		GUI.TextArea (new Rect(5, Screen.height - 80, 85, 20), "Kills: " + tom.getKills().ToString());
		GUI.TextArea (new Rect(5, Screen.height - 60, 85, 30), "Weapon: " + tom.getWeapon());
		GUI.TextArea (new Rect(5, Screen.height - 30, 85, 20), "Time: " + tom.getTime().ToString());
	}
}
