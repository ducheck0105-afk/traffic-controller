using System;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public class EndPointTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car" && other.GetComponent<CarAI>().waypoints.Contains(this.transform))
            {
                var car = other.GetComponent<CarAI>();
                var parent = transform.parent.GetComponent<WaypointsHolder>();
                parent.RemoveCar(car);

                Destroy(other.gameObject);
            }
        }
    }
}