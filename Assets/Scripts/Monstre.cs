using UnityEngine;
using System.Collections;

public class Monstre : MonoBehaviour {

	public PlayerController controller;

	public int maxLife = 5;
	
	public GameObject lifeBar;
	private int _life;
	public int life {
		get { return _life; }
		set { 
			_life = Mathf.Max(0, value);
			float lifeBarScale = (float)_life/(float)maxLife;
			lifeBar.transform.localScale = new Vector3( lifeBarScale, 1, 1 );
		}
	}
	
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
