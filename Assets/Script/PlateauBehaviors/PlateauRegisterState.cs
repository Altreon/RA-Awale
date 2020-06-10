using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauRegisterState : PlateauState
{
    void Update() {
        if(owner.NbGrenierRegistered >= owner.NbGrenier){
            owner.State = Plateau.pState.turnChose;
        }
    }
    public override void OnExitState()
    {
        owner.orderGreniers();

        List<Collider> grenierColliders = new List<Collider>();
        List<Collider> seedColliders = new List<Collider>();

        foreach(Grenier grenier in owner.Greniers){
            grenierColliders.Add(grenier.SurfaceCollider);
        }

        foreach(Grenier grenier in owner.Greniers){
            for(int i = 0; i < owner.NbSeeds; ++i){

                Transform newSeed = Instantiate(owner.seedTemplate);
                Seed seed = newSeed.GetComponent<Seed>();
                Collider col = seed.GetComponent<Collider>();

                foreach(Collider seedCollider in seedColliders){
                    Physics.IgnoreCollision(col, seedCollider);
                }

                foreach(Collider grenierCollider in grenierColliders){
                    Physics.IgnoreCollision(col, grenierCollider);
                }

                seed.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

                grenier.Storage.AddSeed(seed);

                seedColliders.Add(col);
            }
        }

        base.OnExitState();
    }
}
