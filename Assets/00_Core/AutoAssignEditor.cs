using System.Reflection;
using UnityEditor;
using UnityEngine;

// true means this can be use for child classes of AutoAssignBase, except they have a custom separately
[CustomEditor(typeof(MonoBehaviour), true)]
public class AutoAssignEditor : Editor
{
    // A param to access to MonoBehaviour
    private MonoBehaviour TargetBase => (MonoBehaviour)target;

    // Draw Inspector interface 
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(10);

        var customButton = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Bold,
            padding = new(0, 0, 5, 5)
        };
        // Create "Auto Assign Components" button
        if (GUILayout.Button("Assign Components", customButton))
        {
            AutoAssignComponents(TargetBase);
        }

        EditorGUILayout.HelpBox("Click to assign [AutoAssign] on this script", MessageType.Info);
    }

    // Logic assign component using reflection
    private void AutoAssignComponents(MonoBehaviour targetScript)
    {
        // Undo
        Undo.RecordObject(targetScript, "Auto Assign Components");

        int assignedCount = 0;

        // Get all fields of this class
        FieldInfo[] fields = targetScript.GetType().GetFields(
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        foreach (FieldInfo field in fields)
        {
            // Check is this param have [AutoAssign}
            if (field.GetCustomAttribute<AutoAssignAttribute>() != null)
            {
                System.Type componentType = field.FieldType;
                Component component = null;

                // Get component from GameObject
                component = targetScript.gameObject.GetComponent(componentType);

                if (component == null)
                {
                    component = targetScript.gameObject.GetComponentInChildren(componentType, true);
                }


                if (component != null)
                {
                    field.SetValue(targetScript, component);
                    assignedCount++;
                }
                else
                {
                    //Debug.LogError($"[AutoAssign] Failed to find component {componentType.Name} on {targetScript.name}");
                    throw new System.Exception($"[AutoAssign] Failed to find component {componentType.Name} on {targetScript.name}");
                }
            }
        }

        EditorUtility.SetDirty(targetScript);
        Debug.Log($"[AutoAssign] Successfully assigned {assignedCount} components for {targetScript.GetType().Name} on {targetScript.name}.");
    }
}

public class AutoAssignAttribute : PropertyAttribute
{

}