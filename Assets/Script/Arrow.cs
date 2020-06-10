using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Arrow : MonoBehaviour
{
    LineRenderer rl;
    GameObject target = null;

    void Awake()
    {
        rl = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if(!target || target.transform.position.x > 500){
            if(rl.enabled) rl.enabled = false;
            return;
        }

        if(!rl.enabled) rl.enabled = true;

        Vector3 endPos = target.transform.position;

        rl.SetPosition(0, transform.position);
        rl.SetPosition(1, transform.position + (endPos - transform.position) * 0.85f);
        rl.SetPosition(2, transform.position + (endPos - transform.position) * 0.9f);
        rl.SetPosition(3, endPos);
    }

    public void SetTarget (GameObject target){
        this.target = target;
    }
}
