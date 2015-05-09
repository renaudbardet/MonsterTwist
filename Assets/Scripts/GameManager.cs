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

	public GameObject monstre;
	private PlayerController oldPlayer;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void PlayerSlaynMonster( PlayerController player ){

		// "Résurection" de l'ancien joueur monstre
		oldPlayer = monstre.GetComponent<PlayerMovement>().controller;
		Debug.Log ("oldPlayer = " + oldPlayer);
		if (oldPlayer) {
			oldPlayer.RevertToHuman();
		}
		
		Debug.Log ("slayn by " + player);

		// Change le role du monstre
		player.BecomeMonster( monstre.GetComponent<Monstre>() );
		monstre.GetComponent<PlayerMovement> ().controller = player;
	}

	public void MonsterWin(){
		monstre.GetComponent<PlayerMovement> ().controller.RevertToHuman ();
		monstre.GetComponent<Monstre> ().Respawn();
		
		joueur1.GetComponent<PlayerMovement> ().controller.RevertToHuman ();
		joueur2.GetComponent<PlayerMovement> ().controller.RevertToHuman ();
		joueur3.GetComponent<PlayerMovement> ().controller.RevertToHuman ();
		joueur4.GetComponent<PlayerMovement> ().controller.RevertToHuman ();

		batiment1.GetComponent<Batiment>().Reapparai();
		batiment2.GetComponent<Batiment>().Reapparai();
		batiment3.GetComponent<Batiment>().Reapparai();
		batiment4.GetComponent<Batiment>().Reapparai();


	}

}
