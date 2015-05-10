﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerGraphic;
	public PlayerMovement defaultPlayerGraphic;
	public int joystickNumber;
	public bool canShoot = true;

	public Arrow arrow;
	public Arrow fireball;
	private Vector3 movementVector;
	public float movementSpeed = 12;

	public double arrowCooldown = .5;
	public double fireballCooldown = 2;
	private double lastProjectileShot = 0;
	
	public double humanPunchCooldown = .3;
	public double monsterPunchCooldown = .7;
	private double GetPunchCooldown() {
		return isMonster ? monsterPunchCooldown : humanPunchCooldown;
	}
	private double lastPunch = 0;

	private double isStunSince = -100;
	public double stunDelay = 1.5;

	public bool isMonster = false;

	private bool isDodging=false;

	Orient currentHeading = Orient.Right;

	// Use this for initialization
	void Start () {
		playerGraphic = defaultPlayerGraphic.gameObject;
		//movementVector.y = transform.position.y;
		//characterController = GetComponent<CharacterController>();
		defaultPlayerGraphic.controller = this;
	}

	bool getIsStun() {
		return Time.time - isStunSince < stunDelay;
	}

	// Update is called once per frame
	void Update () {

		if (getIsStun())
			return;

		if (isDodging && Time.time - lastDodged > dodgeTime)
			endDodge ();

		string joystickString = joystickNumber.ToString();

		float xAxis = Input.GetAxis("Horizontal_P" + joystickString) * movementSpeed;
		float yAxis = - Input.GetAxis("Vertical_P" + joystickString) * movementSpeed;
		movementVector.x = xAxis;
		movementVector.z = 0;
		movementVector.y = yAxis;

		float xAxisDelta = Mathf.Abs (xAxis);
		float yAxisDelta = Mathf.Abs (yAxis);

		if (xAxisDelta > yAxisDelta) {

			if (xAxis < 0) {
				setHeading (Orient.Left);
			} else if (xAxis > 0) {
				setHeading (Orient.Right);
			}

		} else {

			if (yAxis < 0) {
				setHeading (Orient.Down);
			} else if (yAxis > 0) {
				setHeading (Orient.Up);
			}

		}

		playerGraphic.GetComponent<Rigidbody2D>().velocity = movementVector;

		if ( Input.GetButtonDown ("Fire_P" + joystickString) ) {

			if ( CheckHitClose() != null || isMonster) {
				if ((Time.time - lastPunch) > GetPunchCooldown() ) {

					Debug.Log ("TRY HIT CLOSE RANGE"); 
					hitCloseRange();

				}
			}
			else {

				if (	!isMonster && canShoot
		    		&& ((Time.time - lastProjectileShot) > arrowCooldown)
			    	) {
					
					shoot( arrow );

				}
			}
		}

		if ( Input.GetButtonDown ("Dodge_P" + joystickString) ) {

			if ( !isMonster )
				dodge();
			else if ( (Time.time - lastProjectileShot) > fireballCooldown )
				shoot( fireball );
			
		}
		
	}

	private double lastDodged = 0;
	public double dodgeTime = .3;
	public double dodgeCooldown = 1;

	void dodge() {

		if ( isDodging || getIsStun() || isMonster )
			return;

		if (Time.time - lastDodged < (dodgeTime + dodgeCooldown))
			return;

		isDodging = true;

		Vector3 movementVector = new Vector3 (0, 0, 0);
		switch( currentHeading ) {
		case Orient.Up:
			movementVector.y += 20;
			break;
		case Orient.Down:
			movementVector.y -= 20;
			break;
		case Orient.Left:
			movementVector.x -= 20;
			break;
		case Orient.Right:
			movementVector.x += 20;
			break;
		}
		playerGraphic.GetComponent<Rigidbody2D>().velocity = movementVector;
		playerGraphic.GetComponent<Collider2D> ().enabled = false;

	}

	void endDodge() {
		playerGraphic.GetComponent<Collider2D> ().enabled = true;
		isDodging = false;
	}

	void hitCloseRange(){
		Debug.Log("try hitting smthg");

		lastPunch = Time.time;
		Collider2D collider = CheckHitClose ();

		if (collider == null) return;

		Debug.Log("hitting smthg"+ collider.name);
		if (collider.GetComponent<Monstre> () != null) {
			Debug.Log ("Monstre not null"); 
			GameManager.instance.PlayerHitMonster (this);
		}
		else if (isMonster) {
			Debug.Log ("Is monster true");
			if (collider.GetComponent<PlayerMovement> () != null) {
				Debug.Log("hitting someone");
				collider.GetComponent<PlayerMovement> ().controller.TakeDamage ();
				++playerGraphic.GetComponent<Monstre>().life;
			} else if (collider.GetComponent<Arrow> () != null) {
				Destroy (collider.gameObject);
			}
		}

	}
		
	void shoot( Arrow arrowType ) {

		lastProjectileShot = Time.time;

		Arrow a = Instantiate (arrowType, playerGraphic.transform.position, transform.rotation) as Arrow;
		Rigidbody2D aRigidbody = a.GetComponent<Rigidbody2D> ();

		a.owner = this;
		
		Physics2D.IgnoreCollision (a.GetComponent<Collider2D> (), playerGraphic.GetComponent<Collider2D> ());
		
		switch (currentHeading) {
		case Orient.Up:
			aRigidbody.velocity = new Vector3 (0, a.initialVelocity);
			aRigidbody.rotation = 90;
			break;
		case Orient.Down:
			aRigidbody.velocity = new Vector3 (0, -a.initialVelocity);
			aRigidbody.rotation = 90;

			break;
		case Orient.Left:
			aRigidbody.velocity = new Vector3 (-a.initialVelocity, 0);
			break;
		case Orient.Right:
			aRigidbody.velocity = new Vector3 (a.initialVelocity, 0);
			break;
		}

	}

	void setHeading( Orient newHeading ) {

		if (newHeading != currentHeading) {
			currentHeading = newHeading;
		}

	}

	public void BecomeMonster( Monstre monster ) {

		monster.nbCrush = 0;
		monster.life = monster.maxLife;

		// Stoppe le joueur après l'avoir mis à son spawn puis le rend invisible et immatériel
		this.playerGraphic.transform.position = playerGraphic.GetComponent<PlayerMovement> ().spawn.transform.position;
		this.playerGraphic.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
		this.playerGraphic.GetComponent<BoxCollider2D> ().enabled  = false;
		this.playerGraphic.GetComponent<SpriteRenderer> ().enabled  = false;
		this.canShoot  = false;
		this.isDodging = false;

		isMonster = true;
		this.playerGraphic = monster.gameObject;
		
	}

	public void RevertToHuman() {
		
		isMonster = false;
		this.playerGraphic = this.defaultPlayerGraphic.gameObject;

		this.playerGraphic.gameObject.SetActive(true);
		// le rend visible et matériel
		this.playerGraphic.GetComponent<BoxCollider2D> ().enabled  = true;
		this.playerGraphic.GetComponent<SpriteRenderer> ().enabled  = true;
		this.canShoot = true;

	}

	public void Respawn() {

		RevertToHuman ();

		endDodge ();
		playerGraphic.transform.position = defaultPlayerGraphic.spawn.transform.position;

	}

	public Transform debugCubePrefab;

	Collider2D CheckHitClose() {

		Collider2D ret = null;
		RaycastHit2D hit ; 

		Vector2 playerSize = playerGraphic.GetComponent<Renderer> ().bounds.size;

		playerGraphic.GetComponent<Collider2D> ().enabled = false;

		float errorDistance = .25f;

		switch (currentHeading) {
		case Orient.Up:
			hit = Physics2D.BoxCast (
				playerGraphic.transform.position - (Vector3.up*errorDistance)
				, playerSize, .0f, Vector2.up, 1);

			ret = hit.collider;
			break;

		case Orient.Down:
			hit = Physics2D.BoxCast (
				playerGraphic.transform.position + (Vector3.up*errorDistance)
				, playerSize, .0f, -Vector2.up, 1);

			ret = hit.collider;
			break;

		case Orient.Left:
			hit = Physics2D.BoxCast (
				playerGraphic.transform.position + (Vector3.right*errorDistance)
				, playerSize, .0f, -Vector2.right, 1);
			
			ret = hit.collider;
			break;

		case Orient.Right:
			hit = Physics2D.BoxCast (
				playerGraphic.transform.position - (Vector3.right*errorDistance)
				, playerSize, .0f, Vector2.right, 1);
			
			ret = hit.collider;
			break;
		}
		playerGraphic.GetComponent<Collider2D> ().enabled = true;

		return ret;

	}

	public void TakeDamage() {
		if (!isMonster) {
			Respawn();
		}
	}

	public void BecomeStun() {
		if (!isMonster) {
			isStunSince = Time.time;
		}
	}

}
