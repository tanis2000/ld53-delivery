using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App
{
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] private int NeededDeliveries = 1;

        private int selectedThrowable = 0;
        private int reachedThrowables = 0;
        private Game game;
        private List<Sling> throwables = new List<Sling>();
        
        private void OnEnable()
        {
            throwables.Clear();
            throwables = GetComponentsInChildren<Sling>().ToList();
            Game.Instance.UpdateDeliveries(NeededDeliveries);
        }

        public void StartLevel(Game g, bool showTrajectoryPoints)
        {
            game = g;
            gameObject.SetActive(true);
            StartCoroutine(StartLevel(showTrajectoryPoints));
        }
        
        private IEnumerator StartLevel(bool showTrajectoryPoints)
        {
            yield return new WaitForSeconds(1);
            StopAllThrowables();
            if (!showTrajectoryPoints)
            {
                HideAllTrajectoryPoints();
            }
            throwables.First().StartGame(this);
        }
        
        private void StopAllThrowables()
        {
            foreach (var throwable in throwables)
            {
                throwable.StopGame();
            }
        }

        private void HideAllTrajectoryPoints()
        {
            foreach (var throwable in throwables)
            {
                throwable.RemoveTrajectoryPoints();
            }
        }

        public void Next(bool reached)
        {
            if (reached)
            {
                reachedThrowables++;
            }
            
            selectedThrowable++;
            Debug.Log($"selected: {selectedThrowable}");
            if (selectedThrowable >= throwables.Count)
            {
                EndLevel();
                return;
            }
            
            throwables[selectedThrowable].StartGame(this);
        }

        private void EndLevel()
        {
            if (reachedThrowables >= NeededDeliveries)
            {
                Debug.Log("Level completed");
                game.NextLevel();
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Game over");
                game.GameOver();
            }
        }
    }
}