using System;
using System.Collections.Generic;
using GameBase.Audio;
using GameBase.SceneChanger;
using GameBase.Utils;
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
        [SerializeField] private int NumberOfTutorialLevels = 3;
        
        private static Game instance;
        private bool isPaused;
        private State currentState;
        private int currentLevel;
        private SubmitScore submitScore;
        private XRandom rnd;

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
            rnd = new XRandom(42);
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
            AudioSystem.Instance().Play("Soundmain-theme");
        }

        public void SwitchState(State state)
        {
            currentState = state;
        }

        private void StartFirstLevel()
        {
            currentLevel = 0;
            LevelSystems[0].StartLevel(this, true);
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
                LevelSystems[currentLevel].StartLevel(this, currentLevel <= NumberOfTutorialLevels-1);
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.L) && Application.isEditor)
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

        public XRandom GetRnd()
        {
            return rnd;
        }
    }
}