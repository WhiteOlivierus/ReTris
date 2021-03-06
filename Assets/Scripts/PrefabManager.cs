﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made by Daniel Bergshoeff
public class PrefabManager : MonoBehaviour {


    public GameObject prefabBlock;
    public GameObject prefabBorderBlock;
    public Material dropFasterCube;
    public Material blockRotationCube;
    public Material switchMovementCube;
    public static Material cubeDropFaster;
    public static Material cubeBlockRotation;
    public static Material cubeSwitchMovement;


    void Start () {
        if (cubeDropFaster == null) {
            cubeDropFaster = dropFasterCube;
        }

        if (cubeBlockRotation == null) {
            cubeBlockRotation = blockRotationCube;
        }

        if (cubeSwitchMovement == null) {
            cubeSwitchMovement = switchMovementCube;
        }
    }
}
