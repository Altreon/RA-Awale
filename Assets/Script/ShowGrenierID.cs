using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGrenierID : MonoBehaviour
{
    Text textID;
    void Awake()
    {
        textID = GetComponent<Text>();
    }

    public void Refresh(int id){
        textID.text = id.ToString();
    }
}
