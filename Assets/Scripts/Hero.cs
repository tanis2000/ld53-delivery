using System;
using App.Input;
using GameBase.Animations.Actors;
using GameBase.Effects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App
{
    public class Hero : MonoBehaviour, GameInput.IGameActions
    {
        private Leaner leaner;
        private Mover mover;
        private Hopper hopper;
        private Stretcher stretcher;
        private Flipper flipper;
        private GameInput gameInput;
        
        private int facing = 1;

        private void Awake()
        {
            leaner = GetComponent<Leaner>();
            mover = GetComponent<Mover>();
            hopper = GetComponent<Hopper>();
            stretcher = GetComponent<Stretcher>();
            flipper = GetComponent<Flipper>();
            gameInput = new GameInput();
            gameInput.Game.SetCallbacks(this);
            gameInput.Game.Enable();
            //gameActions.Game.Move.performed += OnMove;
        }

        // private void OnMove(InputAction.CallbackContext context)
        // {
        //     
        // }
        
        private void Update()
        {
            // var movement = gameActions.Game.Move.ReadValue<Vector2>();
            // Debug.Log(movement);
            // if (Input.GetKeyDown(KeyCode.D))
            // {
            //     leaner.Execute(Vector3.right);
            //     mover.Execute(transform.position + Vector3.right * 1f);
            //     hopper.Execute();
            //     stretcher.Execute();
            //     facing = 1;
            //     flipper.Execute(facing);
            // } else if (Input.GetKeyDown(KeyCode.A))
            // {
            //     leaner.Execute(Vector3.left);
            //     mover.Execute(transform.position + Vector3.left * 1f);
            //     hopper.Execute();
            //     stretcher.Execute();
            //     facing = -1;
            //     flipper.Execute(facing);
            // } else if (Input.GetKeyDown(KeyCode.W))
            // {
            //     leaner.Execute(Vector3.up);
            //     mover.Execute(transform.position + Vector3.up * 1f);
            //     hopper.Execute();
            //     stretcher.Execute();
            // } else if (Input.GetKeyDown(KeyCode.S))
            // {
            //     leaner.Execute(Vector3.down);
            //     mover.Execute(transform.position + Vector3.down * 1f);
            //     hopper.Execute();
            //     stretcher.Execute();
            // } else 
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                EffectsSystem.AddTextPopup("500", transform.position + Vector3.up);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var move = context.ReadValue<Vector2>();
            if (move.x > 0)
            {
                leaner.Execute(Vector3.right);
                mover.Execute(transform.position + Vector3.right * 1f);
                hopper.Execute();
                stretcher.Execute();
                facing = 1;
                flipper.Execute(facing);
            } else if (move.x < 0)
            {
                leaner.Execute(Vector3.left);
                mover.Execute(transform.position + Vector3.left * 1f);
                hopper.Execute();
                stretcher.Execute();
                facing = -1;
                flipper.Execute(facing);
            } else if (move.y > 0)
            {
                leaner.Execute(Vector3.up);
                mover.Execute(transform.position + Vector3.up * 1f);
                hopper.Execute();
                stretcher.Execute();
            } else if (move.y < 0)
            {
                leaner.Execute(Vector3.down);
                mover.Execute(transform.position + Vector3.down * 1f);
                hopper.Execute();
                stretcher.Execute();
            }
        }
    }
}