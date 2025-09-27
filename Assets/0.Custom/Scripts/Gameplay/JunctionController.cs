using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _0.Custom.Scripts;

//Controls junction trigger's states

public class JunctionController : MonoBehaviour
{
    public Junction[] trafficLight;
    public float greenLightTime = 5.0f; //time in seconds green light will be activated for each traffic light
    public float yellowLightTime = 2.0f; //time in seconds for yellow light

    private float timer = 0.0f;
    private int junctionIndex = 0;
    private bool waiting = false;
    public bool isAuto;

    private void Awake()
    {
        trafficLight = GetComponentsInChildren<Junction>(true);
    }

    public void ChangeState(GameController.MoveState state)
    {
        if (state == GameController.MoveState.Stop)
        {
            ChangeRed();
        }
        else
        {
            ChangeGreen();
        }
    }

    public void ChangeGreen()
    {
        foreach (var a in trafficLight)
        {
            a.free = true;
            a.waiting = false;
        }
    }

    public void ChangeYellow()
    {
        foreach (var a in trafficLight)
        {
            a.free = false;
            a.waiting = true;
        }
    }

    public void ChangeRed()
    {
        foreach (var a in trafficLight)
        {
            a.free = false;
            a.waiting = false;
        }
    }

    void Update()
    {
        if (!isAuto) return;
        timer += Time.deltaTime;

        //time for green light is over, change states on current and next traffic lights
        if (!waiting && timer >= greenLightTime)
        {
            trafficLight[junctionIndex].free = false;
            trafficLight[junctionIndex].waiting = true;

            if (junctionIndex == trafficLight.Length - 1)
                junctionIndex = 0;
            else
                junctionIndex++;

            trafficLight[junctionIndex].waiting = true;

            waiting = true;
        }

        //time for yellow light is over, change states on current and next traffic lights
        if (waiting && timer >= greenLightTime + yellowLightTime)
        {
            if (junctionIndex == 0)
                trafficLight[trafficLight.Length - 1].waiting = false;
            else
                trafficLight[junctionIndex - 1].waiting = false;

            trafficLight[junctionIndex].waiting = false;
            trafficLight[junctionIndex].free = true;

            waiting = false;
            timer = 0.0f;
        }
    }
}