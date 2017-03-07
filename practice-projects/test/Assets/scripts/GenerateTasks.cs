using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GenerateTasks : MonoBehaviour {

	//TODO: generate counter for tasks naming
	int count;
	public List<Task> tasks;
	private float timer;
	public Transform GenerateNewTaskButton;

	public class Task {
	
		private string name;
		private bool activity1, activity2, activity3; //wash, cut, dry

		//constructor
		public Task(string n, bool a1, bool a2, bool a3) {
			name = n;
			activity1 = a1;
			activity2 = a2;
			activity3 = a3;
		}

		public string GetName() { return this.name;}
		public bool GetA1() { return this.activity1; }
		public bool GetA2(){ return this.activity2; }
		public bool GetA3() { return this.activity3; }
	}

	void Awake () {
		count = 1;
		Init ();
	}

	private void Init() {
		tasks = new List<Task> ();
	}

	//for testing with button
	public void NewTaskButtonClicked() {

		//TODO: generate new task every n random seconds

		GenerateNewTaskButton.GetComponent<Button>().interactable = false;

		GenerateNewTask ();

		GenerateNewTaskButton.GetComponent<Button>().interactable = true;

	}

	private void GenerateNewTask() {

		//int rand;
		string nam = "Task " + count;

		//assign random a1
		bool ac1 = (Random.value > 0.5f);

		//assign random a2
		bool ac2 = (Random.value > 0.5f);

		//assign random a3
		bool ac3 = (Random.value > 0.5f);

		if (tasks.Count == 0)
			Init ();

		tasks.Add(new Task (nam, ac1, ac2, ac3));

		Debug.Log ("----------------");
		Debug.Log ("New task!");
		foreach (Task t in tasks) {
			Debug.Log ("Task: " + t.GetName());
			Debug.Log (" - Action1: " + t.GetA1());
			Debug.Log (" - Action2: " + t.GetA2());
			Debug.Log (" - Action3: " + t.GetA3());
		}
		Debug.Log ("----------------");
		count++;
	}
}