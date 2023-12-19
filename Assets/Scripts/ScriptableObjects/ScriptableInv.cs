using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableInv", menuName = "Inventaire", order = 0)]
public class ScriptableInv : ScriptableObject
{
    public int numLevel;
    public ScriptableInvDrag[] objectList;
}
