using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plateau))]
public abstract class PlateauState : MonoBehaviour
{
    [SerializeField]
    Plateau.pState stateType = Plateau.pState.idle;
    protected Plateau owner;

    public Plateau Owner {
        set {
            owner = value;
        }
    }

    void Awake () {
        enabled = false;
    }

    public virtual void OnEnterState() {
        Debug.Log("Plateau enter on state " + stateType);
        enabled = true;
    }

    public virtual void OnExitState() {
        Debug.Log("Plateau exit on state " + stateType);
        enabled = false;
    }

    public Plateau.pState StateType {
        get{
            return stateType;
        }
    }
}
