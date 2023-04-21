using UnityEngine;

namespace App.Generation
{
    public class GenerationStep : MonoBehaviour
    {
        public bool Enabled = true;
    
        public virtual void Clear()
        {
        }

        public virtual void Generate(int seed)
        {
        }
    }
}