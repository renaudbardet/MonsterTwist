using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject joueur1;
	public GameObject joueur2;
	public GameObject joueur3;
	public GameObject joueur4;

	public GameObject batiment1;
	public GameObject batiment2;
	public GameObject batiment3;
	public GameObject batiment4;

	public Monstre monstre;
	private PlayerController oldPlayer;

	public GameObject instruction;
	public GameObject appuyez;
	public bool isInstruc = true;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		instruction.GetComponent<Renderer> ().enabled = isInstruc;
		appuyez.GetComponent<Renderer> ().enabled = isInstruc;

		if (isInstruc) {
			joueur1.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
			joueur2.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
			joueur3.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
			joueur4.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
		}
	}

	public void PlayerHitMonster( PlayerController player ){

		Debug.Log ("player " + player + " hit the monster");

		--monstre.life;
		Debug.Log ("monster life " + monstre.life);
		if (monstre.life <= 0)
			PlayerSlaynMonster (player);

	}

	public void PlayerSlaynMonster( PlayerController player ){

		// "Résurection" de l'ancien joueur monstre
		PlayerController oldPlayer = monstre.GetComponent<Monstre>().controller;
		Debug.Log ("oldPlayer = " + oldPlayer);
		if (oldPlayer) {
			oldPlayer.RevertToHuman();
		}

		batiment1.GetComponent<Batiment>().Reapparai();
		batiment2.GetComponent<Batiment>().Reapparai();
		batiment3.GetComponent<Batiment>().Reapparai();
		batiment4.GetComponent<Batiment>().Reapparai();

		Debug.Log ("slayn by " + player);

		monstre.GetComponent<Animator>().SetBool ("Death", true);
	}

	public void PlayerBecomeMonster( PlayerController player ){

		// Change le role du monstre
		player.BecomeMonster( monstre );
		monstre.controller = player;
		monstre.GetComponent<Animator>().SetBool ("Death", false);
		
	}

	public void MonsterWin(){
		// Bandeau de victoire
		// ici
		monstre.GetComponent<Monstre> ().Respawn();

		joueur1.GetComponent<PlayerMovement> ().controller.StartCoroutine(joueur1.GetComponent<PlayerMovement> ().controller.Respawn());
		joueur2.GetComponent<PlayerMovement> ().controller.StartCoroutine(joueur2.GetComponent<PlayerMovement> ().controller.Respawn());
		joueur3.GetComponent<PlayerMovement> ().controller.StartCoroutine(joueur3.GetComponent<PlayerMovement> ().controller.Respawn());
		joueur4.GetComponent<PlayerMovement> ().controller.StartCoroutine(joueur4.GetComponent<PlayerMovement> ().controller.Respawn());

		batiment1.GetComponent<Batiment>().Reapparai();
		batiment2.GetComponent<Batiment>().Reapparai();
		batiment3.GetComponent<Batiment>().Reapparai();
		batiment4.GetComponent<Batiment>().Reapparai();

		StartCoroutine(DebutPartie ());
	}

	public IEnumerator DebutPartie(){
		Debug.Log ("3");
		yield return new WaitForSeconds(1f);
		Debug.Log ("2");
		yield return new WaitForSeconds(1f);
		Debug.Log ("1");
		yield return new WaitForSeconds(1f);
		Debug.Log ("Partez !");
		
	}

}
