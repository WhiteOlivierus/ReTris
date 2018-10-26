using UnityEngine;


public class Controller {


	public KeyCode keyLeft, keyRight, keyDown, keyRotate;


	public Controller (KeyCode l, KeyCode r, KeyCode d, KeyCode t) {
		keyLeft = l;
		keyRight = r;
		keyDown = d;
		keyRotate = t;
	}
}
