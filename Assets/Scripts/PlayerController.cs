using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerGraphic;
	private Vector3 movementVector;
	//private CharacterController characterController;
	private float movementSpeed = 10;

	public int joystickNumber;

	// Use this for initialization
	void Start () {
		//movementVector.y = transform.position.y;
		//characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

		string joystickString = joystickNumber.ToString();

		movementVector.x = Input.GetAxis("Horizontal_P" + joystickString) * movementSpeed;
		movementVector.z = 0;
		movementVector.y = - Input.GetAxis("Vertical_P" + joystickString) * movementSpeed;

		playerGraphic.GetComponent<Rigidbody2D>().velocity = movementVector;

	}

}
