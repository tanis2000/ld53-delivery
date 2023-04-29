using System;
using UnityEngine;

namespace App
{
    public class StopOnHit : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            var sling = other.gameObject.GetComponent<Sling>();
            if (sling != null)
            {
                sling.OutOfBoundsStop();
            }
        }
    }
}