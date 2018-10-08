using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stoppings : MonoBehaviour
{

    private UnityEvent stopEvent;
    private GameObject target;
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

    }
    void Start()
    {
        eventHappened = false;
        if (stopEvent == null)
            stopEvent = new UnityEvent();
        switch (eventName)
        {
            case "jaywalk":
                stopEvent.AddListener(_JayWalk);
                break;
            case "car cut in lane":
                break;
            case "red light":
                break;
            case "short of power":
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dist = _DistanceDetection(targetObjectName);
        if (eventHappened == false && dist < targetObjectApproachDistance)
        {
            stopEvent.Invoke();
            eventHappened = true;
        }
    }

    private float _DistanceDetection(string stringName)
    {
        target = GameObject.Find(stringName);
        float dist = Vector3.Distance(target.transform.position, this.transform.position);
        return dist;
    }

    //private _spawnEventObject()
    //{

    //}

    private void _JayWalk(){
        GameObject jayWalkerPrefab = Resources.Load("prefabs/Man") as GameObject;
        GameObject jayWalker = Instantiate(jayWalkerPrefab, this.transform.position, this.transform.rotation);
    }
}
