using UnityEngine;
using System.Collections;

//Enemy generic shooting (auto) behaviour

public class EnemyScript : MonoBehaviour {

	private WeaponScript weapon;

	void Awake() {
	
		//retrieve the weapon only once
		/*will return null now, since its actually attached to its child "weapon"
		 * not on this object itself*/
		//weapon = GetComponent<WeaponScript>();
		weapon = GetComponentInChildren<WeaponScript>();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

			//auto-fire
			if (weapon != null && weapon.CanAttack) {
				weapon.Attack(true);
		}
	}
}
