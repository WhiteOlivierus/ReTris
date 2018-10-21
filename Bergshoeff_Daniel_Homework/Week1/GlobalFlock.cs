using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {

    private GameObject fishPrefab;
    private Vector3 tankSize = Vector3.zero;
    private int numFish = 75;
    private GameObject[] allFish;
    private Vector3 goalPos = Vector3.zero;

    
    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(tankSize.x * 2, tankSize.y * 2, tankSize.z * 2));
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawSphere(goalPos, 0.1f);
    }


    // Use this for initialization
    private void Start() {
        GoalPos = transform.position;
        AllFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++) {
            Vector3 pos = new Vector3(Random.Range(gameObject.transform.position.x - tankSize.x, gameObject.transform.position.x + tankSize.x),
                                      Random.Range(gameObject.transform.position.y - tankSize.y, gameObject.transform.position.y + tankSize.y),
                                      Random.Range(gameObject.transform.position.z - tankSize.z, gameObject.transform.position.z + tankSize.z));
            AllFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            AllFish[i].GetComponent<Flock>().globalFlock = this;
        }
    }


    // Update is called once per frame
    private void Update() {
        if (Random.Range(0, 10000) < 50) {
            GoalPos = new Vector3(Random.Range(gameObject.transform.position.x - tankSize.x, gameObject.transform.position.x + tankSize.x),
                                        Random.Range(gameObject.transform.position.y - tankSize.y, gameObject.transform.position.y + tankSize.y),
                                        Random.Range(gameObject.transform.position.z - tankSize.z, gameObject.transform.position.z + tankSize.z));
        }
    }


    public GameObject FishPrefab
    {
        get { return fishPrefab; }
        set { fishPrefab = value; }
    }


    public Vector3 TankSize
    {
        get { return tankSize; }
        set { tankSize = value; }
    }


    public int NumFish
    {
        get { return numFish; }
        set { if (value >= 0) numFish = value; }
    }


    public GameObject[] AllFish
    {
        get { return allFish; }
        set { allFish = value; }
    }


    public Vector3 GoalPos
    {
        get { return goalPos; }
        set { goalPos = value; }
    }
}

