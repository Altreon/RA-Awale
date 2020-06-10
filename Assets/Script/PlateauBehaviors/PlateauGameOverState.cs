using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauGameOverState : PlateauState
{
    public override void OnEnterState()
    {
        if(owner.PlayerNumber == 0){
            foreach (Grenier grenier in owner.Greniers){
                Seed[] seeds = grenier.Storage.RemoveAllSeed();
                if(seeds == null) continue;
                for(int i = 0; i < seeds.Length; ++i){
                    ++owner.ScoreJ1;
                    Destroy(seeds[i].gameObject);
                }
            }   
        }else{
            foreach (Grenier grenier in owner.Greniers){
                Seed[] seeds = grenier.Storage.RemoveAllSeed();
                if(seeds == null) continue;
                for(int i = 0; i < seeds.Length; ++i){
                    ++owner.ScoreJ2;
                    Destroy(seeds[i].gameObject);
                }
            }   
        }

        base.OnEnterState();
    }
}