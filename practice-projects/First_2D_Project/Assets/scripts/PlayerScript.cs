using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour {

	// 1 - The speed of the ship
	/* We first define a public variable that will appear in the “Inspector"
	 * view of Unity and can be edited by the programmer at any time (arbitrary). This is the speed applied to the ship.
	 */
	public Vector2 speed = new Vector2(50, 50);

	//2 - Store the movement and the component
	private Vector2 movement;
	private Rigidbody2D rigidbodyComponent;

	// Update is called once per frame
	void Update () {

		// 3 - Retrieve axis information
		/* We use the default axis that can be redefined in 
		 * “Edit” -> “Project Settings” -> “Input”. This will return a value between [-1, 1],
		 * 0 being the idle state, 1 the right, -1 the left.
		 */
		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");

		// 4 - Movement per direction
		/* We multiply the direction by the speed */
		movement = new Vector2 (speed.x * inputX, speed.y * inputY);

		// 5 - Shooting (calling the WeaponScript script)
		/* The “Down” at the end allows us to get the input when the button has been pressed once and only once.
		 * GetButton() returns true at each frame until the button is released. In our case, we clearly want the behavior
		 * of the GetButtonDown() method.
		 */
		bool shoot = Input.GetButtonDown("Fire1"); //click or
		shoot |= Input.GetButtonDown ("Fire2"); //ctrl
		//if the keys click or ctrl is being pressed. these are the defaults.

		if (shoot) {

			WeaponScript weapon = GetComponent<WeaponScript> ();

			if (weapon != null) {

				//false since the player is not an enemy
				weapon.Attack (false);
			}
		}
	}

	/* We change the rigidbody velocity. This will tell the physic engine to move the game object.
	 * We do that in FixedUpdate() as it is recommended to do everything that is physics-related in here.
	 */
	void FixedUpdate() {

		// 5 - Get the component and store the reference
		/* We need to access the rigidbody component, but we can avoid to do it every
		 * frame by storing a reference.
		 */
		if (rigidbodyComponent == null) {
			rigidbodyComponent = GetComponent<Rigidbody2D> ();
		}

		// 6 - Move the game object
		rigidbodyComponent.velocity = movement;
	
	}

	//used to handle collisions between the player and the enemy
	void OnCollisionEnter2D(Collision2D collision) {

		bool damagePlayer = false;

		//collision with the enemy
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript> ();

		if (enemy != null) {

			//kill the enemy
			HealthScript enemyHealth = enemy.gameObject.GetComponent<HealthScript> ();
			if (enemyHealth != null) {
				enemyHealth.Damage (enemyHealth.hitPoints);

				damagePlayer = true;
			}
		}

		//damage the player
		if (damagePlayer) {
			HealthScript playerHealth = this.GetComponent<HealthScript> ();
			if (playerHealth != null) {
				playerHealth.Damage (1);
			}
		}
	}

	void OnDestroy () {


		//Debug.Log ("ded");
		//game over. need to call the other scene for menu!
		var gameOver = GameObject.FindObjectOfType<GameOverScript>();//couldnt find gameoverscript in the scene
		//Debug.Log ("after finding object");
		Debug.Log ("Instance created");
		gameOver.ShowButtons ();

		/*
		GameOverScript gameOver = GameObject.Find ("Panel").GetComponent<GameOverScript> ();
		gameOver.ShowButtons ();
		*/
	}

}
