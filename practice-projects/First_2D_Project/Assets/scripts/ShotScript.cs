using UnityEngine;
using System.Collections;

public class ShotScript : MonoBehaviour {

	// 1 - Designer variables

	//damage inflicted 
	public int damage = 1;

	//Projectile damage player or enemies?
	public bool isEnemyShot = false;

	// Use this for initialization
	void Start () {

		//limited time to live to avoid leaks
		Destroy(gameObject, 20); // 20 seconds
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
