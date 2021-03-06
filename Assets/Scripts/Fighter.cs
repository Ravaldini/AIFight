﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class Fighter : MonoBehaviour {

    GameObject enemy;
    Animator anim;    

    public string nick = "none";
    public string style = "none";

    public int stamina = 100;
    public float speed = 0.0f;    
    public bool knockdown = false;
    public bool knockout = false;

    public string nextMove = "idle";
    public string currentMove = "idle";
    public string lastMove = "idle";

    public float distanceToEnemy = 6.0f;
    public string enemyMove = "idle";

    public List<Move> moves = new List<Move>();




	// Use this for initialization
	void Start () {        

        if (this.name == "Fighter1") enemy = GameObject.Find("Fighter2");
        if (this.name == "Fighter2") enemy = GameObject.Find("Fighter1");

        if (this.name == "Fighter1") LoadStyle("Style1.txt");
        if (this.name == "Fighter2") LoadStyle("Style2.txt");
        
        anim = GetComponent<Animator>();
                
        StartCoroutine(MakeDecision());

        //FighterListing();

    }





    public void LoadStyle(string fileName)
    {

        StreamReader str = new StreamReader(fileName, Encoding.UTF8);
        int i = 0;
        int iParse;
        float fParse;

        string st = str.ReadLine();
        if (st.StartsWith("nick")) nick = st.Substring(st.IndexOf("=") + 2);
        st = str.ReadLine();
        if (st.StartsWith("style")) style = st.Substring(st.IndexOf("=") + 2);

        while (!str.EndOfStream)
        {            

                st = str.ReadLine();
            if (st.StartsWith("moveName"))
            {
                moves.Add(new Move());
                moves[i].moveName = st.Substring(st.IndexOf("=") + 2);

                st = str.ReadLine();
                moves[i].movePredcessor = st.Substring(st.IndexOf("=") + 2);

                st = str.ReadLine();
                int.TryParse(st.Substring(st.IndexOf("=") + 2), out iParse);
                moves[i].moveWeight = iParse;

                st = str.ReadLine();
                float.TryParse(st.Substring(st.IndexOf("=") + 2), out fParse);
                moves[i].distanceToEnemyMax = fParse;

                st = str.ReadLine();
                float.TryParse(st.Substring(st.IndexOf("=") + 2), out fParse);
                moves[i].distanceToEnemyMin = fParse;

                st = str.ReadLine();
                moves[i].enemyMove = st.Substring(st.IndexOf("=") + 2);

                i++;
            }
        }
        str.Close();
               
        
    }





    public void FighterListing() {
        Debug.Log("FighterListing START");
        Debug.Log("nick = " + nick);
        Debug.Log("style = " + style);

        int i = 0;

        foreach (Move myMove in moves)
        {
            Debug.Log("Move: " + i);
            i++;
            Debug.Log("moveName = " + myMove.moveName);
            Debug.Log("movePredcessor = " + myMove.movePredcessor);
            Debug.Log("moveWeight = " + myMove.moveWeight);            
            Debug.Log("distanceToEnemyMax = " + myMove.distanceToEnemyMax);
            Debug.Log("distanceToEnemyMin = " + myMove.distanceToEnemyMin);
            Debug.Log("enemyMove = " + myMove.enemyMove);
        }
        
    }






    IEnumerator MakeDecision()
    {


        while (true)
        {

            

            distanceToEnemy = Math.Abs(transform.position.x - enemy.transform.position.x);
            enemyMove = enemy.GetComponent<Fighter>().currentMove;

            int i = 0;            

            foreach (Move myMove in moves)
            {
                myMove.canUseNow = true;
                if (distanceToEnemy > myMove.distanceToEnemyMax) myMove.canUseNow = false;
                if (distanceToEnemy < myMove.distanceToEnemyMin) myMove.canUseNow = false;
                //if (enemyMove != myMove.enemyMove) myMove.canUseNow = false;
                if (myMove.canUseNow) i++;
                //if (myMove.canUseNow) Debug.Log("myMove.canUseNow = " + myMove.canUseNow + "i = " + i);
            }

            if (i > 0)
            {
                int j = (int)UnityEngine.Random.Range(0, i+1);

                i = 1;
                foreach (Move myMove in moves)
                {
                    if (i == j) nextMove = myMove.moveName;
                    if (myMove.canUseNow) i++;                    
                }                

               
            }

            /*Debug.Log(nick + " - distanceToEnemy - " + distanceToEnemy);
            Debug.Log(nick + " - enemyMove - " + enemyMove);
            Debug.Log(nick + " - nextMove - " + nextMove);*/


            int k = 1;
            k = (int)UnityEngine.Random.Range(0, 5 + 1);
            if (k == 1) yield return new WaitForSeconds(0.2f);
            if (k == 2) yield return new WaitForSeconds(0.3f);
            if (k == 3) yield return new WaitForSeconds(0.3f);
            if (k == 4) yield return new WaitForSeconds(0.4f);
            if (k == 5) yield return new WaitForSeconds(0.5f);
        }

    }

    
    






    public void set_speed(float sp)
    {

        if (this.name == "Fighter1") speed = sp;
        if (this.name == "Fighter2") speed = sp*(-1);

        
        //Debug.Log("set_speed: " + speed);

    }




    public void go_to_idle()
    {

        currentMove = "idle";
        anim.SetInteger("MoveNumber", 0);
        //speed = 0;
        //Debug.Log("set_currentMove: " + currentMove);

    }




    void FixedUpdate()
    {
        

        if (speed != 0)
        {
            float x = transform.position.x + speed;
            float y = transform.position.y;
            float z = transform.position.z;

            transform.position = new Vector3(x, y, z);
        }

        distanceToEnemy = Math.Abs(transform.position.x - enemy.transform.position.x);
        if (distanceToEnemy < 1.0f)
        {

            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            if (this.name == "Fighter1") {
                x = (enemy.transform.position.x - 1);
            }

            if (this.name == "Fighter2")
            {
                x = (enemy.transform.position.x + 1);
            }           

            transform.position = new Vector3(x, y, z);
            
        }

        if (transform.position.x < -7)
        {
            float x = -7;
            float y = transform.position.y;
            float z = transform.position.z;

            transform.position = new Vector3(x, y, z);
        }

        if (transform.position.x > 7)
        {
            float x = 7;
            float y = transform.position.y;
            float z = transform.position.z;

            transform.position = new Vector3(x, y, z);
        }
        


        if (currentMove == "idle" && nextMove != "idle")
        {
            currentMove = nextMove;
            if (currentMove == "stepforward") anim.SetInteger("MoveNumber", 1);
            if (currentMove == "stepback") anim.SetInteger("MoveNumber", 2);
            if (currentMove == "punch") anim.SetInteger("MoveNumber", 3);
            lastMove = currentMove;
            nextMove = "idle";
        }

    }


    


    public class Move {

		public string moveName = "idle";
		public string movePredcessor = "root";
		public int  moveWeight = 100;        
        public float distanceToEnemyMax = 0.0f;
        public float distanceToEnemyMin = 0.0f;
        public string enemyMove = "any";
        public bool canUseNow = false;
    }
}
