using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	public GameObject prefabBlock;
	public GameObject prefabBorderBlock;
    public Material dropFasterCube;
    public Material blockRotationCube;
    public Material switchMovementCube;
    public static Material cubeDropFaster;
    public static Material cubeBlockRotation;
    public static Material cubeSwitchMovement;

    // Use this for initialization
    void Start () {
		if(cubeDropFaster == null)
        {
            cubeDropFaster = dropFasterCube;
        }
        if(cubeBlockRotation == null)
        {
            cubeBlockRotation = blockRotationCube;
        }
        if(cubeSwitchMovement == null)
        {
            cubeSwitchMovement = switchMovementCube;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
