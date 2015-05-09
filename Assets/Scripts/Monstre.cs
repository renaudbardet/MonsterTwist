﻿using UnityEngine;
using System.Collections;

public class Monstre : MonoBehaviour {

	public PlayerController controller;

	public int maxLife = 5;
	public int life;
	
	// Use this for initialization
	void Start () {
		life = maxLife;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {

		PlayerController playerHitting = null;

		if (collision.gameObject.GetComponent<PlayerMovement> () != null) {
			playerHitting = collision.gameObject.GetComponent<PlayerMovement> ().controller;
		}
		
		if (collision.gameObject.GetComponent<Arrow> () != null) {
			playerHitting = collision.gameObject.GetComponent<Arrow> ().owner;
			Debug.Log(playerHitting);
		}

		Debug.Log(playerHitting);

		if (playerHitting != null)
			GameManager.instance.PlayerHitMonster (playerHitting);

	}

}
