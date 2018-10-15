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
                case "Arriving destination":
                    _ArrivingDestination(player);
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
        //load and play audio
        LoadAndPlaySoundToObject(player, "alert");
        //load game object
        LoadSpawnObject(player, "Man");
    }

    private void _ArrivingDestination(GameObject player){
        LoadAndPlaySoundToObject(player, "arrivingDestination");
    }

    private void _CarCutInLane(){

    }

    private void _RedLight(){

    }

    private void LoadAndPlaySoundToObject(GameObject player, string soundFile)
    {
        var audioClip = Resources.Load<AudioClip>("Audios/" + soundFile);
        AudioSource audioSource = player.AddComponent<AudioSource>() as AudioSource;
        player.GetComponent<AudioSource>().PlayOneShot(audioClip, 1.0f);
    }

    private void LoadSpawnObject(GameObject player, string prefabName)
    {
        GameObject prefab = Resources.Load("prefabs/" + prefabName) as GameObject;
        GameObject prefabInstantiate = Instantiate(prefab, player.transform.position, player.transform.rotation);
    }

}
