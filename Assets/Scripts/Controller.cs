﻿using UnityEngine;

public class Controller {

	public KeyCode keyLeft;
	public KeyCode keyRight;
	public KeyCode keyDown;
	public KeyCode keyRotate;

	public Controller (KeyCode l, KeyCode r, KeyCode d, KeyCode t) {
		keyLeft = l;
		keyRight = r;
		keyDown = d;
		keyRotate = t;
	}
}
