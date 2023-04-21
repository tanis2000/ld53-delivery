using App.Generation;
using UnityEditor;
using UnityEngine;

namespace App.Editor
{
    [CustomEditor(typeof(SeededGenerator))]
    public class SeededGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var seededGenerator = (SeededGenerator)target;

            // Generate if empty
            if (!seededGenerator.Generated && seededGenerator.GenerateOnChange)
            {
                seededGenerator.Generate(seededGenerator.CurrentSeed);
            }

            // Pick random seed and generate
            if (GUILayout.Button("Randomize"))
            {
                seededGenerator.CurrentSeed = Random.Range(0, 100);
                seededGenerator.Generate(seededGenerator.CurrentSeed);
            }

            // Generate all steps
            if (GUILayout.Button("Generate"))
            {
                seededGenerator.Generate(seededGenerator.CurrentSeed);
            }

            // Clear
            if (GUILayout.Button("Clear"))
            {
                seededGenerator.Clear();
            }

            // Regenerate on parameter change on the SeededGenerator
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                seededGenerator.Regenerate();
            }
        }
   
    }
}