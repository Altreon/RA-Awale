using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredSeeds : MonoBehaviour
{
    [SerializeField]
    Transform dropPosition = null;
    [SerializeField]
    Transform seedsParent = null;
    [SerializeField]
    bool sticky = false;
    [SerializeField]
    float addSpaceTime = 1;
    [SerializeField]
    float maxDistanceWithSeed = 4;
    [SerializeField]
    float timeBetwwenDistCheck = 1;

    Queue<Seed> seedsAdded;
    bool seedsAdding = false;

    int seed_number;

    public int SeedNumber
    {
        get
        {
            return seed_number;
        }
    }

    void Awake () {
        seedsAdded = new Queue<Seed>();
    }

    void Start () {
        if(!sticky) StartCoroutine(CheckSeedNotLost());
    }

    void OnDestroy() {
        sticky = true;
    }
    
    public void AddSeed(Seed seed) {
        seed.transform.position = dropPosition.position;
        seed.transform.parent = seedsParent;

        ++seed_number;

        if(sticky){
            seed.Rigidbody.isKinematic = true;
        }else{
            seed.UnSleep();

            SetCollision(seed.GetComponent<Collider>(), true);

            seed.gameObject.SetActive(false);
            seedsAdded.Enqueue(seed);

            if(!seedsAdding) StartCoroutine(ShowSeedsAdded());
        }
    }

    public void AddSeeds(Seed[] seeds) {
        foreach(Seed seed in seeds){
            AddSeed(seed);
        }
    }

    public Seed RemoveASeed(){
        Seed seed = seedsParent.GetChild(0).GetComponent<Seed>();
        if(!seed) return null;

        --seed_number;

        if(sticky){
            SetCollision(seed.GetComponent<Collider>(), false);
        }

        return seed;
    }

    public Seed[] RemoveAllSeed(){
        Seed[] seeds = new Seed[SeedNumber];
        int i = 0;
        while(seedsAdded.Count != 0){
            seeds[i] = seedsAdded.Dequeue();
            ++i;
        }

        Seed[] seedsChild = seedsParent.GetComponentsInChildren<Seed>();
        for(int j = 0; j < seedsChild.Length; ++j){
            seeds[i+j] = seedsChild[j];
        }

        if(seeds.Length == 0) return null;

        seed_number -= seeds.Length;
        return seeds;
    }

    IEnumerator ShowSeedsAdded () {
        if(seedsAdded.Count == 0){
            seedsAdding = false;
        }else{
            if(!seedsAdding){
                seedsAdding = true;
                seedsAdded.Dequeue().gameObject.SetActive(true);
                yield return new WaitForSeconds(addSpaceTime);
            }else{
                yield return new WaitForSeconds(addSpaceTime);
                if(seedsAdded.Count != 0) seedsAdded.Dequeue().gameObject.SetActive(true);
            }

            yield return ShowSeedsAdded();
        }
    }

    void SetCollision (Collider collider, bool collision) {
        foreach(Collider c in seedsParent.parent.GetComponentsInChildren<Collider>()){
            Physics.IgnoreCollision(collider, c, !collision);
        }

        foreach(Seed s in seedsAdded){
            Collider c = s.GetComponent<Collider>();
            Physics.IgnoreCollision(collider, c, !collision);
        }
    }

    IEnumerator CheckSeedNotLost () {
        while(!sticky){
            if(seed_number != 0){
                Seed[] seedsChild = seedsParent.GetComponentsInChildren<Seed>();
                foreach(Seed seed in seedsChild){
                    if(Vector3.Distance(dropPosition.position, seed.transform.position) >= maxDistanceWithSeed){
                        seed.Rigidbody.velocity = Vector3.zero;
                        seed.transform.position = dropPosition.position;
                    }
                }
            }

            yield return new WaitForSeconds(timeBetwwenDistCheck);

        }
    }
}
