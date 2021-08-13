using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KCMTimer : MonoBehaviour {
    public Text TimeCount;
    public float Timecost;

	void Start () {
		
	}

    void Update()
    {
        Timecost -= Time.deltaTime;
        TimeCount.text = "남은시간:" + string.Format("{0:f0}", Timecost);
        
    }
}
