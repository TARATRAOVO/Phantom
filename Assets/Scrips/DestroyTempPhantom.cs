using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTempPhantom : MonoBehaviour
{
    private GameObject[] tempPhantoms;
    // Start is called before the first frame update
    public void DestroyTemperoryPhantom()
    {

        tempPhantoms = GameObject.FindGameObjectsWithTag("TempPhantom");
        foreach (GameObject tempPhantom in tempPhantoms)
        {
            Destroy(tempPhantom);
        }
    }
}
