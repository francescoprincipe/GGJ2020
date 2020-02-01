using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerInfo player1Info;
    public PlayerInfo player2Info;

    public TextMeshProUGUI player1Points_text;
    public TextMeshProUGUI player2Points_text;

    private void FixedUpdate()
    {
        player1Points_text.text = player1Info.points.ToString();
        player2Points_text.text = player2Info.points.ToString();
    }
}
