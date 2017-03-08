using UnityEngine;
using System.Collections;

public class TaskClass : MonoBehaviour {

	public class Task {

		public string name;
		public bool action1; //wash
		public bool action2; //dry
		public bool action3; //cut

		//constructor
		public Task(string n, bool a1, bool a2, bool a3) {
			name = n;
			a1 = action1;
			a2 = action2;
			a3 = action3;
		}
	}

	void Start() {

	}

}

