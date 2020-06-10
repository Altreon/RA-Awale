using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Plateau : MonoBehaviour
{
    static Plateau instance;

    public enum pState {
        idle,
        register,
        turnChose,
        turnPlay,
        turnEarn,
        gameOver,
    };

    PlateauState state = null;
    
    Dictionary<pState, PlateauState> stateCollection;

    [SerializeField]
    Transform grenierTemplate = null;
    public Transform seedTemplate = null;
    [SerializeField]
    int nbGrenier = 12;
    [SerializeField]
    int nbSeedsInit = 4;

    public delegate void PStatusDelegate(pState state);
    public PStatusDelegate StateChangeDelegate;
    public delegate void IntegerDelegate(int value);
    public IntegerDelegate ScoreUpdatedDelegate;

    List<Grenier> greniersRegistered;
    Grenier[] greniers;

    int scoreJ1 = 0;
    int scoreJ2 = 0;

    public Pointer pointer;

    public int SelectedGrenierId { get; set; }
    public int ChosenGrenierId { get; set; }

    // Joueur 1 : player_number = 0
    // Joueur 2 : player_number = 1
    private int player_number = 1;
    public int PlayerNumber
    {
        get
        {
            return player_number;
        }
        set
        {
            if(player_number != value){
                SoundBox.Instance.PlayPlayerSwitch();
            }
            player_number = value;
        }
    }

    public static Plateau Instance {
        get {
            return instance;
        }
    }

    void Awake () {
         if(instance != null){
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start () {
        stateCollection = new Dictionary<pState, PlateauState>();

        foreach(PlateauState ps in GetComponents<PlateauState>()){
            if(stateCollection.ContainsKey(ps.StateType)) continue;
            stateCollection.Add(ps.StateType, ps);
            ps.Owner = this;
        }

        greniersRegistered = new List<Grenier>();

        MarkerManager.Instance.GrenierTargetFondDelegate += GrenierFound;
        MarkerManager.Instance.GrenierTargetLostDelegate += GrenierLost;

        State = pState.register;
    }

    void OnDestroy () {
        MarkerManager.Instance.GrenierTargetFondDelegate -= GrenierFound;
        MarkerManager.Instance.GrenierTargetLostDelegate -= GrenierLost;
    }

    void GrenierFound (int id, Transform marker) {
        if(State == pState.register){
            Grenier grenier = null;
            foreach(Grenier g in greniersRegistered){
                if(g.Id == id){
                    grenier = g;
                    break;
                }
            }

            if(!grenier){
                Transform newGrenier = Instantiate(grenierTemplate, marker.position + grenierTemplate.position, marker.rotation);
                grenier = newGrenier.GetComponent<Grenier>();
                grenier.Id = id;

                greniersRegistered.Add(grenier);

                foreach(Grenier gX in greniersRegistered){
                    foreach(Grenier gY in greniersRegistered){
                        if(gX.Id + 1 % 12 == gY.Id){
                            gX.SetArrowTarget(gY.gameObject);
                        }
                    }
                }
            }else{
                grenier.enabled = true;
            }


            grenier.marker = marker;
        }else{
            Grenier grenier = GetGrenier(id);

            if(!grenier) return;

            grenier.enabled = true;
            grenier.marker = marker;
        }
    }

    void GrenierLost (int id) {
        Grenier grenier = null;

        if(State == pState.register){
            foreach(Grenier g in greniersRegistered){
                if(g.Id == id){
                    grenier = g;
                    break;
                }
            }
        }else{
            grenier = GetGrenier(id);
        }

        if(!grenier) return;

        grenier.enabled = false;
        grenier.transform.position = new Vector3(10000, 10000, 10000);
    }

    public void orderGreniers () {
        greniers = new Grenier[NbGrenier];

        foreach(Grenier grenier in greniersRegistered){
            greniers[grenier.Id] = grenier;
        }
    }

    public bool CheckGameOver () {
        PlayerNumber = (PlayerNumber + 1) % 2;
        foreach (Grenier grenier in Greniers)
        {
            if (PlayableChoice(grenier)) {
                PlayerNumber = (PlayerNumber + 1) % 2;
                return false;
            }
        }
        
        PlayerNumber = (PlayerNumber + 1) % 2;
        return true;
    }

    public bool PlayableChoice(Grenier grenier)
    {
        if(grenier.Storage.SeedNumber == 0 || PlayerNumber != grenier.player_number) return false;

        int target = 0;
        if (grenier.Storage.SeedNumber < 12)
        {
            target = (grenier.Id + grenier.Storage.SeedNumber) % 12;
        }
        else
        {
            if (grenier.Storage.SeedNumber % 11 == 0)
            {
                target = (grenier.Id - 1) % 12;
            }
            else
            {
                target = (grenier.Id + (grenier.Storage.SeedNumber % 11)) % 12;
            }
        }
        if ((target == 5 && PlayerNumber == 1) || (target == 11 && PlayerNumber == 0))
        {
            for (int i=target; i>=target-5; i--)
            {
                if (Greniers[i].Storage.SeedNumber < 1 || Greniers[i].Storage.SeedNumber > 2)
                {
                    return true;
                }
            }
            return false;
        }
        return true;
    }

    public pState State {
        set {
            if(state) state.OnExitState();

            state = stateCollection[value];

            state.OnEnterState();

            StateChangeDelegate(value);
        }
        get{
            return state.StateType;
        }
    }

    public Grenier[] Greniers {
        get {
            return greniers;
        }
    }

    public int NbGrenier {
        get {
            return nbGrenier;
        }
    }

    public int NbGrenierRegistered {
        get {
            return greniersRegistered.Count;
        }
    }

    public Grenier GetGrenier(int id){
        if(id < 0 || id >= nbGrenier) return null;

        return greniers[id];
    }

    public int NbSeeds {
        get {
            return nbSeedsInit;
        }
    }

    public int ScoreJ1 {
        set {
            scoreJ1 = value;

            ScoreUpdatedDelegate(0);
        }
        get{
            return scoreJ1;
        }
    }

    public int ScoreJ2 {
        set {
            scoreJ2 = value;

            ScoreUpdatedDelegate(1);
        }
        get{
            return scoreJ2;
        }
    }
}
