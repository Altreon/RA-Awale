using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    static GuiManager instance;

    [System.Serializable]
    public struct StateToText {
        public Plateau.pState state;
        public string text;
    }
    public StateToText[] stateToText;

    Dictionary<Plateau.pState, string> stateToTextDic;

    [SerializeField]
    Text stateText = null;
    [SerializeField]
    Text scoreJ1Value = null;
    [SerializeField]
    Text scoreJ2Value = null;
    [SerializeField]
    Text gameOverText = null;


    public static GuiManager Instance {
        get {
            return instance != null ? instance : null;
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
        stateToTextDic = new Dictionary<Plateau.pState, string>();

        foreach(StateToText stt in stateToText){
            if(!stateToTextDic.ContainsKey(stt.state)) stateToTextDic.Add(stt.state, stt.text);
        }

        Plateau.Instance.StateChangeDelegate += ChangeStateText;
        Plateau.Instance.ScoreUpdatedDelegate += UpdateScore;
    }

    void ChangeStateText(Plateau.pState state) {
        string s = null;

        if(state == Plateau.pState.gameOver){
            s = gameOverText.text;
            s = Plateau.Instance.ScoreJ1 > Plateau.Instance.ScoreJ2 ? s.Replace ("#", "1") : 
                                                                      s.Replace ("#", "2");
		    gameOverText.text = s;
            return;
        }

        if(!stateToTextDic.ContainsKey(state)) return;

        s = stateToTextDic[state];
		s = s.Replace ("#", (Plateau.Instance.PlayerNumber + 1).ToString());
		stateText.text = s;
    }

    void UpdateScore (int player){
        if(player == 0){
            scoreJ1Value.text = Plateau.Instance.ScoreJ1.ToString();
        }else{
            scoreJ2Value.text = Plateau.Instance.ScoreJ2.ToString();
        }
    }
}
