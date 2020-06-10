using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;

public class MarkerManager
{
    static MarkerManager instance;

    VuMarkManager markManager;

    public delegate void TargetFondDelegate(int targetId, Transform marker);
    public delegate void TargetLostDelegate(int targetId);
    public TargetFondDelegate GrenierTargetFondDelegate;
    public TargetLostDelegate  GrenierTargetLostDelegate;

    List<VuMarkBehaviour> registerMarkBehaviour;
    struct Marker {
        public int id;

        public Marker(int id){
            this.id = id;
        }
    }

    public static MarkerManager Instance {
        get {
            return instance != null ? instance : null;
        }
    }

    public static void Init() {
        if(instance != null) return;

        instance = new MarkerManager();

        instance.registerMarkBehaviour = new List<VuMarkBehaviour>();

	    instance.markManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        instance.markManager.RegisterVuMarkBehaviourDetectedCallback(instance.MarkFond);
        instance.markManager.RegisterVuMarkLostCallback(instance.TargetLost);
    }

    void MarkFond (VuMarkBehaviour markBehaviour){
        if(registerMarkBehaviour.Contains(markBehaviour)) return;

        registerMarkBehaviour.Add(markBehaviour);

        markBehaviour.RegisterVuMarkTargetAssignedCallback(() => TargetFond(markBehaviour));
    }

    void TargetFond (VuMarkBehaviour markBehaviour){
        int targetId = int.Parse(markBehaviour.VuMarkTarget.InstanceId.ToString());

        GrenierTargetFondDelegate(targetId, markBehaviour.transform.GetChild(0));
    }

    void TargetLost (VuMarkTarget markTarget){
        int targetId = int.Parse(markTarget.InstanceId.ToString());

        GrenierTargetLostDelegate(targetId);
    }

    public int NbMarkerDetected () {
        return registerMarkBehaviour.Count;
    }
}
