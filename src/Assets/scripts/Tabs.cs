using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour {

	ToolboxManager manager;
	// GameObject toolbox;

	public void switchCustomer() {

		GameObject curr_worker = this.transform.parent.gameObject;

		curr_worker.transform.SetAsLastSibling ();
	}

	public void switchTabs() {

		// Debug.Log (this.transform.parent.name);

		GameObject curr_tab = this.transform.parent.gameObject;

		if (CreateNewBlock.canCreate) {
			
			curr_tab.transform.SetAsLastSibling ();

			if (curr_tab.transform.name == "Tab1") {

				manager.txt_checkinLeft_thread1.color = Color.black;
				manager.txt_cutLeft_thread1.color = Color.black;
				manager.txt_dryLeft_thread1.color = Color.black;
				manager.txt_washLeft_thread1.color = Color.black;
				manager.txt_whileLeft_thread1.color = Color.black;
				manager.txt_ifLeft_thread1.color = Color.black;
				manager.txt_resourcesLeft_thread1.color = Color.black;
				manager.txt_returnLeft_thread1.color = Color.black;
				manager.txt_checkoutLeft_thread1.color = Color.black;
				manager.txt_groomLeft_thread1.color = Color.black;
				manager.txt_pickupLeft_thread1.color = Color.black;

				manager.txt_checkinLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_cutLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_dryLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_washLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_whileLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_ifLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_resourcesLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_returnLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_checkoutLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_groomLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_pickupLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);

				//if (this.GetComponent<Image>().color == Color.white) {

					this.GetComponent<Image> ().color = new Vector4 (0.9F, 0.9F, 0.9F, 1);
					GameObject.Find ("Tab2").transform.Find ("Tick").GetComponent<Image> ().color = Color.white;

				//}

			} else if (curr_tab.transform.name == "Tab2") {

				manager.txt_checkinLeft_thread2.color = Color.black;
				manager.txt_cutLeft_thread2.color = Color.black;
				manager.txt_dryLeft_thread2.color = Color.black;
				manager.txt_washLeft_thread2.color = Color.black;
				manager.txt_whileLeft_thread2.color = Color.black;
				manager.txt_ifLeft_thread2.color = Color.black;
				manager.txt_resourcesLeft_thread2.color = Color.black;
				manager.txt_returnLeft_thread2.color = Color.black;
				manager.txt_checkoutLeft_thread2.color = Color.black;
				manager.txt_groomLeft_thread2.color = Color.black;
				manager.txt_pickupLeft_thread2.color = Color.black;

				manager.txt_checkinLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_cutLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_dryLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_washLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_whileLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_ifLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_resourcesLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_returnLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_checkoutLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_groomLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
				manager.txt_pickupLeft_thread1.color = new Vector4(0.82F, 0.82F, 0.82F, 1);

				//if (this.GetComponent<Image>().color == Color.white) {

					this.GetComponent<Image> ().color = new Vector4 (0.9F, 0.9F, 0.9F, 1);
					GameObject.Find ("Tab1").transform.Find ("Tick").GetComponent<Image> ().color = Color.white;

				//}
			}

		} else {
			manager.showError ("Use or discard your current object first");
		}
	}

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("_SCRIPTS_").GetComponent<ToolboxManager> ();
		// toolbox = GameObject.Find ("DropAreaTools");

		GameObject.Find ("Tab1").transform.Find ("Tick").GetComponent<Image> ().color = new Vector4 (0.9F, 0.9F, 0.9F, 1);

		manager.txt_checkinLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_cutLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_dryLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_washLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_whileLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_ifLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_resourcesLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_returnLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_checkoutLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_groomLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
		manager.txt_pickupLeft_thread2.color = new Vector4(0.82F, 0.82F, 0.82F, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}