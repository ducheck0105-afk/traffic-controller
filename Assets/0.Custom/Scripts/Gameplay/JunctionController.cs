using UnityEngine;
using System.Collections;
using _0.Custom.Scripts;

//Controls junction trigger's states

public class JunctionController : MonoBehaviour
{
    public Junction[] junctions;
    public float greenLightTime = 5.0f; //time in seconds green light will be activated for each traffic light
    public float yellowLightTime = 2.0f; //time in seconds for yellow light

    private float timer = 0.0f;
    private int junctionIndex = 0;
    private bool waiting = false;
    public bool isAuto;

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
        foreach (var a in junctions)
        {
            a.free = true;
            a.waiting = false;
        }
    }

    public void ChangeYellow()
    {
        foreach (var a in junctions)
        {
            a.free = false;
            a.waiting = true;
        }
    }

    public void ChangeRed()
    {
        foreach (var a in junctions)
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
            junctions[junctionIndex].free = false;
            junctions[junctionIndex].waiting = true;

            if (junctionIndex == junctions.Length - 1)
                junctionIndex = 0;
            else
                junctionIndex++;

            junctions[junctionIndex].waiting = true;

            waiting = true;
        }

        //time for yellow light is over, change states on current and next traffic lights
        if (waiting && timer >= greenLightTime + yellowLightTime)
        {
            if (junctionIndex == 0)
                junctions[junctions.Length - 1].waiting = false;
            else
                junctions[junctionIndex - 1].waiting = false;

            junctions[junctionIndex].waiting = false;
            junctions[junctionIndex].free = true;

            waiting = false;
            timer = 0.0f;
        }
    }
}