using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PremiseData))]
public class PremiseEditor : Editor
{
    private PremiseData _myScript { get { return (PremiseData)target; } }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(25);
        EditorGUILayout.BeginVertical();
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Get Premise"))
        {
            _myScript.GetPremise();
        }

        if (GUILayout.Button("Reset Premise"))
        {
            _myScript.ResetPremise();
        }
        
        if (GUILayout.Button("Delete Premise"))
        {
            _myScript.DeletePremise();
        }

        if (GUILayout.Button("Save Premise"))
        {
            _myScript.SavePremise();
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }
}
