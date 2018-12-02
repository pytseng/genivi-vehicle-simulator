using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonEventListener : MonoBehaviour {
	bool eventTriggered;
	GameObject player;
	// Use this for initialization
	void Start () {
		eventTriggered = false;

	}
	// Update is called once per frame
	void Update () {
		if (!eventTriggered) {
			if (Input.GetKeyDown(KeyCode.B)) {
				player = FindObjectOfType<VehicleController>().gameObject;
				if (!eventTriggered) {
					if (player.GetComponent<BalloonEvent>() == null) {
						player.AddComponent<BalloonEvent>();
						eventTriggered = true;
					}
				}
			}
		} else {
			if (Input.GetKeyDown(KeyCode.B)) {
				player = FindObjectOfType<VehicleController>().gameObject;
				if (player.GetComponent<BalloonEvent>()) {
					Destroy(player.GetComponent<BalloonEvent>());
					eventTriggered = false;
				}
			}
		}

	}
}
