using UnityEngine;
using System.Collections;

public class Monstre : MonoBehaviour {

	public PlayerController controller;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.GetComponent<PlayerMovement>().controller){
			GameManager.instance.PlayerSlaynMonster (collision.gameObject.GetComponent<PlayerMovement>().controller);
		}
	}

}
