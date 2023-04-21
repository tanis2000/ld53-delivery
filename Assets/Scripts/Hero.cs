using System;
using GameBase.Animations.Actors;
using GameBase.Effects;
using UnityEngine;

namespace App
{
    public class Hero : MonoBehaviour
    {
        private Leaner leaner;
        private Mover mover;
        private Hopper hopper;
        private Stretcher stretcher;
        private Flipper flipper;
        
        private int facing = 1;

        private void Awake()
        {
            leaner = GetComponent<Leaner>();
            mover = GetComponent<Mover>();
            hopper = GetComponent<Hopper>();
            stretcher = GetComponent<Stretcher>();
            flipper = GetComponent<Flipper>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                leaner.Execute(Vector3.right);
                mover.Execute(transform.position + Vector3.right * 1f);
                hopper.Execute();
                stretcher.Execute();
                facing = 1;
                flipper.Execute(facing);
            } else if (Input.GetKeyDown(KeyCode.A))
            {
                leaner.Execute(Vector3.left);
                mover.Execute(transform.position + Vector3.left * 1f);
                hopper.Execute();
                stretcher.Execute();
                facing = -1;
                flipper.Execute(facing);
            } else if (Input.GetKeyDown(KeyCode.W))
            {
                leaner.Execute(Vector3.up);
                mover.Execute(transform.position + Vector3.up * 1f);
                hopper.Execute();
                stretcher.Execute();
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                leaner.Execute(Vector3.down);
                mover.Execute(transform.position + Vector3.down * 1f);
                hopper.Execute();
                stretcher.Execute();
            } else if (Input.GetKeyDown(KeyCode.Space))
            {
                EffectsSystem.AddTextPopup("500", transform.position + Vector3.up);
            }
        }
    }
}