using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnPlateauState : MonoBehaviour
{
    [SerializeField]
    Plateau.pState[] plateauActiveStates = null;

    bool subscribed = false;

    void Awake()
    {
        if(Plateau.Instance){
            Plateau.Instance.StateChangeDelegate += CheckStateChanged;
            subscribed = true;
        }
    }

    void Start () {
        if(!subscribed){
            Plateau.Instance.StateChangeDelegate += CheckStateChanged;
            subscribed = true;
        }
    }

    void OnDestroy () {
        if(subscribed){
            Plateau.Instance.StateChangeDelegate -= CheckStateChanged;
            subscribed = false;
        }
    }

    void CheckStateChanged (Plateau.pState state)
    {
        foreach(Plateau.pState activeState in plateauActiveStates){
            if(activeState == state){
                gameObject.SetActive(true);
                return;
            }
        }

        gameObject.SetActive(false);
    }
}
