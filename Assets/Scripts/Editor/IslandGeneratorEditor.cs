using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IslandGen))]
public class IslandGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IslandGen.RandomSeed = EditorGUILayout.Toggle(false);
    }
}
