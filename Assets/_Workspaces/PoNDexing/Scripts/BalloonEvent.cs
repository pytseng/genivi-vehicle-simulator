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
    }
	
	// Update is called once per frame
	void Update () {
        //InvokeRepeating("BalloonFest", 2.0f, 1f);
    }
    private GameObject _GetTargetObeject()
    {
        return FindObjectOfType<VehicleController>().gameObject;
    }
    private void BalloonFest(){
        var spawningSphere = new Vector3(Random.Range(-10f,10f), Random.Range(0f, 10f), Random.Range(0f, 10f));
        Instantiate(balloon, player_t.position, Quaternion.identity);
    }
}
