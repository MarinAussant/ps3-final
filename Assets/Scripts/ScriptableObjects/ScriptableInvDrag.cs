using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableInvDrag", menuName = "DuoInventaireObject", order = 0)]
public class ScriptableInvDrag : ScriptableObject
{
    public string objectName;
    public int id;
    public GameObject objectDraggable;
    public Sprite imageRef;
    [TextArea(15, 20)]
    public string infoText;

}
