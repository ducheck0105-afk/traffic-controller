using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _0.Custom.Scripts;

public class CarAI : MonoBehaviour
{
    public Transform waypointsHolder; //parent of all waypoints
    public int startWaypointIndex = 0; //waypoint's index in array which will be car's first destination
    public bool freezeYAxis = true; //used to decide move car on Y axis or not when following the path
    [HideInInspector] public bool isMain;
    private float acceleration = 2f;
    private float deceleration = 5f;
    private float maxMoveSpeed = 7f;
    private float rotationSpeed = 2.5f;

    public float distanceBeforeDeceleration = 8f; //distance between car and obstacle when car must start's deceleration

    public LayerMask decelerationLayer; //car checks on these layers if there is something in front of it
    public Transform[] wheels;
    public bool loop = true; //used to decide loop infinitely on path or not
    public float radius = 0.5f;

    private Transform myTransform;
    public List<Transform> waypoints = new List<Transform>();
    private int waypointIndex = 0;
    private int lastWaypointID = 0;
    private float currentSpeed = 0.0f;
    private bool accelerating = true;
    private bool stopped = false;
    private bool onJunction = false;
    private Junction currentJunction;

    void Start()
    {
        //if waypointsHolder variable is null give warning
        if (!waypointsHolder)
        {
            Debug.LogWarning("'WaypointsHolder' isn't set'");
            enabled = false;
            return;
        }

        //cache transform component, good for performance
        myTransform = GetComponent<Transform>();

        //fill waypoints array
        foreach (Transform child in waypointsHolder)
            waypoints.Add(child);

        //set first waypoint index
        if (startWaypointIndex < waypoints.Count)
            waypointIndex = startWaypointIndex;
    }


    void FixedUpdate()
    {
        if (onJunction)
            return;

        RaycastHit hit;
        if (Physics.SphereCast(myTransform.position, radius, myTransform.forward, out hit, distanceBeforeDeceleration,
                decelerationLayer))
        {
            if (hit.collider.gameObject.tag == "Car" && hit.collider.gameObject.GetComponent<CarAI>().isMain == isMain && isMain)
                accelerating = false;
        }

        else
            accelerating = true;
    }


    void Update()
    {
        MoveTrans();
    }

    private void MoveTrans()
    {
        if (!stopped)
        {
            //controll the speed
            if (accelerating)
                currentSpeed = maxMoveSpeed;
            else
                currentSpeed = 0;

            //clamp speed, it won't go more than value entered in 'maxMoveSpeed'
            currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, maxMoveSpeed);

            Vector3 direction = (waypoints[waypointIndex].position - myTransform.position).normalized;

            //if freezeYAxis is true, car won't follow the path on Y axis
            if (freezeYAxis)
                direction = new Vector3(direction.x, 0.0f, direction.z);

            Quaternion newRotation = Quaternion.LookRotation(direction);

            //rotate car towards waypoint
            // myTransform.rotation = Quaternion.Slerp(myTransform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            // myTransform.Translate(0, 0, currentSpeed * Time.deltaTime);


            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            myTransform.position += direction * currentSpeed * Time.deltaTime;

            //rotate wheels depending on speed
            foreach (Transform wheel in wheels)
                wheel.Rotate(Vector3.right, currentSpeed * Time.deltaTime * 90, Space.Self);
        }
    }

    void OnTriggerStay(Collider col)
    {
        //if car enters in the waypoint
        if (col.tag == "Waypoint" && Vector3.Distance(myTransform.position, waypoints[waypointIndex].position) < 1.0f)
        {
            if (col.GetInstanceID() == lastWaypointID)
                return;

            waypointIndex++;

            //if this is last waypoint, check loop value, if it isn't true then stop the car, else continue moving
            if (waypointIndex >= waypoints.Count)
            {
                if (loop)
                    waypointIndex = 0;
                else
                    stopped = true;
            }

            lastWaypointID = col.GetInstanceID();
        }

        //if car is standing in junction and its state is 'free' than start moving
        if (!accelerating && col.tag == "Junction" && currentJunction.free)
        {
            onJunction = false;
            accelerating = true;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        //if car enters in junction check its state and if it's not 'free' stop the car

        if (col.gameObject.tag == "Car" && col.GetComponent<CarAI>().isMain != this.isMain)
        {
            GameController.instance.GameOver(false);
        }

        if (col.tag == "Junction")
        {
            currentJunction = col.GetComponent<Junction>();

            if (!currentJunction)
            {
            }
            else if (!currentJunction.free)
            {
                onJunction = true;
                accelerating = false;
            }
        }
    }

    //if car's deceleration is set low and it wasn't able to stop in junction trigger then keep moving
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Junction")
        {
            onJunction = false;
            accelerating = true;
        }
    }
}