using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMHGameMng : NMHSingleton<NMHGameMng>
{
    public enum GameState
    {
        PLACE,
        SELECT_SHIP_TO_ATTACK,
        SELECT_CUBE_TO_ATTACK,
        ATTACK, 
        SELECT_CUBE_TO_ROTATE_MAP,
        ROTATE_MAP,
    }

    Ray rayTouch;
    Ray rayFacing;

    RaycastHit rhit;

    Vector2 prevMousePos;
    Vector2 curMousePos;

    NMHShip selectedShip;
    NMHShip[] ships;

    GameState gameState;
    PlayerType curPlayer;
    
    CubePos attackPos;

    NMHCube[] selToRot;
    NMHGrid.SideType[] selSideToRot;

    bool isMovingShip;
    bool canStopMoving;



    public NMHMap map;

    public NMHInGameUICtrl inGameUICtrl;



	void Start ()
    {
        gameState = GameState.PLACE;
        curPlayer = PlayerType.PLAYER_1;

        canStopMoving = false;
        isMovingShip = false;

        ships = map.GetComponentsInChildren<NMHShip>();

        selToRot = new NMHCube[2];
        selSideToRot = new NMHGrid.SideType[2];
    }
	
	void Update ()
    {
        ShootRay();

        GetCurFacingGrid();
        GetCurTouchingGrid();

        LookAroundMap();
        SelectShip();

        MoveShip();
        StopMovingShip();
        MoveShipAgain();
        RotateShip();
        PlaceShip();

        SelectCubeToAttack();

        SelectCubeToSelectRotatingSide();

        RotateMap();

        if(Input.GetKeyDown(KeyCode.F1))
        {
            ChangeTurn();
        }
    }



    void ShootRay()
    {
        rayTouch = Camera.main.ScreenPointToRay(Input.mousePosition);

        rayFacing = Camera.main.ScreenPointToRay(new Vector3(960, 540));
    }

    bool RayCast(Ray _ray)
    {
        if(Physics.Raycast(_ray.origin, _ray.direction, out rhit))
        {
            return true;
        }

        return false;
    }

    void LookAroundMap()
    {
        if (Input.GetMouseButton(0) &&
            isMovingShip == false &&
            map.CheckCubeIsRotate(GetCurTouchigCube()) == false)
        {
            map.transform.Rotate((Input.GetAxis("Mouse Y") * 250 * Time.deltaTime), (Input.GetAxis("Mouse X") * 250 * -Time.deltaTime), 0, Space.World);
        }
    }


    void SelectShip()
    {
        if (Input.GetMouseButtonUp(0) &&
            RayCast(rayTouch) == true &&
            rhit.transform.tag == "Ship")// &&
            // curPlayer == rhit.transform.GetComponent<NMHShip>().playertype)
        {
            if (selectedShip == null)
            {
                selectedShip = rhit.transform.GetComponent<NMHShip>();
            }

            switch (gameState)
            {
                case GameState.PLACE:
 
                    selectedShip.DetachShip();

                    canStopMoving = false;
                    isMovingShip = true;
                    break;
                case GameState.SELECT_SHIP_TO_ATTACK:
                    inGameUICtrl.SetUIvisible(NMHInGameUICtrl.UIType.SELECT_SHIP_TO_ATTACK, true);
                    break;
                case GameState.ATTACK:
                    break;
            }
        }
    }

    void ReleaseShip()
    {
        selectedShip = null;
    }

    void MoveShip()
    {
        if (Input.GetMouseButton(0) &&
            selectedShip != null &&
            gameState == GameState.PLACE &&
            isMovingShip == true &&
            GetCurTouchingGrid() != null)
        {

            selectedShip.transform.parent = GetCurTouchingGrid().cube.transform;
            selectedShip.SetTransformByGrid(GetCurTouchingGrid().cube.GetCubePos(GetCurTouchingGrid().sideType));
        }
    }

    void StopMovingShip()
    {
        if (Input.GetMouseButtonUp(0) &&
            selectedShip != null &&
            canStopMoving == true &&
            isMovingShip == true &&
            gameState == GameState.PLACE)
        {
            isMovingShip = false;
        }
        else if(canStopMoving == false)
        {
            canStopMoving = true;
        }
    }

    void MoveShipAgain()
    {
        if (Input.GetMouseButton(0) &&
            selectedShip != null &&
            gameState == GameState.PLACE &&
            GetCurTouchingGrid() != null)
        {
            isMovingShip = true;
        }
    }

    public void RotateShip()
    {
        if (selectedShip != null &&
            Input.GetMouseButtonDown(1))
        {
            selectedShip.Rotate();
        }
    }

    public void PlaceShip()
    {
        if (gameState == GameState.PLACE)
        {
            if (selectedShip != null &&
            Input.GetMouseButtonDown(0) &&
            RayCast(rayTouch) == false &&
            selectedShip.CheckCanPlace() == true &&
            isMovingShip == false)
            {
                map.SetAllCubeColorToNormal();

                selectedShip.Place();
                selectedShip = null;
            }
        }
    }
    
    public void ShowAttackRange()
    {
        if (gameState == GameState.SELECT_CUBE_TO_ATTACK)
        {
            List<CubePos> attackPoses = selectedShip.GetAttackPoses();

            for (int i = 0; i < attackPoses.Count; i++)
                GetCubeByCubePos(attackPoses[i]).SetColor(NMHCube.ColorType.GREEN);
        }
    }

    public void SelectCubeToAttack()
    {
        if (gameState == GameState.SELECT_CUBE_TO_ATTACK)
        {
            if (Input.GetMouseButtonUp(0) &&
                GetCurTouchingCubePos().depth != -1 &&
                GetCubeByCubePos(GetCurTouchingCubePos()) != null)
            {

                attackPos = GetCurTouchingCubePos();

                bool isOK = false;

                for(int i_pos = 0; i_pos < selectedShip.GetAttackPoses().Count; i_pos++)
                {
                    if (selectedShip.GetAttackPoses()[i_pos] == attackPos)
                        isOK = true;
                }

                if (isOK)
                {
                    ShowAttackRange();

                    GetCubeByCubePos(attackPos).SetColor(NMHCube.ColorType.BLUE);

                    inGameUICtrl.SetUIvisible(NMHInGameUICtrl.UIType.SELECT_CUBE_TO_ATTACK, true);
                }
            }
        }
    }

    public void Attack()
    {
        NMHShip targetShip = GetCubeByCubePos(attackPos).GetShip()[(int)attackPos.sideType];

        if (targetShip != null &&
            targetShip.playertype != curPlayer)
        {
            targetShip.DamageByPos(attackPos);
        }
        else
        {
            //TODO : 빗맞았을때..
        }

        ReleaseShip();

        map.SetAllCubeColorToNormal();

        SetGameState(GameState.SELECT_CUBE_TO_ROTATE_MAP);
    }

    void SelectCubeToSelectRotatingSide()
    {
        if (gameState == GameState.SELECT_CUBE_TO_ROTATE_MAP)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (selToRot[0] != null && selToRot[1] != null)
                    selToRot[0] = null; selToRot[1] = null;

                if (selToRot[0] == null &&
                    GetCurTouchigCube() != null)
                {
                    selToRot[0] = GetCurTouchigCube();
                    selToRot[0].SetColor(NMHCube.ColorType.GREEN);
                    selSideToRot[0] = GetCurTouchingCubePos().sideType;
                }
                else if (selToRot[1] == null &&
                        GetCurTouchigCube() != null)
                {
                    selToRot[1] = GetCurTouchigCube();
                    selToRot[1].SetColor(NMHCube.ColorType.GREEN);
                    selSideToRot[1] = GetCurTouchingCubePos().sideType;
                }

                if (selToRot[0] != null && selToRot[1] != null)
                {
                    if (map.SelectRotatingChild(selToRot[0], selToRot[1], selSideToRot[0], selSideToRot[1]))
                        SetGameState(GameState.ROTATE_MAP);
                    else
                    {
                        map.SetAllCubeColorToNormal();
                        selToRot[0] = null; selToRot[1] = null;
                    }
                }
            }
        }
    }

    public void RotateMap()
    {
        if (gameState == GameState.ROTATE_MAP)
        {
            if (Input.GetMouseButton(0) &&
                map.CheckCubeIsRotate(GetCurTouchigCube()) == true)
            {
                map.Rotate();
            }
            else if(Input.GetMouseButtonUp(0))
            {
                map.FinishRotate();
            }
        }

    }

    public void ChangeTurn()
    {
        map.SetAllCubeColorToNormal();

        SetGameState(GameState.SELECT_SHIP_TO_ATTACK);

        if (curPlayer == PlayerType.PLAYER_1)
            curPlayer = PlayerType.PLAYER_2;
        else
            curPlayer = PlayerType.PLAYER_1;
    }

    public void SetGameState(GameState _gameState)
    {
        gameState = _gameState;

        switch(gameState)
        {
            case GameState.SELECT_CUBE_TO_ATTACK:
                inGameUICtrl.SetUIvisible(NMHInGameUICtrl.UIType.BACK, true);
                break;
            case GameState.SELECT_CUBE_TO_ROTATE_MAP:
                NMHGameMng.GetInstance().map.SetAllCubeColorToNormal();
                inGameUICtrl.SetUIvisible(NMHInGameUICtrl.UIType.BACK, false);
                break;
            case GameState.ROTATE_MAP:
                inGameUICtrl.SetUIvisible(NMHInGameUICtrl.UIType.BACK, true);
                break;
            default:
                inGameUICtrl.SetUIvisible(NMHInGameUICtrl.UIType.BACK, false);
                break;
        }
    }

    public bool GetIsMovingShip()
    {
        if (isMovingShip)
            return true;
        else
            return false;
    }

    public NMHGrid GetCurFacingGrid()
    {
        if (RayCast(rayFacing))
        {
            return rhit.transform.GetComponent<NMHGrid>();
        }

        return null;
    }

    public NMHGrid GetCurTouchingGrid()
    {
        if (RayCast(rayTouch))
            return rhit.transform.GetComponent<NMHGrid>();
        else
            return null;
    }

    public CubePos GetCurTouchingCubePos()
    {
        NMHGrid curTouchingGrid = GetCurTouchingGrid();

        CubePos ret = new CubePos(0, 0, 0, NMHGrid.SideType.FRONT_TO_BEHIND);

        if (curTouchingGrid != null)
            ret = curTouchingGrid.cube.GetCubePos(curTouchingGrid.sideType);
        else
            ret.depth = -1;

        return ret;
    }

    public NMHCube GetCurTouchigCube()
    {
        NMHGrid grid = GetCurTouchingGrid();

        if (grid != null)
        {
            NMHCube ret = grid.cube;

            return ret;
        }

        return null;
    }

    public NMHGrid GetGridByCubePos(CubePos _cubePos)
    {
        NMHGrid[] grids = map.GetComponentsInChildren<NMHGrid>();

        for(int i = 0; i < grids.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grids[i].cube.GetCubePos((NMHGrid.SideType)j) == _cubePos &&
                    grids[i].sideType == _cubePos.sideType)
                {
                    return grids[i];
                }
            }
        }

        return null;
    }


    public NMHCube GetCubeByCubePos(CubePos _cubePos)
    {
        for(int i = 0; i < 125; i ++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (map.cube[i].GetCubePos((NMHGrid.SideType)j) == _cubePos)
                {
                    return map.cube[i];
                }   
            }
        }

        return null;
    }

    public NMHShip GetSelectedShip()
    {
        return selectedShip;
    }

    public NMHShip[] GetShips()
    {
        return ships;
    }

    public GameState GetGameState()
    {
        return gameState;
    }
}
