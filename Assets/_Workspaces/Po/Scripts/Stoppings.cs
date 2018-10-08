using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stoppings : MonoBehaviour
{

    private GameObject player;
    private bool eventHappened; 

    [SerializeField]
    private string eventName;
    [SerializeField]
    private string targetObjectName; //XE_Rigged(Clone) is the name of the spawned car
    [SerializeField]
    private float targetObjectApproachDistance;
    [SerializeField]
    private float targetObjectAwayDistance;



    // Use this for initialization
    private void Awake()
    {
        targetObjectName = "XE_Rigged(Clone)";
    }
    void Start()
    {
        eventHappened = false;
        player = _GetTargetObeject(targetObjectName);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = _DistanceDetection(targetObjectName);
        if (eventHappened == false && dist < targetObjectApproachDistance)
        {

            switch (eventName)
            {
                case "jaywalk":
                    _JayWalk(player);
                    break;
                case "car cut in lane":
                    _CarCutInLane();
                    break;
                case "red light":
                    _RedLight();
                    break;
                case "short of power":
                    break;
                case "none":
                    Debug.Log("no event");
                    break;
            }
            eventHappened = true;
        }
    }

    private GameObject _GetTargetObeject (string stringName){
        //player = GameObject.Find(stringName);
        player = FindObjectOfType<VehicleController>().gameObject;
        return player;
    }

    private float _DistanceDetection(string stringName)
    {
        player = GameObject.Find(stringName);
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        return dist;
    }


    private void _JayWalk(GameObject player){
        //play audio
        var alertClip = Resources.Load<AudioClip>("Audios/alert");
        AudioSource alert = player.AddComponent<AudioSource>() as AudioSource;
        player.GetComponent<AudioSource>().PlayOneShot(alertClip, 1.0f);

        //load game object
        GameObject jayWalkerPrefab = Resources.Load("prefabs/Man") as GameObject;
        GameObject jayWalker = Instantiate(jayWalkerPrefab, this.transform.position, this.transform.rotation);
    }

    private void _CarCutInLane(){

    }

    private void _RedLight(){

    }
}
