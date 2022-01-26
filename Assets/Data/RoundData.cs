using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundData", menuName = "Data/RoundData", order = 1)]
public class RoundData : ScriptableObject
{
    public Sprite[] spritesToFind;
    public int[] buttonsNumber;
}
