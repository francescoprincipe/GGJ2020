using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WorkbenchInput", fileName = "WorkbenchInput")]
public class WorkbenchInput : ScriptableObject
{
    public List<KeyCode> EventButtons;
}
