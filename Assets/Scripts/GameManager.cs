using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {
    
    private const float TIME_DELAY_BETWEEN_BUTTONS_BLINKS = 2f;

    public ButtonScript blueButton;
    public ButtonScript greenButton;
    public ButtonScript redButton;
    public ButtonScript yellowButton;

    private CountManager countManager;
    private UIManager interfaceManager;

    private List<ValueTypes.buttonType> taskList;
    private List<ValueTypes.buttonType> playerList;

    private int taskLoop;
    private bool messageOnScreen;
    private ValueTypes.gameState gameState;

    void Start() {
        countManager = GetComponent<CountManager>();
        interfaceManager = GetComponent<UIManager>();

        taskList = new List<ValueTypes.buttonType>();
        playerList = new List<ValueTypes.buttonType>();

        ResetData();
        PrepareNewRound();
    }

    void LateStart() {
        UpdateScore();
    }

    void Update() {
        DoesGameStart();
    }

    public void PlayerClick(ValueTypes.buttonType button) {
        if(gameState == ValueTypes.gameState.playerTurn) {
            playerList.Add(button);
            CheckResult();
        }

        RestoreBonusScore();
    }

    public void UpdateBestScore(int newBestScore) {
        interfaceManager.DisplayNewBestScore(newBestScore);
    }

    public int GetScore() {
        return countManager.GetScore();
    }

    private void PrepareNewRound() {
        taskList.Clear();
        playerList.Clear();

        countManager.PrepareNewRound();

        gameState = ValueTypes.gameState.start;
    }

    private void DoesGameStart() {
        if (gameState == ValueTypes.gameState.start && Input.anyKeyDown) {
            SwitchGameState(ValueTypes.gameState.task);
            UpdateScore();

            taskLoop = countManager.GetCountTaskButtons();
            StartCoroutine(RandomButtonBlink());
        }
    }

    private void CheckResult() {
        if (playerList[playerList.Count - 1] != taskList[playerList.Count - 1])
            GameOver();
        else {
            IncreaseScore();
            UpdateScore();

            if (taskList.Count == playerList.Count) {
                SwitchGameState(ValueTypes.gameState.win);
                GoToNextLevel();
            }

        }
    }

    private void GameOver() {
        SwitchGameState(ValueTypes.gameState.lose);

        ResetData();
        PrepareNewRound();
    }

    private void GoToNextLevel() {
        PrepareNewRound();
    }

    private IEnumerator RandomButtonBlink() {
        yield return new WaitForEndOfFrame(); 

        switch ((int)(Random.Range(25, 125) / 25)) {
            case 1:
                blueButton.ClickButton();
                taskList.Add(ValueTypes.buttonType.blue);
                break;
            case 2:
                greenButton.ClickButton();
                taskList.Add(ValueTypes.buttonType.green);
                break;
            case 3:
                redButton.ClickButton();
                taskList.Add(ValueTypes.buttonType.red);
                break;
            case 4:
                yellowButton.ClickButton();
                taskList.Add(ValueTypes.buttonType.yellow);
                break;
            default:
                Debug.Log("GameManager/RandomBlinks setting random button error!");
                blueButton.ClickButton();
                taskList.Add(ValueTypes.buttonType.green);
                break;
        }

        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_BUTTONS_BLINKS);

        if (--taskLoop > 0) {
            StartCoroutine(RandomButtonBlink());
        } else {
            SwitchGameState(ValueTypes.gameState.playerTurn);

            RestoreBonusScore();
        }
    }

    private void SwitchGameState(ValueTypes.gameState newGameState) {
        gameState = newGameState;
        interfaceManager.SwitchGameState(newGameState);
    }

    private void ResetData() {
        countManager.ResetData();
    }

    private void IncreaseScore() {
        countManager.IncreaseScore();
    }

    private void RestoreBonusScore() {
        countManager.RestoreBonusScore();
    }

    private void UpdateScore() {
        interfaceManager.UpdateScore();
    }
}
