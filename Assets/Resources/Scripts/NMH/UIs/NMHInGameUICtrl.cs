using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMHInGameUICtrl : MonoBehaviour
{
    public enum UIType
    {
        FINISH_SELECTING,
        SELECT_SHIP_TO_ATTACK,
        SELECT_CUBE_TO_ATTACK,
        BACK,
    }

    public GameObject objRotate;
    public GameObject objPlace;
    public GameObject objFinishSelecting;
    public GameObject objSelectShipToAttack;
    public GameObject objSelectCubeToAttack;
    public GameObject objBack; 

    public GameObject[] objShipPref;

    Vector3 vec3RotatePos;
    Vector3 vec3PlacePos;

    const int CHASE_UI_PADDING = 80;



    private void Start()
    {
        NMHSoundMng.GetInstance().RunBGM(NMHSoundMng.BGMList.IN_GAME);
    }

    //아래는 Unity 이벤트 함수=================================================================================================================
    private void Update()
    {
        //ChaseUI();
    }

    //아래는 일반 함수==========================================================================================================================

    public void ChaseUI()
    {
        if (NMHGameMng.GetInstance().GetSelectedShip() != null &&
            NMHGameMng.GetInstance().GetIsMovingShip() == true)
        {
            vec3RotatePos = Camera.main.WorldToScreenPoint(NMHGameMng.GetInstance().GetSelectedShip().transform.position) - new Vector3(960, 540, 0) + new Vector3(-100, -300, 0);
            vec3PlacePos = Camera.main.WorldToScreenPoint(NMHGameMng.GetInstance().GetSelectedShip().transform.position) - new Vector3(960, 540, 0) + new Vector3(100, -300, 0);

            objRotate.GetComponent<RectTransform>().anchoredPosition = new Vector3(Mathf.Max(-(NMHUserClass.SCREEN_WIDTH / 2 - CHASE_UI_PADDING), Mathf.Min(vec3RotatePos.x, NMHUserClass.SCREEN_WIDTH / 2 - CHASE_UI_PADDING)),
                                                                                   Mathf.Max(-(NMHUserClass.SCREEN_HEIGHT / 2 - CHASE_UI_PADDING), Mathf.Min(vec3RotatePos.y, NMHUserClass.SCREEN_HEIGHT / 2 - CHASE_UI_PADDING)),
                                                                                   0);
            objPlace.GetComponent<RectTransform>().anchoredPosition = new Vector3(Mathf.Max(-(NMHUserClass.SCREEN_WIDTH / 2 - CHASE_UI_PADDING), Mathf.Min(vec3PlacePos.x, NMHUserClass.SCREEN_WIDTH - CHASE_UI_PADDING)),
                                                                                   Mathf.Max(-(NMHUserClass.SCREEN_HEIGHT / 2 - CHASE_UI_PADDING), Mathf.Min(vec3PlacePos.y, NMHUserClass.SCREEN_HEIGHT - CHASE_UI_PADDING)),
                                                                                   0);
        }

        if (NMHGameMng.GetInstance().GetSelectedShip() != null &&
            NMHGameMng.GetInstance().GetIsMovingShip() == false)
        {
            objRotate.SetActive(true);
            objPlace.SetActive(true);

            vec3RotatePos = Camera.main.WorldToScreenPoint(NMHGameMng.GetInstance().GetSelectedShip().transform.position) - new Vector3(960, 540, 0) + new Vector3(-100, -300, 0);
            vec3PlacePos = Camera.main.WorldToScreenPoint(NMHGameMng.GetInstance().GetSelectedShip().transform.position) - new Vector3(960, 540, 0) + new Vector3(100, -300, 0);

            objRotate.GetComponent<RectTransform>().anchoredPosition = new Vector3(Mathf.Max(-(NMHUserClass.SCREEN_WIDTH / 2 - CHASE_UI_PADDING), Mathf.Min(vec3RotatePos.x, NMHUserClass.SCREEN_WIDTH - CHASE_UI_PADDING)),
                                                                                    Mathf.Max(-(NMHUserClass.SCREEN_HEIGHT / 2 - CHASE_UI_PADDING), Mathf.Min(vec3RotatePos.y, NMHUserClass.SCREEN_HEIGHT - 100)),
                                                                                    0);
            objPlace.GetComponent<RectTransform>().anchoredPosition = new Vector3(Mathf.Max(-(NMHUserClass.SCREEN_WIDTH / 2 - CHASE_UI_PADDING), Mathf.Min(vec3PlacePos.x, NMHUserClass.SCREEN_WIDTH - CHASE_UI_PADDING)),
                                                                                   Mathf.Max(-(NMHUserClass.SCREEN_HEIGHT / 2 - CHASE_UI_PADDING), Mathf.Min(vec3PlacePos.y, NMHUserClass.SCREEN_HEIGHT - CHASE_UI_PADDING)),
                                                                                   0);
        }
    }



    public void ButRotateShip()
    {
        NMHGameMng.GetInstance().RotateShip();
    }

    public void ButPlaceShip()
    {
        NMHGameMng.GetInstance().PlaceShip();

        objRotate.SetActive(false);
        objPlace.SetActive(false);
    }

    public void BTFinishPlacingShip()
    {
        NMHGameMng.GetInstance().SetGameState(NMHGameMng.GameState.SELECT_SHIP_TO_ATTACK);

        objFinishSelecting.SetActive(false);
    }

    public void BTSelectShipToAttack()
    {
        NMHGameMng.GetInstance().SetGameState(NMHGameMng.GameState.SELECT_CUBE_TO_ATTACK);
        NMHGameMng.GetInstance().ShowAttackRange();

        objSelectShipToAttack.SetActive(false);
    }

    public void BTSelectCubeToAttack()
    {
        NMHGameMng.GetInstance().SetGameState(NMHGameMng.GameState.ATTACK);

        NMHGameMng.GetInstance().Attack();

        objSelectCubeToAttack.SetActive(false);
    }

    public void BTBack()
    {
        switch(NMHGameMng.GetInstance().GetGameState())
        {
            case NMHGameMng.GameState.PLACE:
                break;
            case NMHGameMng.GameState.SELECT_SHIP_TO_ATTACK:
                break;
            case NMHGameMng.GameState.SELECT_CUBE_TO_ATTACK:
                NMHGameMng.GetInstance().map.SetAllCubeColorToNormal();
                NMHGameMng.GetInstance().SetGameState(NMHGameMng.GameState.SELECT_SHIP_TO_ATTACK);
                break;
            case NMHGameMng.GameState.ATTACK:
                break;
            case NMHGameMng.GameState.ROTATE_MAP:
                NMHGameMng.GetInstance().SetGameState(NMHGameMng.GameState.SELECT_CUBE_TO_ROTATE_MAP);
                break;
        }
    }

    public void SetUIvisible(UIType _uiType, bool _isVisible)
    {
        switch (_uiType)
        {
            case UIType.FINISH_SELECTING:
                objFinishSelecting.SetActive(_isVisible);
                break;
            case UIType.SELECT_SHIP_TO_ATTACK:
                objSelectShipToAttack.SetActive(_isVisible);
                break;
            case UIType.SELECT_CUBE_TO_ATTACK:
                objSelectCubeToAttack.SetActive(_isVisible);
                break;
            case UIType.BACK:
                objBack.SetActive(_isVisible);
                break;
        }
    }

    public void createship(string _name)
    {
        switch (_name)
        {
            case "battle_ship":
                break;
            case "cruiser":
                break;
            case "patrol_ship":
                break;
            case "fast_ship":
                break;
            case "destoryer":
                break;
            case "submarine":
                break;
        }
    }
}
