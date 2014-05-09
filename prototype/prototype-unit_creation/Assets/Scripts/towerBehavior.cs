﻿using UnityEngine;
using System.Collections;
namespace global{
	public class towerBehavior : MonoBehaviour {
		
		public gameManager Manager;

		public enum AttackState{
			ambient,
			attack
		};
		
		public enum player{
			player0, // =0
			player1, // <0
			player2  // >0
		};
		
		#region unitVars
		public GameObject mEnemyToSpawn = null;
		public GameObject unitPrefab = null;
		private float spawnTime = 0;
		private float spawnInterval = .5f;
		#endregion

		public int units = 10;
		private float sendRate = .75f;
		private int unitsToSend;
		public Sprite player0Sprite;
		public Sprite player1Sprite;
		public Sprite player2Sprite;
		public Quaternion destination;
		private float incrementTime = 0;
		private float incrementInterval = 3;

		private AttackState attackState = AttackState.ambient;
		public player currentState = player.player0;

		// Use this for initialization
		void Start () {

			Manager = GameObject.Find ("Main Camera").GetComponent<gameManager>();

			if(this.transform.position.x == 0){
				//currentState = player.player0;
			}
			if(this.transform.position.x < 0){
				//currentState = player.player1;
				units = -units;
			}
			if(this.transform.position.x > 0){
				//currentState = player.player2;
			}
		
			#region initialize unit spawning
			if (null == unitPrefab) 
				unitPrefab = Resources.Load("Prefabs/unit") as GameObject;
			#endregion
		}
		
		// Update is called once per frame
		void Update () {

			if ((Time.realtimeSinceStartup - incrementTime) > incrementInterval) {
				if(units < 0) {units--;}
				if(units > 0) {units++;}
				incrementTime = Time.realtimeSinceStartup;
			}

			spriteManage();
			attack();

		}


		void attack() {
			//if the tower is not in attack state, exit function
			if(attackState == AttackState.ambient) {
				return;
			}
			
			if(unitsToSend == 0){
				attackState = AttackState.ambient;
				return;
			}
			
			//send troops to the tower
			if ((Time.realtimeSinceStartup - spawnTime) > spawnInterval) {
				spawnUnit ();
				spawnTime = Time.realtimeSinceStartup;

			}
		}
		
		void spriteManage() {
			if (units == 0 && currentState != player.player0) {
				currentState = player.player0;
				SpriteRenderer r = GetComponent<SpriteRenderer>();
				r.sprite = player0Sprite;
				attackState = AttackState.ambient;
			}
			else if (units < 0 && currentState != player.player1) {
				currentState = player.player1;
				SpriteRenderer r = GetComponent<SpriteRenderer>();
				r.sprite = player1Sprite;
			}
			else if (units > 0 && currentState != player.player2) {
				currentState = player.player2;
				SpriteRenderer r = GetComponent<SpriteRenderer>();
				r.sprite = player2Sprite;
			}
		}
		
		void OnTriggerEnter2D(Collider2D other)
		{
			string name = other.name;
			if (name.Contains("unit(")) {
				GameObject unit = GameObject.Find(other.name);
				unitBehavior otherscript = (unitBehavior) unit.GetComponent(typeof(unitBehavior));
				units += otherscript.getDamage();
				Destroy(other.gameObject);
			}
		}
		
		void spawnUnit() {
			GameObject e = Instantiate(unitPrefab) as GameObject;
			unitBehavior unit = e.GetComponent<unitBehavior>();
			if(unit != null) {
				e.name = "unit(" + Time.realtimeSinceStartup + ")";
				if(units < 0) {unit.setDamage(-1);}
				else if(units > 0) {unit.setDamage(1);}
				e.transform.rotation = destination;
				e.transform.position = transform.position + (e.transform.up * 4);
				units -= unit.getDamage(); unitsToSend--;
			}
		}
		
		void OnMouseUp() {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			unitsToSend = Mathf.Abs((int) (units * sendRate));
			destination = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
			attackState = AttackState.attack;
		}	
/*
void OnMouseUp() {
			Vector3 dist;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			for(int i = 0; i < Manager.towers.Length; i++) {
				dist = mousePos - Manager.towers[i].transform.position;
				Debug.Log(Mathf.Abs (dist.magnitude));
				if(Mathf.Abs(dist.magnitude) < 4) {
					unitsToSend = Mathf.Abs((int) (units * sendRate));
					destination = Quaternion.LookRotation(Vector3.forward, Manager.towers[i].transform.position - transform.position);
					attackState = AttackState.attack;
					return;
				}
			}
		}
*/
	}
}
