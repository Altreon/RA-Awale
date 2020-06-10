using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    static Pointer instance;

    public bool IsPointing { get; set; } = false;

    public delegate void PointerDelegate(bool is_pointing, Collider collider_pointed);
    public PointerDelegate PointerStateDelegate;

    List<Collider> colliderIn;

    public static Pointer Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        colliderIn = new List<Collider>();

        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        colliderIn.Add(other);
        PointerStateDelegate(true, other);
    }

    private void OnTriggerExit(Collider other)
    {
        colliderIn.Remove(other);

        PointerStateDelegate(false, other);
        
        if(colliderIn.Count != 0){
           PointerStateDelegate(true, colliderIn[0]);
        }
    }

}
