using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	//total hitpoints
	public int hitPoints = 1;

	//Enemy or Player?
	public bool isEnemy = true;

	//Inflicts damage and checks if the object should be destroyed
	// parameter: damage count
	public void Damage(int damageCount) {

		hitPoints -= damageCount;

		if (hitPoints <= 0) {
			//dead!
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider) {

		//Is this a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();

		//avoid friendly fire, even from enemy to enemy
		if (shot != null) {

			if (shot.isEnemyShot != isEnemy) {
				Damage (shot.damage);

				//Destroy shot
				//Remember to always target the game object, otherwise you will just remove the script
				Destroy (shot.gameObject);
			
			}
		}
			
	}

}
