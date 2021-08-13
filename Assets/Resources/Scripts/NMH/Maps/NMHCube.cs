using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CubePos
{
    public CubePos(int _x, int _y, int _depth, NMHGrid.SideType _sideType)
    {
        x = _x;
        y = _y;
        depth = _depth;
        sideType = _sideType;
    }

    public int x;
    public int y;
    public int depth;

    public NMHGrid.SideType sideType;



    public override bool Equals(object obj)
    {
        if (!(obj is CubePos))
        {
            return false;
        }

        var pos = (CubePos)obj;
        return x == pos.x &&
               y == pos.y &&
               depth == pos.depth;
    }

    public override int GetHashCode()
    {
        var hashCode = 363242613;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        hashCode = hashCode * -1521134295 + depth.GetHashCode();
        return hashCode;
    }

    public static bool operator == (CubePos _cubePos1, CubePos _cubePos2)
    {
        if (_cubePos1.x == _cubePos2.x &&
           _cubePos1.y == _cubePos2.y &&
           _cubePos1.depth == _cubePos2.depth &&
           _cubePos1.sideType == _cubePos2.sideType)
        {
            return true;
        }
        else
            return false;
    }
    public static bool operator !=(CubePos _cubePos1, CubePos _cubePos2)
    {
        if (_cubePos1.x == _cubePos2.x &&
           _cubePos1.y == _cubePos2.y &&
           _cubePos1.depth == _cubePos2.depth &&
            _cubePos1.sideType == _cubePos2.sideType)
        {
            return false;
        }
        else
            return true;
    }

    public static CubePos operator + (CubePos _cubePos1, CubePos _cubePos2)
    {
        CubePos ret;
        ret.x = _cubePos1.x + _cubePos2.x;
        ret.y = _cubePos1.y + _cubePos2.y;
        ret.depth = _cubePos1.depth + _cubePos2.depth;
        ret.sideType = _cubePos1.sideType;

        return ret;
    }
}

public class NMHCube : MonoBehaviour
{
    public enum ColorType
    {
        NORMAL,
        GREEN,
        RED,
        YELLOW,
        BLUE
    }



    CubePos[] cubePos;

    NMHShip[] ships;

    Color defaultColor;


    //아래는 이벤트 함수======================================================================================

    private void Awake()
    {
        cubePos = new CubePos[3];

        ships = new NMHShip[3];

        for (int i = 0; i < 3; i++)
            ships[i] = null;

        defaultColor = GetComponent<MeshRenderer>().material.color;
    }



    //아래는 게터 세터======================================================================================

    public CubePos GetCubePos(NMHGrid.SideType _sidetype)
    {
        return cubePos[(int)_sidetype];
    }

    public void SetCubePos(NMHGrid.SideType _sidetype, int _x, int _y, int _depth)
    {
        cubePos[(int)_sidetype].x = _x;
        cubePos[(int)_sidetype].y = _y;
        cubePos[(int)_sidetype].depth = _depth;
        cubePos[(int)_sidetype].sideType = _sidetype;
    }

    public NMHShip[] GetShip()
    {
        return ships;
    }

    public void SetShip(NMHGrid.SideType _sideType, NMHShip _ship)
    {
        ships[(int)_sideType] = _ship;
    }

    public void SetColor(ColorType _colorType)
    {
        switch(_colorType)
        {
            case ColorType.NORMAL:
                GetComponent<MeshRenderer>().material.color = defaultColor;
                break;
            case ColorType.GREEN:
                GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case ColorType.RED:
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case ColorType.YELLOW:
                GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            case ColorType.BLUE:
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
        }
    }
}
