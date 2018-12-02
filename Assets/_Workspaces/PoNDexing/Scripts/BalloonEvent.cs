using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonEvent : MonoBehaviour {
    private Transform player_t;
    GameObject balloon;
    // Use this for initialization
    void Start () {
        player_t = _GetTargetObeject().transform;
        balloon = Resources.Load("prefabs/balloon") as GameObject;
        InvokeRepeating("BalloonFest", 2.0f, 10f);
    }
	
	// Update is called once per frame
	void Update () {

    }
    private GameObject _GetTargetObeject()
    {
        return FindObjectOfType<VehicleController>().gameObject;
    }
    private void BalloonFest(){
        var spawningSphere = new Vector3(Random.Range(10f,30f), Random.Range(0f, 5f), Random.Range(-5f, 5f));
        Instantiate(balloon, player_t.position + spawningSphere, Quaternion.identity);
    }
}
