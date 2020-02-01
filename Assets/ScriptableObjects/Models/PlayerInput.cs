using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerInput", fileName = "PlayerInput")]
public class PlayerInput : ScriptableObject
{
    public string HorizontalInputName;
    public string VerticalInputName;
    public string InteractionInputName;
    public string DashInputName;
}