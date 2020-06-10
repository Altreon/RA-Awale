using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Seed : MonoBehaviour
{
    [SerializeField]
    float gravity = 9.81f;
    [SerializeField]
    float timeToSleep = 2f;

    Rigidbody rb;
    Renderer rend;
    bool isAwake = true;
    float timeAwake;
    

    void Awake () {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();

        timeAwake = Time.time;
    }

    void Update () {
        if(!isAwake) rb.Sleep();
    }

    void FixedUpdate()
    {
        if(!isAwake) return;

        Vector3 dir = -transform.parent.up;
        rb.velocity += gravity * dir * Time.fixedDeltaTime;

        if(Time.time - timeAwake >= timeToSleep){
            rb.isKinematic = true;
            isAwake = false;
        }
    }

    public void SetColor(Color color) {
        rend.material.SetColor("_Color", color);
        rend.material.SetColor("_EmissionColor", color);
    }

    public void UnSleep(){
        isAwake = true;
        rb.isKinematic = false;
        timeAwake = Time.time;
    }

    public Rigidbody Rigidbody {
        get{
            return rb;
        }
    }
}
