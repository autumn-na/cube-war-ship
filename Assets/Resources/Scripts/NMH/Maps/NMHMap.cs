using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMHMap : MonoBehaviour
{
    public enum RotationDirection
    {
        CLOCKWISE,
        COUNTER_CLOCKWISE,
    }


    public readonly static int horizontal = 5;
    public readonly static int vertical = 5;
    public readonly static int height = 5;

    public NMHCube[] cube;
    public Transform rotatingParent;

    NMHGrid.SideType curRotatingSidetype;

    void Start ()
    {
        InitCube();
    }

	void Update ()
    {

    }



    void InitCube()
    {
        for (int i_cube = 0; i_cube < 125; i_cube++)
        {
            cube[i_cube].SetCubePos(NMHGrid.SideType.UP_TO_DOWN, 4- (i_cube % 5), i_cube / 25, (i_cube / 5) % 5);
            cube[i_cube].SetCubePos(NMHGrid.SideType.LEFT_TO_RIGHT, 4 - (i_cube / 25) % 5, 4 - (i_cube / 5 % 5), 4 - (i_cube % 5) );
            cube[i_cube].SetCubePos(NMHGrid.SideType.FRONT_TO_BEHIND, 4 - (i_cube % 5), 4 - (i_cube / 5 % 5), i_cube / 25);
        }
    }

    void ResetRotating()
    {
        int cnt = 0;

        Transform[] tr = new Transform[25];

        foreach (Transform child in rotatingParent)
        {
            tr[cnt] = child;

            cnt++;
        }

        for(int i = 0; i < cnt; i++)
        {
            tr[i].parent = transform;
        }

        SetAllCubeColorToNormal();

        rotatingParent.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    public bool SelectRotatingChild(NMHCube _cube1, NMHCube _cube2, NMHGrid.SideType _side1, NMHGrid.SideType _side2)
    {
        ResetRotating();

        int rotateDepth = -1;

        bool isOK = false;

        for (int i_pos = 0; i_pos < 3; i_pos++)
        {
            if (_cube1.GetCubePos((NMHGrid.SideType)i_pos).sideType == _cube2.GetCubePos((NMHGrid.SideType)i_pos).sideType)

                if (_cube1.GetCubePos((NMHGrid.SideType)i_pos).depth == _cube2.GetCubePos((NMHGrid.SideType)i_pos).depth &&
                    !((_side1 == _side2) && (_side2 == (NMHGrid.SideType)i_pos)))
                {
                    if (_side1 == _side2)
                    {
                        if (!(_cube1.GetCubePos((NMHGrid.SideType)i_pos).x == _cube2.GetCubePos((NMHGrid.SideType)i_pos).x || _cube1.GetCubePos((NMHGrid.SideType)i_pos).y == _cube2.GetCubePos((NMHGrid.SideType)i_pos).y))
                        {
                            continue;
                        }
                    }

                    rotateDepth = _cube1.GetCubePos((NMHGrid.SideType)i_pos).depth;
                    curRotatingSidetype = (NMHGrid.SideType)i_pos;

                    isOK = true;
                }
        }

        if (isOK == false)
            return false;

        CheckRotatingParentDir();

        for (int i_cube = 0; i_cube < cube.Length; i_cube++)
        {
            for (int j_pos = 0; j_pos < 3; j_pos++)
            {
                if (cube[i_cube].GetCubePos((NMHGrid.SideType)j_pos).sideType == curRotatingSidetype &&
                    cube[i_cube].GetCubePos((NMHGrid.SideType)j_pos).depth == rotateDepth)
                {
                    cube[i_cube].transform.parent = rotatingParent;

                    cube[i_cube].SetColor(NMHCube.ColorType.GREEN);
                }
            }
        }

        return true;
    }

    public void CheckRotatingParentDir()
    {
        switch(curRotatingSidetype)
        {
            case NMHGrid.SideType.FRONT_TO_BEHIND:
                rotatingParent.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                break;
            case NMHGrid.SideType.LEFT_TO_RIGHT:
                rotatingParent.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
                break;
            case NMHGrid.SideType.UP_TO_DOWN:
                break;
        }
    }

    public void Rotate()
    {
        Vector2 mouseMoveDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseMoveDir.Normalize();

        if (mouseMoveDir.x != 0 || mouseMoveDir.y != 0)
        {
            float angle = Vector2.SignedAngle(rotatingParent.transform.up, mouseMoveDir);

            if (angle > 0 )
                rotatingParent.Rotate(Vector3.up, 2, Space.Self);
            else if (angle < 0)
                rotatingParent.Rotate(Vector3.up, -2, Space.Self);

            //Debug.Log(rotatingParent.localEulerAngles);
        }
    }

    public void FinishRotate()
    {
        switch(curRotatingSidetype)
        {
            case NMHGrid.SideType.FRONT_TO_BEHIND:
                if (rotatingParent.localEulerAngles.x % 90f >= 45f)
                {
                    float plus = 90f - rotatingParent.localEulerAngles.x % 90f;

                    rotatingParent.localEulerAngles = new Vector3(rotatingParent.localEulerAngles.x + plus, rotatingParent.localEulerAngles.y, rotatingParent.localEulerAngles.z);
                }
                else
                {
                    float minus = rotatingParent.localEulerAngles.x % 90f;

                    rotatingParent.localEulerAngles = new Vector3(rotatingParent.localEulerAngles.x - minus, rotatingParent.localEulerAngles.y, rotatingParent.localEulerAngles.z);
                }

                if (rotatingParent.localEulerAngles.x == 0f)
                    RotateCubeData(RotationDirection.COUNTER_CLOCKWISE);
                else if (rotatingParent.localEulerAngles.x == 180f)
                    RotateCubeData(RotationDirection.CLOCKWISE);

                if (rotatingParent.localEulerAngles.x != 90f)
                    ResetRotating();

                break;
            case NMHGrid.SideType.LEFT_TO_RIGHT:
                if (rotatingParent.localEulerAngles.x % 90f >= 45f)
                {
                    float plus = 90f - rotatingParent.localEulerAngles.x % 90f;

                    rotatingParent.localEulerAngles = new Vector3(rotatingParent.localEulerAngles.x + plus, rotatingParent.localEulerAngles.y, rotatingParent.localEulerAngles.z);
                }
                else
                {
                    float minus = rotatingParent.localEulerAngles.x % 90f;

                    rotatingParent.localEulerAngles = new Vector3(rotatingParent.localEulerAngles.x - minus, rotatingParent.localEulerAngles.y, rotatingParent.localEulerAngles.z);
                }

                if (rotatingParent.localEulerAngles.x == 90f)
                    RotateCubeData(RotationDirection.CLOCKWISE);
                else if (rotatingParent.localEulerAngles.x == 270f)
                    RotateCubeData(RotationDirection.COUNTER_CLOCKWISE);

                if (rotatingParent.localEulerAngles.x != 0f)
                    ResetRotating();

                break;
            case NMHGrid.SideType.UP_TO_DOWN:
                if (rotatingParent.localEulerAngles.y % 90f >= 45f)
                {
                    float plus = 90f - rotatingParent.localEulerAngles.y % 90f;

                    rotatingParent.localEulerAngles = new Vector3(rotatingParent.localEulerAngles.x, rotatingParent.localEulerAngles.y + plus, rotatingParent.localEulerAngles.z);
                }
                else
                {
                    float minus = rotatingParent.localEulerAngles.y % 90f;

                    rotatingParent.localEulerAngles = new Vector3(rotatingParent.localEulerAngles.x, rotatingParent.localEulerAngles.y - minus, rotatingParent.localEulerAngles.z);
                }

                if (rotatingParent.localEulerAngles.y == 90f)
                    RotateCubeData(RotationDirection.CLOCKWISE);
                else if (rotatingParent.localEulerAngles.y == 270f)
                    RotateCubeData(RotationDirection.COUNTER_CLOCKWISE);

                if (rotatingParent.localEulerAngles.y != 0f)
                    ResetRotating();

                break;
        }
    }

    public void RotateCubeData(RotationDirection _dir)
    {
        //CubePos[,] poses = new CubePos[5, 4];
        //CubePos[] firstPoses = new CubePos[5];

        //if (_dir == RotationDirection.CLOCKWISE)
        //{
        //    foreach(Transform tr in rotatingParent)
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            CubePos pos = tr.GetComponent<NMHCube>().GetCubePos((NMHGrid.SideType)i);

        //            if ((NMHGrid.SideType)i == curRotatingSidetype)
        //            {
        //                tr.GetComponent<NMHCube>().SetCubePos((NMHGrid.SideType)i, pos.y, pos.x, pos.depth);
        //            }
        //            else
        //            {
        //                NMHGrid.SideType firstType = new NMHGrid.SideType();
        //                NMHGrid.SideType secondType = new NMHGrid.SideType();

                        

        //                switch (curRotatingSidetype)
        //                {
        //                    case NMHGrid.SideType.UP_TO_DOWN:
        //                        firstType = NMHGrid.SideType.LEFT_TO_RIGHT;
        //                        secondType = NMHGrid.SideType.FRONT_TO_BEHIND;

        //                        if (pos.sideType == firstType && pos.depth == 0)    poses[pos.x, 0] = pos; firstPoses[pos.x] = pos;
        //                        if (pos.sideType == secondType && pos.depth == 0)   poses[pos.x, 1] = pos;
        //                        if (pos.sideType == firstType && pos.depth == 4)    poses[pos.x, 2] = pos;
        //                        if (pos.sideType == secondType && pos.depth == 4)   poses[pos.x, 3] = pos;

        //                        break;
        //                    case NMHGrid.SideType.LEFT_TO_RIGHT:
        //                        firstType = NMHGrid.SideType.UP_TO_DOWN;
        //                        secondType = NMHGrid.SideType.FRONT_TO_BEHIND;
        //                        break;
        //                    case NMHGrid.SideType.FRONT_TO_BEHIND:
        //                        firstType = NMHGrid.SideType.LEFT_TO_RIGHT;
        //                        secondType = NMHGrid.SideType.UP_TO_DOWN;
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //}

        //if (_dir == RotationDirection.COUNTER_CLOCKWISE)
        //{
        //    foreach (Transform tr in rotatingParent)
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            CubePos pos = tr.GetComponent<NMHCube>().GetCubePos((NMHGrid.SideType)i);

        //            tr.GetComponent<NMHCube>().SetCubePos((NMHGrid.SideType)i, 4 - pos.y, pos.x, pos.depth);
        //        }
        //    }
        //}
    }

    public bool CheckCubeIsRotate(NMHCube _cube)
    {
        NMHCube[] cubes;

        cubes = rotatingParent.GetComponentsInChildren<NMHCube>();

        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i] == _cube)
                return true;
        }

        return false;
    }

    public void SetAllCubeColorToNormal()
    {
        for(int i = 0; i < cube.Length; i++)
        {
            cube[i].SetColor(NMHCube.ColorType.NORMAL);
        }
    }
}
