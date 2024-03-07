using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Yapper", order = 2)]
public class YapperState : ScriptableObject
{
    [TextArea]
    [SerializeReference] public string yapImg;
}
