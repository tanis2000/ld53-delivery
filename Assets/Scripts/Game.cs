using System;
using GameBase.Audio;
using UnityEngine;

namespace App
{
    public class Game : MonoBehaviour
    {
        public enum State
        {
            Running,
            Win,
            GameOver,
        }
        private static Game instance;
        private bool isPaused;
        private State currentState;

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (Game)FindObjectOfType(typeof(Game));
                }

                return instance;
            }
        }
        
        public State GetCurrentState() => currentState;

        private void OnEnable()
        {
            instance = this;
            currentState = State.Running;
            PlayMainTheme();
        }

        public bool IsPaused()
        {
            return isPaused;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void PlayMainTheme()
        {
            AudioSystem.Instance().Play("SoundTheme1");
        }

        public void SwitchState(State state)
        {
            currentState = state;
        }
    }
}