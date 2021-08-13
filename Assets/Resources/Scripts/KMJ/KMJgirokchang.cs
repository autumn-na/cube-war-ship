using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KMJgirokchang : MonoBehaviour {
    public GameObject Girok;

    public void GirokSetActive()
    {
        Girok.SetActive(!Girok.activeInHierarchy);
    }
}
