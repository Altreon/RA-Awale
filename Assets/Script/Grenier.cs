using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StoredSeeds))]
[RequireComponent(typeof(OutlineColor))]
public class Grenier : MonoBehaviour
{
    int id;
    public Transform marker = null;
    public int player_number;
    
    [SerializeField]
    Arrow arrow;

    [SerializeField]
    ShowGrenierID showGrenierID = null;

    [SerializeField]
    Color selectableColorChoseJ1 = Color.white;
    [SerializeField]
    Color selectableColorChoseJ2 = Color.white;
    [SerializeField]
    Color selectableColorPlayJ1 = Color.white;
    [SerializeField]
    Color selectableColorPlayJ2 = Color.white;
    [SerializeField]
    Color selectedColor = Color.white;



    [SerializeField]
    float posSpeed = 0;
    [SerializeField]
    float rotSpeed = 0;

    [SerializeField]
    Collider surfaceCollider = null;

    StoredSeeds storage;
    OutlineColor outlineColor;

    void Awake () {
        storage = GetComponent<StoredSeeds>();
        outlineColor = GetComponent<OutlineColor>();
    }

    void FixedUpdate () {
        transform.rotation = Quaternion.Slerp(transform.rotation, marker.rotation, rotSpeed);
        transform.position = marker.position;
    }

    public int Id {
        set {
            if (value >= 0)
            {
                id = value;
                showGrenierID.Refresh(value);
                if (value <= 5) player_number = 0;
                else player_number = 1;
            }
        }
        get {
            return id;
        }
    }

    public StoredSeeds Storage {
        get {
            return storage;
        }
    }

    public Collider SurfaceCollider  {
        get{
            return surfaceCollider;
        }
    }

    public void Selectable () {
        if(Plateau.Instance.State == Plateau.pState.turnChose){
            if(id >= 0 && id <= 5){
                outlineColor.SetColor(selectableColorChoseJ1);
            }else{
                 outlineColor.SetColor(selectableColorChoseJ2);
            }
        }else{
            if(Plateau.Instance.PlayerNumber == 0){
                outlineColor.SetColor(selectableColorPlayJ1);
            }else{
                 outlineColor.SetColor(selectableColorPlayJ2);
            }
        }
    }

    public void Selected () {
        outlineColor.SetColor(selectedColor);
    }

    public void Unselected () {
        outlineColor.Hide();
    }
    
    public void SetArrowTarget(GameObject target){
        arrow.SetTarget(target);
    }

}
