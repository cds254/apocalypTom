using UnityEngine;
using System;
using System.Collections;

public class gui : MonoBehaviour {

	// Update is called once per frame
	void OnGUI() {
		GridManager gm = GameObject.Find ("Main Camera").GetComponent<GridManager>();
		Tom tom = GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>();

		GUI.TextArea(new Rect(5, Screen.height - 110, 85, 20), "Health: " + tom.getHealth().ToString());
		GUI.TextArea(new Rect(Screen.width - 130, Screen.height - 30, 125, 20), "Mob Count: " + tom.getMobCount().ToString());
		GUI.TextArea (new Rect(5, Screen.height - 90, 85, 20), "Ammo: " + tom.getAmmo().ToString());
		GUI.TextArea (new Rect(5, Screen.height - 70, 85, 20), "Kills: " + tom.getKills().ToString());
		GUI.TextArea (new Rect(5, Screen.height - 50, 85, 30), "Weapon: " + tom.getWeapon());
		GUI.TextArea (new Rect(5, Screen.height - 20, 85, 20), "Time: " + tom.getTime().ToString());

		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>().getHealth() <= 0) 
		{
			GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
			myButtonStyle.fontSize = 75;

			myButtonStyle.normal.textColor = Color.red;
			myButtonStyle.hover.textColor = Color.red;

			bool testButtonTwo = GUI.Button(new Rect(Screen.width/4, Screen.height/2-100, 600, 200), "Game Over!", myButtonStyle);
			//GUI.TextArea (new Rect(Screen.width/2-100, Screen.height/2-100, 200, 200), "Game Over");

		}
	}
}
