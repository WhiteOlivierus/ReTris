using UnityEngine;

//Made by Bas Dijkstra
public class Controller {


	public KeyCode keyLeft, keyRight, keyDown, keyRotate;


	public Controller (KeyCode l, KeyCode r, KeyCode d, KeyCode t) {
		keyLeft = l;
		keyRight = r;
		keyDown = d;
		keyRotate = t;
	}
}
