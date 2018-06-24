using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class Fighter : MonoBehaviour {

    GameObject enemy;

    public string nick = "none";
    public string style = "none";

    public int stamina = 100;    
    public bool knockdown = false;
    public bool knockout = false;

    public string nextMove = "none";
    public string currentMove = "none";
    public string lastMove = "none";

    public float distanceToEnemy = 3.0f;
    public string enemyMove = "none";

    public List<Move> moves = new List<Move>();

	// Use this for initialization
	void Start () {
        
        if (this.name == "Fighter1") enemy = GameObject.Find("Fighter2");
        if (this.name == "Fighter2") enemy = GameObject.Find("Fighter1");
    }

    public void LoadStyle(string fileName)
    {

        StreamReader str = new StreamReader(fileName, Encoding.UTF8);
        int i = 0;
        int iParse;
        float fParse;

        string st = str.ReadLine();
        if (st.StartsWith("nick")) nick = st.Substring(st.IndexOf("=") + 1);
        st = str.ReadLine();
        if (st.StartsWith("style")) style = st.Substring(st.IndexOf("=") + 1);

        while (!str.EndOfStream)
        {            

                st = str.ReadLine();
            if (st.StartsWith("moveName"))
            {
                moves.Add(new Move());
                moves[i].moveName = st.Substring(st.IndexOf("=") + 1);

                st = str.ReadLine();
                moves[i].movePredcessor = st.Substring(st.IndexOf("=") + 1);

                st = str.ReadLine();
                int.TryParse(st.Substring(st.IndexOf("=") + 1), out iParse);
                moves[i].moveWeight = iParse;

                st = str.ReadLine();
                float.TryParse(st.Substring(st.IndexOf("=") + 1), out fParse);
                moves[i].distanceToEnemyMax = fParse;

                st = str.ReadLine();
                float.TryParse(st.Substring(st.IndexOf("=") + 1), out fParse);
                moves[i].distanceToEnemyMin = fParse;

                st = str.ReadLine();
                moves[i].enemyMove = st.Substring(st.IndexOf("=") + 1);

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

    public void MakeDecision () {
        
        int i = 0;

        foreach (Move myMove in moves)
        {
            myMove.canUseNow = true;
            if (distanceToEnemy > myMove.distanceToEnemyMax) myMove.canUseNow = false;
            if (distanceToEnemy < myMove.distanceToEnemyMin) myMove.canUseNow = false;
            if (enemyMove != myMove.enemyMove) myMove.canUseNow = false;
            if (myMove.canUseNow) i++;
        }
                
        int j = (int) UnityEngine.Random.Range(0, i+1);

        foreach (Move myMove in moves)
        {
            if (i == j) nextMove = myMove.moveName;
            if (myMove.canUseNow) i++;                        
        }        
    }

    public void MakeMove()
    {
        lastMove = currentMove;
        currentMove = nextMove;
        nextMove = "none";
        //вызываем физику и анимацию
    }

    void FixedUpdate()
    {
        distanceToEnemy = Math.Abs(transform.position.x - enemy.transform.position.x);
        enemyMove = enemy.GetComponent<Fighter>().currentMove;
        MakeDecision();
        MakeMove();
    }



    public class Move {

		public string moveName = "none";
		public string movePredcessor = "root";
		public int  moveWeight = 100;        
        public float distanceToEnemyMax = 0.0f;
        public float distanceToEnemyMin = 0.0f;
        public string enemyMove = "any";
        public bool canUseNow = false;
    }
}
