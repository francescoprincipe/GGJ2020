using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviour), true)]
public class DraggablePointDrawer : Editor
{
    private float size = 0.1f;
    private GUIStyle style = new GUIStyle();

    private void OnEnable()
    {
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
    }

    public void OnSceneGUI()
    {
        var property = serializedObject.GetIterator();

        while (property.Next(true))
        {
            if (property.isArray && property.arrayElementType == "Vector3" && serializedObject.targetObject.GetType().GetField(property.name)?.GetCustomAttributes(typeof(DraggablePoint), false).Length > 0)
            {
                for (int i = 0; i < property.arraySize; i++)
                {
                    Draw(property.GetArrayElementAtIndex(i), i);
                }
            }
            else if (property.propertyType == SerializedPropertyType.Vector3 && serializedObject.targetObject.GetType().GetField(property.name)?.GetCustomAttributes(typeof(DraggablePoint), false).Length > 0)
            {
                Draw(property);
            }
        }
    }

    private void Draw(SerializedProperty element, int? i = null)
    {
        Handles.Label(new Vector3(element.vector3Value.x, element.vector3Value.y, element.vector3Value.z), (i != null) ? i.ToString() : element.name, style);
        element.vector3Value = Handles.PositionHandle(element.vector3Value, Quaternion.identity);
        Handles.DrawWireDisc(element.vector3Value, new Vector3(0, 0, 1), size);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

public class DraggablePoint : PropertyAttribute { }