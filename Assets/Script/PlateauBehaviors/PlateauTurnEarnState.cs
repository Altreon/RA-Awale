using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauTurnEarnState : PlateauState
{
    [SerializeField]
    float timeBetweenEachEarn = 0;
    [SerializeField]
    float transitionSpeed = 0;
    [SerializeField]
    float distToEarned = 0;

    [SerializeField]
    Transform J1ScorePos = null;
    [SerializeField]
    Transform J2ScorePos = null;

    List<Seed> seedsEarning;
    List<Seed> seedsDestoying;

    void Awake () {
        seedsEarning = new List<Seed>();
        seedsDestoying = new List<Seed>();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        StartCoroutine(Earn());
    }

    public override void OnExitState()
    {
        while(seedsDestoying.Count != 0){
            
            Destroy(seedsDestoying[0].gameObject);
            seedsDestoying.RemoveAt(0);
        }

        base.OnExitState();
    }

    void Update () {
        for(int i = 0; i < seedsEarning.Count; ++i){
            Seed seed = seedsEarning[i];

            Vector3 pos = J1ScorePos.position;
            if(owner.PlayerNumber == 1){
                pos = J2ScorePos.position;
            }

            if(Vector3.Distance(seed.transform.position, pos) <= distToEarned){
                EarnSeed(seed);
                seedsEarning.Remove(seed);
                --i;
            }else{
                seed.transform.position = Vector3.Lerp(seed.transform.position, pos, transitionSpeed * Time.deltaTime);
            }
        }
    }

    IEnumerator Earn () {
        StoredSeeds lastGrenierStorage = owner.GetGrenier(owner.SelectedGrenierId).Storage;

        if(lastGrenierStorage.SeedNumber == 2 ||lastGrenierStorage.SeedNumber == 3){
            Seed[] seeds = lastGrenierStorage.RemoveAllSeed();
            foreach(Seed seed in seeds){
                if(seed.gameObject.activeSelf){
                    seed.enabled = false;
                    seedsEarning.Add(seed);
                }else{
                    EarnSeed(seed);
                }
                
                yield return new WaitForSeconds(timeBetweenEachEarn);
            }

            --owner.SelectedGrenierId;
            if(owner.SelectedGrenierId == -1){
                owner.SelectedGrenierId = owner.NbGrenier-1;
            }
            yield return Earn();
        }else{
            while(seedsEarning.Count != 0){
                yield return null;
            }

            if(owner.CheckGameOver()){
                owner.State = Plateau.pState.gameOver;
            } else {
                owner.State = Plateau.pState.turnChose;
            }
        }                 
    }

    void EarnSeed (Seed seed) {
        seedsDestoying.Add(seed);
        seed.gameObject.SetActive(false);
                
        if(owner.PlayerNumber == 0){
            ++owner.ScoreJ1;
        }else{
            ++owner.ScoreJ2;
        }

        SoundBox.Instance.PlaySeedEarn();
    }
}
