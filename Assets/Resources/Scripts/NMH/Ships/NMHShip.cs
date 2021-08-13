using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    PLAYER_1,
    PLAYER_2
}

public enum ShipType
{
    /// <summary>
    /// 1X1 Ship.
    /// </summary>
    FAST_SHIP,
    /// <summary>
    /// 1X2 Ship.
    /// </summary>
    PATROL_SHIP,
    /// <summary>
    /// 1X3 Ship.
    /// </summary>
    DESTROYER,
    /// <summary>
    /// 2X2 Ship.
    /// </summary>
    CRUISER,
    /// <summary>
    /// 1X4 Ship
    /// </summary>
    BATTLE_SHIP,
    /// <summary>
    /// 1X3 Ship
    /// </summary>
    SUBMARINE
}

public enum Direction
{
    FORWARD,
    BACK,
    LEFT,
    RIGHT
}

public class NMHShip : MonoBehaviour
{
    public ShipType shipType;           //프리펩에서 지정
    public Direction direction;         //기본적으로 LEFT
    public PlayerType playertype;       //어떤 플레이어의 것인가?

    CubePos pivotPos;
    CubePos pivotLastPos;
    CubePos[] pos;

    List<CubePos> attackPoses;

    bool[] isAlive;

    float padding;



    private void Awake()
    {
        Init();
    }

    private void Start()
    {
       
    }

    private void Update()
    {
       
    }

    //아래는 함수================================================================================================================================================

    public void Init()         //초기화
    {
        attackPoses = new List<CubePos>();

        switch (shipType)
        {
            case ShipType.FAST_SHIP:
                pos = new CubePos[1];
                isAlive = new bool[1];
                padding = 0;
                break;
            case ShipType.PATROL_SHIP:
                pos = new CubePos[2];
                isAlive = new bool[2];
                padding = 0.35f;
                break;
            case ShipType.DESTROYER:
                pos = new CubePos[3];
                isAlive = new bool[3];
                padding = 1.1f;
                break;
            case ShipType.CRUISER:
                pos = new CubePos[4];
                isAlive = new bool[4];
                padding = 0.2f;
                break;
            case ShipType.BATTLE_SHIP:
                pos = new CubePos[4];
                isAlive = new bool[4];
                padding = 1.5f;
                break;
            case ShipType.SUBMARINE:
                pos = new CubePos[3];
                isAlive = new bool[3];
                padding = 0f;
                break;
        }

        for (int i = 0; i < isAlive.Length; i++)
            isAlive[i] = true;
    }

    void SetAttackPoses()       //공격범위 체크
    {
        switch(shipType)
        {
            case ShipType.FAST_SHIP:
                for (int i = 0; i < NMHGameMng.GetInstance().map.cube.Length; i ++)
                {
                    CubePos check = NMHGameMng.GetInstance().map.cube[i].GetCubePos(pivotPos.sideType);

                    if (check.depth == 0 || check.depth == 4)
                    {
                        if (check.depth == pos[0].depth)
                            attackPoses.Add(check);
                    }
                }
                break;
            case ShipType.PATROL_SHIP:
                for (int i = 0; i < NMHGameMng.GetInstance().map.cube.Length; i++)
                {
                    for (int j = 0; j < 3; j ++)
                    {
                        CubePos check = NMHGameMng.GetInstance().map.cube[i].GetCubePos((NMHGrid.SideType)j);

                        if (check.depth == pos[0].depth ||
                            (check.depth == pivotLastPos.depth && check.sideType == pivotLastPos.sideType))
                        {
                            attackPoses.Add(check);
                        }
                    }
                }
                break;
            case ShipType.DESTROYER:
                for (int i = 0; i < NMHGameMng.GetInstance().map.cube.Length; i++)
                {
                    for (int j_side = 0; j_side < 3; j_side++)
                    {
                        CubePos check = NMHGameMng.GetInstance().map.cube[i].GetCubePos((NMHGrid.SideType)j_side);

                        if(check.depth == 0 || check.depth == 4)
                        {
                            if (check.sideType == pivotPos.sideType)
                            {
                                if (check.depth == pivotPos.depth)
                                    attackPoses.Add(check);
                            }
                            else
                                attackPoses.Add(check);

                        }
                    }
                }
                break;
            case ShipType.BATTLE_SHIP:
                for (int i = 0; i < NMHGameMng.GetInstance().map.cube.Length; i++)
                {
                    for (int j_side = 0; j_side < 3; j_side++)
                    {
                        CubePos check = NMHGameMng.GetInstance().map.cube[i].GetCubePos((NMHGrid.SideType)j_side);

                        if (check.depth == 0 || check.depth == 4)
                        {
                            attackPoses.Add(check);
                        }
                    }
                }
                break;
            case ShipType.SUBMARINE:
                for (int i = 0; i < NMHGameMng.GetInstance().map.cube.Length; i++)
                {
                    for (int j_side = 0; j_side < 3; j_side++)
                    {
                        CubePos check = NMHGameMng.GetInstance().map.cube[i].GetCubePos((NMHGrid.SideType)j_side);

                        if (pivotPos.sideType == NMHGrid.SideType.FRONT_TO_BEHIND)
                        {
                            if ((check.depth == 0 || check.depth == 4) &&
                                 (check.x == pivotPos.x && (check.sideType == NMHGrid.SideType.FRONT_TO_BEHIND || check.sideType == NMHGrid.SideType.UP_TO_DOWN) ||
                                 (check.y == pivotPos.y && (check.sideType == NMHGrid.SideType.LEFT_TO_RIGHT || check.sideType == NMHGrid.SideType.FRONT_TO_BEHIND))))
                            {
                                attackPoses.Add(check);
                            }
                        }
                        else if (pivotPos.sideType == NMHGrid.SideType.UP_TO_DOWN)
                        {
                            if (check.depth == 0 || check.depth == 4)
                            {
                                if (check.x == pivotPos.x &&
                                    (check.sideType == NMHGrid.SideType.FRONT_TO_BEHIND || check.sideType == NMHGrid.SideType.UP_TO_DOWN))
                                {
                                    attackPoses.Add(check);
                                }
                                else if (check.y == pivotPos.y &&
                                        (check.sideType == NMHGrid.SideType.UP_TO_DOWN))
                                {
                                    attackPoses.Add(check);
                                }
                                else if (check.depth == 0 &&
                                            check.x == 4 - pivotPos.y &&
                                            check.sideType == NMHGrid.SideType.LEFT_TO_RIGHT)
                                {
                                    attackPoses.Add(check);
                                }
                                else if (check.depth == 4 &&
                                            check.x == 4 - pivotPos.y &&
                                            check.sideType == NMHGrid.SideType.LEFT_TO_RIGHT)
                                {
                                    attackPoses.Add(check);
                                }
                            }
                        }
                        else if (pivotPos.sideType == NMHGrid.SideType.LEFT_TO_RIGHT)
                        {
                            if (check.depth == 0 || check.depth == 4)
                            {
                                if  ((check.x == pivotPos.x || check.y == pivotPos.y) &&
                                    (check.sideType == NMHGrid.SideType.LEFT_TO_RIGHT))
                                {
                                    attackPoses.Add(check);
                                }
                                else if (check.y == 4 - pivotPos.x &&
                                        (check.sideType == NMHGrid.SideType.UP_TO_DOWN))
                                {
                                    attackPoses.Add(check);
                                }
                                else if (check.y == (4 - pivotPos.x) &&
                                        (check.sideType == NMHGrid.SideType.FRONT_TO_BEHIND))
                                {
                                    attackPoses.Add(check);
                                }
                            }
                        }
                    }
                }
                break;
            case ShipType.CRUISER:
                for (int i = 0; i < NMHGameMng.GetInstance().map.cube.Length; i++)
                {
                    for (int j_side = 0; j_side < 3; j_side++)
                    {
                        CubePos check = NMHGameMng.GetInstance().map.cube[i].GetCubePos((NMHGrid.SideType)j_side);

                        if  (check.depth == 0 || check.depth == 4)   
                        {
                            if (check.sideType == pos[0].sideType && check.depth == pivotPos.depth)
                                continue;

                            attackPoses.Add(check);
                        }
                    }
                }
                break;
        }
    }
    
    public void SetGridByDirection(CubePos _cubePos)      //이 배가 점유하는 그리드 변경         
    {
        switch (shipType)
        {
            case ShipType.FAST_SHIP:
            case ShipType.PATROL_SHIP:
            case ShipType.DESTROYER:
            case ShipType.BATTLE_SHIP:
            case ShipType.SUBMARINE:

                pivotPos = GetLimitedPos(_cubePos);

                pos[pos.Length / 2] = pivotPos;

                switch (direction)
                {
                    case Direction.FORWARD:
                        for (int i_grid = pos.Length / 2 - 1; i_grid >= 0; i_grid--)   pos[i_grid] = pivotPos + new CubePos(0, pos.Length / 2 - i_grid, 0, pivotPos.sideType);  //mid - 1 --> 0  (수가 작을수록 머리에 가까움)
                        for (int i_grid = pos.Length / 2 + 1; i_grid < pos.Length; i_grid++) pos[i_grid] = pivotPos + new CubePos(0, pos.Length / 2 - i_grid, 0, pivotPos.sideType);  //mid + 1 --> size - 1  (수가 클수록 꼬리에 가까움)
                        break;
                    case Direction.BACK:
                        for (int i_grid = pos.Length / 2 - 1; i_grid >= 0; i_grid--) pos[i_grid] = pivotPos + new CubePos(0, pos.Length / 2 - i_grid, 0, pivotPos.sideType);  //mid - 1 --> 0  (수가 작을수록 머리에 가까움)
                        for (int i_grid = pos.Length / 2 + 1; i_grid < pos.Length; i_grid++) pos[i_grid] = pivotPos + new CubePos(0, pos.Length / 2 - i_grid, 0, pivotPos.sideType);  //mid + 1 --> size - 1  (수가 클수록 꼬리에 가까움)
                        break;
                    case Direction.LEFT:
                        for (int i_grid = pos.Length / 2 - 1; i_grid >= 0; i_grid--) pos[i_grid] = pivotPos + new CubePos(pos.Length / 2 - i_grid, 0, 0, pivotPos.sideType);  //mid - 1 --> 0  (수가 작을수록 머리에 가까움)
                        for (int i_grid = pos.Length / 2 + 1; i_grid < pos.Length; i_grid++) pos[i_grid] = pivotPos + new CubePos(pos.Length / 2 - i_grid, 0, 0, pivotPos.sideType);  //mid + 1 --> size - 1  (수가 클수록 꼬리에 가까움)
                        break;
                    case Direction.RIGHT:
                        for (int i_grid = pos.Length / 2 - 1; i_grid >= 0; i_grid--) pos[i_grid] = pivotPos + new CubePos(pos.Length / 2 - i_grid, 0, 0, pivotPos.sideType);  //mid - 1 --> 0  (수가 작을수록 머리에 가까움)
                        for (int i_grid = pos.Length / 2 + 1; i_grid < pos.Length; i_grid++) pos[i_grid] = pivotPos + new CubePos(pos.Length / 2 - i_grid, 0, 0, pivotPos.sideType);  //mid + 1 --> size - 1  (수가 클수록 꼬리에 가까움)
                        break;
                }
                break;
            case ShipType.CRUISER:      //좌상 - 우상 - 좌하 - 우하 (좌하가 피봇)

                pivotPos = GetLimitedPos(_cubePos);
                pos[2] = pivotPos;

                switch (direction)
                {
                    case Direction.FORWARD:
                    case Direction.BACK:
                    case Direction.LEFT:
                    case Direction.RIGHT:
                        pos[0] = pivotPos + new CubePos(0, 1, 0, pivotPos.sideType);
                        pos[1] = pivotPos + new CubePos(1, 1, 0, pivotPos.sideType);
                        pos[2] = pivotPos;
                        pos[3] = pivotPos + new CubePos(1, 0, 0, pivotPos.sideType);
                        break;
                }
                break;
        }
    }

    public void DetachShip()
    {
        attackPoses.Clear();

        for(int i = 0; i < pos.Length; i++)
        {
            NMHGameMng.GetInstance().GetCubeByCubePos(pos[i]).SetShip(pos[i].sideType, null);
        }
    }

    public void SetRotationByDirection()        //방향을 통해 로테이션 수정
    {
        switch(direction)
        {
            case Direction.FORWARD:
                switch (pivotPos.sideType)
                {
                    case NMHGrid.SideType.UP_TO_DOWN:
                        if(pivotPos.depth == 0) transform.localEulerAngles = new Vector3(0, 0, 0);
                        if(pivotPos.depth == 4) transform.localEulerAngles = new Vector3(0, 180, 180);
                        break;
                    case NMHGrid.SideType.LEFT_TO_RIGHT:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(270, 90, 0);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(270, 270, 0);
                        break;
                    case NMHGrid.SideType.FRONT_TO_BEHIND:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(270, 0, 0);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(270, 180, 0);
                        break;
                }
                break;
            case Direction.BACK:
                switch (pivotPos.sideType)
                {
                    case NMHGrid.SideType.UP_TO_DOWN:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(0, 180, 0);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(0, 0, 180);
                        break;
                    case NMHGrid.SideType.LEFT_TO_RIGHT:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(90, 0, 90);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(90, 0, 270);
                        break;
                    case NMHGrid.SideType.FRONT_TO_BEHIND:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(90, 180, 0);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(90, 180, 180);
                        break;
                }
                break;
            case Direction.LEFT:
                switch (pivotPos.sideType)
                {
                    case NMHGrid.SideType.UP_TO_DOWN:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(0, 270, 0);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(0, 270, 180);
                        break;
                    case NMHGrid.SideType.LEFT_TO_RIGHT:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(0, 0, 90);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(180, 0, 270);
                        break;
                    case NMHGrid.SideType.FRONT_TO_BEHIND:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(0, 270, 90);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(180, 270, 270);
                        break;
                }
                break;
            case Direction.RIGHT:
                switch (pivotPos.sideType)
                {
                    case NMHGrid.SideType.UP_TO_DOWN:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(0, 90, 0);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(0, 90, 180);
                        break;
                    case NMHGrid.SideType.LEFT_TO_RIGHT:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(180, 0, 90);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(0, 0, 270);
                        break;
                    case NMHGrid.SideType.FRONT_TO_BEHIND:
                        if (pivotPos.depth == 0) transform.localEulerAngles = new Vector3(0, 90, 270);
                        if (pivotPos.depth == 4) transform.localEulerAngles = new Vector3(0, 270, 270);
                        break;
                }
                break;
        }
    }
    
    public bool CheckCanPlace()
    {
        bool canPlace = true;

        for(int i = 0; i < pos.Length; i++)
        {
            NMHShip[] ships = NMHGameMng.GetInstance().GetCubeByCubePos(pos[i]).GetShip();

            if (ships[(int)pos[i].sideType] != null &&
                ships[(int)pos[i].sideType] != this)
            {
                canPlace = false;

                break;
            }
        }

        return canPlace;
    }

    public void SetTransformByGrid(CubePos _cubePos)       //그리드를 넘겨 거기에 위치시키도록 함
    {
        SetGridByDirection(_cubePos);
        SetRotationByDirection();

        if(NMHGameMng.GetInstance().GetGridByCubePos(GetLimitedPos(_cubePos)) != null)
            transform.localPosition = NMHGameMng.GetInstance().GetGridByCubePos(GetLimitedPos(_cubePos)).transform.localPosition;

        float x = transform.localPosition.x;
        float y = transform.localPosition.y;
        float z = transform.localPosition.z;

        switch(shipType)
        {
            case ShipType.FAST_SHIP:
            case ShipType.DESTROYER:
            case ShipType.SUBMARINE:
                switch (pivotPos.sideType)
                {
                    case NMHGrid.SideType.UP_TO_DOWN:
                        if (pivotPos.depth == 0) transform.localPosition = new Vector3(x, 6.4f + padding, z);
                        if (pivotPos.depth == 4) transform.localPosition = new Vector3(x, -6.4f - padding, z);
                        break;
                    case NMHGrid.SideType.LEFT_TO_RIGHT:
                        if (pivotPos.depth == 0) transform.localPosition = new Vector3(-6.4f - padding, y, z);
                        if (pivotPos.depth == 4) transform.localPosition = new Vector3(6.4f + padding, y, z);
                        break;
                    case NMHGrid.SideType.FRONT_TO_BEHIND:
                        if (pivotPos.depth == 0) transform.localPosition = new Vector3(x, y, -6.4f - padding);
                        if (pivotPos.depth == 4) transform.localPosition = new Vector3(x, y, 6.4f + padding);
                        break;
                }
                break;
            case ShipType.PATROL_SHIP:
            case ShipType.BATTLE_SHIP:
                if (direction == Direction.FORWARD || direction == Direction.BACK)
                {
                    switch (pivotPos.sideType)
                    {
                        case NMHGrid.SideType.UP_TO_DOWN:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x, 6.4f + padding, z + NMHGrid.halfof);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x, -6.4f - padding, z + NMHGrid.halfof);
                            break;
                        case NMHGrid.SideType.LEFT_TO_RIGHT:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(-6.4f - padding, y + NMHGrid.halfof, z);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(6.4f + padding, y + NMHGrid.halfof, z);
                            break;
                        case NMHGrid.SideType.FRONT_TO_BEHIND:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x, y + NMHGrid.halfof, -6.4f - padding);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x, y + NMHGrid.halfof, 6.4f + padding);
                            break;
                    }
                }

                if (direction == Direction.LEFT || direction == Direction.RIGHT)
                {
                    switch (pivotPos.sideType)
                    {
                        case NMHGrid.SideType.UP_TO_DOWN:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x + NMHGrid.halfof, 6.4f + padding + padding, z);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x + NMHGrid.halfof, -6.4f - padding, z);
                            break;
                        case NMHGrid.SideType.LEFT_TO_RIGHT:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(-6.4f - padding, y, z - NMHGrid.halfof);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(6.4f + padding, y, z - NMHGrid.halfof);
                            break;
                        case NMHGrid.SideType.FRONT_TO_BEHIND:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x + NMHGrid.halfof, y, -6.4f - padding);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x + NMHGrid.halfof, y, 6.4f + padding);
                            break;
                    }
                }
                break;
            case ShipType.CRUISER:
                if (direction == Direction.FORWARD || direction == Direction.BACK)
                {
                    switch (pivotPos.sideType)
                    {
                        case NMHGrid.SideType.UP_TO_DOWN:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x + NMHGrid.halfof, 6.4f + padding, z + NMHGrid.halfof);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x + NMHGrid.halfof, -6.4f - padding, z + NMHGrid.halfof);
                            break;
                        case NMHGrid.SideType.LEFT_TO_RIGHT:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(-6.4f - padding, y + NMHGrid.halfof, z - NMHGrid.halfof);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(6.4f + padding, y + NMHGrid.halfof, z - NMHGrid.halfof);
                            break;
                        case NMHGrid.SideType.FRONT_TO_BEHIND:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x + NMHGrid.halfof, y + NMHGrid.halfof, -6.4f - padding);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x + NMHGrid.halfof, y + NMHGrid.halfof, 6.4f + padding);
                            break;
                    }
                }

                if (direction == Direction.LEFT || direction == Direction.RIGHT)
                {
                    switch (pivotPos.sideType)
                    {
                        case NMHGrid.SideType.UP_TO_DOWN:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x + NMHGrid.halfof, 6.4f + padding, z + NMHGrid.halfof);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x + NMHGrid.halfof, -6.4f - padding, z + NMHGrid.halfof);
                            break;
                        case NMHGrid.SideType.LEFT_TO_RIGHT:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(-6.4f - padding, y + NMHGrid.halfof, z - NMHGrid.halfof);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(6.4f + padding, y + NMHGrid.halfof, z - NMHGrid.halfof);
                            break;
                        case NMHGrid.SideType.FRONT_TO_BEHIND:
                            if (pivotPos.depth == 0) transform.localPosition = new Vector3(x + NMHGrid.halfof, y + NMHGrid.halfof, -6.4f - padding);
                            if (pivotPos.depth == 4) transform.localPosition = new Vector3(x + NMHGrid.halfof, y + NMHGrid.halfof, 6.4f + padding);
                            break;
                    }
                }
                break;
        }

        ColorCubeByCubePos();
    }

    public void Rotate()
    {
        switch (direction)
        {
            case Direction.FORWARD:
                direction = Direction.RIGHT;
                break;
            case Direction.BACK:
                direction = Direction.LEFT;
                break;
            case Direction.LEFT:
                direction = Direction.FORWARD;
                break;
            case Direction.RIGHT:
                direction = Direction.BACK;
                break;
        }

        SetTransformByGrid(pivotPos);
    }

    public void Place()
    {
        for (int i = 0; i < pos.Length; i ++)
        {
            NMHGameMng.GetInstance().GetCubeByCubePos(pos[i]).SetShip(pos[i].sideType, this);

            SetAttackPoses();
        }
    }

    public void DamageByPos(CubePos _pos)
    {
        for(int i = 0; i < pos.Length; i++)
        {
            if(pos[i] == _pos)
            {
                isAlive[i] = false;

                if(CheckAlive() == true)
                {

                }
                else
                {
                    Die();
                }

                return;
            }
        }
    }

    bool CheckAlive()
    {
        for(int i = 0; i < isAlive.Length; i++)
        {
            if (isAlive[i] == true)
                return true;
        }

        return false;
    }

    void Die()
    {
        Destroy(gameObject);
    }




    CubePos GetLimitedPos(CubePos _pos)
    {
        CubePos limitedPos;
        limitedPos = _pos;

        if (shipType != ShipType.CRUISER)
        {
            switch (direction)
            {
                case Direction.FORWARD:
                case Direction.BACK:
                    limitedPos.y = Mathf.Max((pos.Length - 1) / 2, Mathf.Min(limitedPos.y, NMHMap.height - 1 - pos.Length / 2));
                    break;
                case Direction.LEFT:
                case Direction.RIGHT:
                    limitedPos.x = Mathf.Max((pos.Length - 1) / 2, Mathf.Min(limitedPos.x, NMHMap.height - 1 - pos.Length / 2));
                    break;
            }
        }
        else
        {
            limitedPos.x = Mathf.Max(0, Mathf.Min(limitedPos.x, NMHMap.height - 2));
            limitedPos.y = Mathf.Max(0, Mathf.Min(limitedPos.y, NMHMap.height - 2));
        }

        limitedPos.depth = _pos.depth;
        limitedPos.sideType = _pos.sideType;

        return limitedPos;
    }

    void ColorCubeByCubePos()
    {
        for (int i_pos = 0; i_pos < pos.Length; i_pos++)
        {
            if (CheckCanPlace() == true)     //색칠하기
                NMHGameMng.GetInstance().GetCubeByCubePos(pos[i_pos]).SetColor(NMHCube.ColorType.GREEN);
            else
                NMHGameMng.GetInstance().GetCubeByCubePos(pos[i_pos]).SetColor(NMHCube.ColorType.RED);
        }

        NMHCube[] cubes = NMHGameMng.GetInstance().map.cube;

        for (int i_cube = 0; i_cube < cubes.Length; i_cube++)
        {
            bool isNormal = true;

            for (int j_pos = 0; j_pos < pos.Length; j_pos++)
            {
                if (cubes[i_cube] == NMHGameMng.GetInstance().GetCubeByCubePos(pos[j_pos]))
                {
                    isNormal = false;
                }
            }

            if (isNormal == true)
                cubes[i_cube].SetColor(NMHCube.ColorType.NORMAL);
        }
    }

    public CubePos[] GetPoses()
    {
        return pos;
    }

    public List<CubePos> GetAttackPoses()
    {
        return attackPoses;
    }
}
