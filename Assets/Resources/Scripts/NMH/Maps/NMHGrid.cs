using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMHGrid : MonoBehaviour
{
    public enum SideType
    {
        UP_TO_DOWN,
        LEFT_TO_RIGHT,
        FRONT_TO_BEHIND
    }

    public NMHCube cube;

    public SideType sideType;

    public readonly static float width = 2.54f;
    public readonly static float height = 2.54f;
    public readonly static float halfof = 1.27f;
    public readonly static float colOuterPos = 6.4f;
    public readonly static float colThickness = 0.1f;
}
