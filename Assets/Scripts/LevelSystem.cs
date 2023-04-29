using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App
{
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] private List<Sling> Throwables;
        [SerializeField] private int NeededDeliveries = 1;

        private int selectedThrowable = 0;
        private int reachedThrowables = 0;
        private Game game;

        private void OnEnable()
        {
            Throwables.Clear();
            Throwables = GetComponentsInChildren<Sling>().ToList();
            Game.Instance.UpdateDeliveries(NeededDeliveries);
        }

        public void StartLevel(Game g)
        {
            game = g;
            gameObject.SetActive(true);
            StartCoroutine(StartLevel());
        }
        
        private IEnumerator StartLevel()
        {
            yield return new WaitForSeconds(1);
            StopAllThrowables();    
            Throwables.First().StartGame(this);
        }
        
        private void StopAllThrowables()
        {
            foreach (var throwable in Throwables)
            {
                throwable.StopGame();
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
            if (selectedThrowable >= Throwables.Count)
            {
                EndLevel();
                return;
            }
            
            Throwables[selectedThrowable].StartGame(this);
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