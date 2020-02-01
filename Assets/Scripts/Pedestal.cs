using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public PedestalType PedestalType;
}

public enum PedestalType
{
    Normal = 0,
    Repair = 1,
    Points = 2
}
