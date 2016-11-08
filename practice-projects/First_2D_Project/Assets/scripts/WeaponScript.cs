using UnityEngine;
using System.Collections;

/* Script purpose: Instantiate a projectile in front of the game object it is attached to.
 * Meant to be reused everywhere (players, enemies, etc).
 */

public class WeaponScript : MonoBehaviour {

	//--------------------------------
	// Designer variables
	//--------------------------------

	//projectile prefab for shooting
	//needed to set the shot that will be used with this weapon
	public Transform shotPrefab;

	//cooldown in seconds between two shots
	public float shootingRate = 0.25f;

	//--------------------------------
	// Cooldown
	//--------------------------------

	private float shootCooldown;

	// Use this for initialization
	void Start () {
		shootCooldown = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		//If it is greater than 0 we simply cannot shoot.
		if (shootCooldown > 0) {
			// We substract the elapsed time at each frame.
			shootCooldown -= Time.deltaTime;
		}
	}

	//--------------------------------
	// Shooting from another script
	//--------------------------------

	//Create a new projectile if possible
	//meant to be called from other scripts
	public void Attack(bool isEnemy) {
		if (CanAttack) {
			shootCooldown = shootingRate;

			//create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;

			//Assign position
			shotTransform.position = transform.position;

			//
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();

			if (shot != null) {
				shot.isEnemyShot = isEnemy;
			}

			//Make the weapoin shot always towards it
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();

			if (move != null) {
				move.direction = this.transform.right; //move to the right of the sprite
			}
		}
	}

	public bool CanAttack {
		get {
			return shootCooldown <= 0f;
		}
	}
}
