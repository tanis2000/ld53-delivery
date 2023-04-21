using UnityEngine;

namespace App.Generation
{
    public class SeededGenerator : MonoBehaviour
    {
        public int CurrentSeed = 0;
        public bool GenerateOnChange = true;
        public bool Generated = false;

        public void Clear()
        {
            Generated = false;
            var steps = GetComponentsInChildren<GenerationStep>();
            foreach (var step in steps)
            {
                step.Clear();
            }
        }
    
        public void Generate(int seed)
        {
            Clear();
            var steps = GetComponentsInChildren<GenerationStep>();
            foreach (var step in steps)
            {
                if (step.Enabled)
                {
                    step.Generate(seed);
                }
            }

            Generated = true;
#if UNITY_EDITOR
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
        }

        public void Regenerate()
        {
            Generate(CurrentSeed);
        }

        public void Regenerate(GenerationStep changedStep)
        {
            Generate(CurrentSeed);
        }
    }
}