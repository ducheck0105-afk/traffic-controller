using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _0.Custom.Scripts;
using _0.Custom.Scripts.Gameplay;
using Random = UnityEngine.Random;

public class WaypointsHolder : MonoBehaviour
{
    public bool drawLines = true;
    public Color drawColor = Color.green;

    public float spawnTimeMin = 5;
    public float spawnTimeMax = 10f;
    public CarAI.CarFlow flow;
    public int maxCar;
    public List<CarAI> cars = new List<CarAI>();

    private float spawnTimer;
    public bool ignore;
    private Coroutine IE;

    private void Start()
    {
        if (ignore) return;
        IE = StartCoroutine(SpawnCarIE());
    }

    public void RemoveCar(CarAI obj)
    {
        cars.Remove(obj);
        if (cars.Count == 0)
        {
            if (IE != null) StopCoroutine(IE);
            IE = StartCoroutine(SpawnCarIE());
        }
    }

    public void SpawnCar()
    {
        if (ignore || GameController.instance.stopAll) return;
        var carParent = GameController.instance.carParent;
        var prefab = CarData.Instance.GetCarPrefab(flow);
        var obj = Instantiate(prefab, carParent);
        foreach (Transform child in transform)
        {
            obj.waypoints.Add(child);
        }

        obj.transform.position = transform.GetChild(0).position;
        obj.gameObject.SetActive(true);
        cars.Add(obj);
    }

    private IEnumerator SpawnCarIE()
    {
        Debug.Log("SpawnOneMore");
        if (cars.Count >= maxCar) yield break;

        while (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
            yield return null;
        }

        SpawnCar();
        spawnTimer = Random.Range(spawnTimeMin, spawnTimeMax);
        StartCoroutine(SpawnCarIE());
    }

    private IEnumerator SpawnOneMore()
    {
        Debug.Log("SpawnOneMore");
        float time = 2;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        SpawnCar();
    }
}