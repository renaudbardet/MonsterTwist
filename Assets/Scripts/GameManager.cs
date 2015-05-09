using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject joueur1;
	public GameObject joueur2;
	public GameObject joueur3;
	public GameObject joueur4;

	public GameObject monstre;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void PlayerSlaynMonster( PlayerController player ){

		Debug.Log ("slayn by " + player);

	}

}
