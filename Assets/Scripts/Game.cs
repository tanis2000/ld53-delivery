using System;
using System.Collections.Generic;
using GameBase.Audio;
using GameBase.SceneChanger;
using TMPro;
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

        [SerializeField] private List<LevelSystem> LevelSystems;
        [SerializeField] private GameObject GameOverUI;
        [SerializeField] private GameObject GameCompletedUI;
        [SerializeField] private TMP_Text DeliveriesText;
        
        private static Game instance;
        private bool isPaused;
        private State currentState;
        private int currentLevel;
        private SubmitScore submitScore;

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
            submitScore = FindObjectOfType<SubmitScore>();
            PlayMainTheme();
            StartFirstLevel();
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

        private void StartFirstLevel()
        {
            currentLevel = 0;
            LevelSystems[0].StartLevel(this);
        }

        public void NextLevel()
        {
            currentLevel++;
            if (currentLevel >= LevelSystems.Count)
            {
                Debug.Log("Game over. No more levels to play.");
                GameCompleted();
            }
            else
            {
                LevelSystems[currentLevel].StartLevel(this);
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            {
                LevelSystems[currentLevel].gameObject.SetActive(false);
                NextLevel();
            }
        }

        private void SubmitScore()
        {
            submitScore.SubmitFakeScore();
        }

        public void GameOver()
        {
            SubmitScore();
            ShowGameOverUI();
        }

        public void GameCompleted()
        {
            SubmitScore();
            ShowGameCompletedUI();
        }

        private void ShowGameOverUI()
        {
            GameOverUI.SetActive(true);
        }
        
        private void ShowGameCompletedUI()
        {
            GameCompletedUI.SetActive(true);
        }

        public void GoToMainMenu()
        {
            SceneChanger.Instance.ChangeScene("MainMenu");
        }

        public void UpdateDeliveries(int num)
        {
            DeliveriesText.text = $"{num}";
        }
    }
}