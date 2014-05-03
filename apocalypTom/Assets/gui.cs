using UnityEngine;
using System;
using System.Collections;

public class gui : MonoBehaviour {

	// Update is called once per frame
	void OnGUI() {
		GridManager gm = GameObject.Find ("Main Camera").GetComponent<GridManager>();
		Tom tom = GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>();

		GUI.TextArea(new Rect(5, Screen.height - 25, 85, 20), "Health: " + tom.getHealth().ToString());
		GUI.TextArea (new Rect(Screen.width - 90, Screen.height - 25, 85, 20), "Ammo: " + tom.getAmmo().ToString());
		GUI.TextArea (new Rect(5, 5, 85, 20), "Kills: " + tom.getKills().ToString());
		GUI.TextArea (new Rect(Screen.width - 90, 5, 85, 20), "Time: " + tom.getTime().ToString());

		//bool testButtonTwo = false;
		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>().getHealth() <= 0) 
		{
			GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
			myButtonStyle.fontSize = 75;

			myButtonStyle.normal.textColor = Color.red;
			myButtonStyle.hover.textColor = Color.red;

			int time = tom.getTime();

			if(GUI.Button(new Rect(Screen.width/2-400, Screen.height/2-100, 800, 200), "Game Over!\n Kills: " 
			              + tom.getKills ().ToString() + ", Time: " + time.ToString(), myButtonStyle))
				Application.LoadLevel("Z_scene");
		}
	}
}
