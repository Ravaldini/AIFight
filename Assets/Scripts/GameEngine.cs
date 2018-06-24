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

        f1.GetComponent<Fighter>().LoadStyle("Style1.txt");
        f1.GetComponent<Fighter>().FighterListing();

        f2.GetComponent<Fighter>().LoadStyle("Style2.txt");
        f2.GetComponent<Fighter>().FighterListing();
    }
	
	// Update is called once per frame
	void Update () {
    

    }

	void FixedUpdate () {
		//Проведение боя
	}
}
