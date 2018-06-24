using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour {

	GameObject f1;
	GameObject f2;


	// Use this for initialization
	void Start () {

		Debug.Log("GameEngine START");

		f1 = GameObject.Find ("Fighter1");
		f2 = GameObject.Find ("Fighter2");
     
    }
	
	// Update is called once per frame
	void Update () {
    

    }

	void FixedUpdate () {
		//Проведение боя
	}
}
