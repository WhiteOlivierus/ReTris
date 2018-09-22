using UnityEngine;

public class Controller : MonoBehaviour {

	public KeyCode keyLeft;
	public KeyCode keyRight;
	public KeyCode keyDown;
	public KeyCode keyRotate;


	void Start () {
		DontDestroyOnLoad (gameObject);
	}


	public void SetKeys (KeyCode [] keys) {
		keyLeft = keys [0];
		keyRight = keys [1];
		keyDown = keys [2];
		keyRotate = keys [3];
	}
}
