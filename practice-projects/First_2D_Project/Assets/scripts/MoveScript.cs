using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	// 1 - Designed variables
	public Vector2 speed = new Vector2(10, 10);

	// Moving direction
	public Vector2 direction = new Vector2(-1, 0); //(left/right, up/down)
	 
	private Vector2 movement;
	private Rigidbody2D rigidbodyComponent;

	// Update is called once per frame
	void Update () {
		// 2 - Movement
		movement = new Vector2 (speed.x * direction.x, speed.y * direction.y);
	}

	void FixedUpdate() {
		// Apply movement to the rigid body
		if (rigidbodyComponent == null) {
			rigidbodyComponent = GetComponent<Rigidbody2D> ();
		}

		rigidbodyComponent.velocity = movement;
	
	}

	// Use this for initialization
	void Start () {
	
	}
}
