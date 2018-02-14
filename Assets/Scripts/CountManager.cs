using UnityEngine;

public class CountManager : MonoBehaviour {

    private GameManager gameManager;

    private int round;
    private int countTaskButtons;
    private int score;
    private int bestScore;

    private float bonusScore;


    void Start () {
        gameManager = GetComponent<GameManager>();

        ResetData();
        PrepareNewRound();
    }
	
    void Update () {
        DecreaseBonusScore();
    }

    public void ResetData() {
        round = 0;
        score = 0;

        RestoreBonusScore();
    }

    public void PrepareNewRound() {
        round++;
        countTaskButtons = round + 1;
    }

    public void RestoreBonusScore() {
        bonusScore = 100;
    }

    public int GetCountTaskButtons() {
        return countTaskButtons;
    }

    public void IncreaseScore() {
        score = score++ + (int)bonusScore;

        if(score > bestScore) {
            bestScore = score;
            gameManager.UpdateBestScore(bestScore);
        }
    }

    public int GetScore() {
        return score;
    }

    private void DecreaseBonusScore() {
        if (bonusScore > 0){
            bonusScore = bonusScore - .15f;
        } else{
            bonusScore = 0;
        }
    }
}
