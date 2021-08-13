using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KMJBuy : MonoBehaviour {
    public GameObject On;
    public GameObject Off;

    public void BuySetActive()
    {
        On.SetActive(true);
        Off.SetActive(false);
    }
}
