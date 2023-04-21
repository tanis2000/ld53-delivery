using UnityEngine;

namespace App
{
    /// <summary>
    /// This is the global game state which should contain all the variables and constants shared across the whole game
    /// </summary>
    public class GameState : MonoBehaviour
    {
        private static GameState instance;
        
        public static GameState Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (GameState)FindObjectOfType(typeof(GameState));
                }

                return instance;
            }
        }
    }
}