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

    private List<ValueTypes.ButtonType> taskList;
    private List<ValueTypes.ButtonType> playerList;

    private int taskLoop;
    private bool messageOnScreen;
    private ValueTypes.GameState gameState;

    public void Start() {
        countManager = GetComponent<CountManager>();
        interfaceManager = GetComponent<UIManager>();

        taskList = new List<ValueTypes.ButtonType>();
        playerList = new List<ValueTypes.ButtonType>();

        countManager.ResetData();
        PrepareNewRound();
    }

    public void LateStart() {
        interfaceManager.UpdateScore();
    }

    public void Update() {
        DoesGameStart();
    }

    public void PlayerClick(ValueTypes.ButtonType button) {
        if(gameState == ValueTypes.GameState.PlayerTurn) {
            playerList.Add(button);
            CheckResult();
        }

        countManager.RestoreBonusScore();
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

        gameState = ValueTypes.GameState.Start;
    }

    private void DoesGameStart() {
        if (gameState == ValueTypes.GameState.Start && Input.anyKeyDown) {
            SwitchGameState(ValueTypes.GameState.Task);
            interfaceManager.UpdateScore();

            taskLoop = countManager.GetCountTaskButtons();
            StartCoroutine(RandomButtonBlink());
        }
    }

    private void CheckResult() {
        if (playerList[playerList.Count - 1] != taskList[playerList.Count - 1]){
            GameOver();
        } else {
            countManager.IncreaseScore();
            interfaceManager.UpdateScore();

            if (taskList.Count == playerList.Count) {
                SwitchGameState(ValueTypes.GameState.Win);
                GoToNextLevel();
            }

        }
    }

    private void GameOver() {
        SwitchGameState(ValueTypes.GameState.Lose);

        countManager.ResetData();
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
                taskList.Add(ValueTypes.ButtonType.Blue);
                break;
            case 2:
                greenButton.ClickButton();
                taskList.Add(ValueTypes.ButtonType.Green);
                break;
            case 3:
                redButton.ClickButton();
                taskList.Add(ValueTypes.ButtonType.Red);
                break;
            case 4:
                yellowButton.ClickButton();
                taskList.Add(ValueTypes.ButtonType.Yellow);
                break;
            default:
                Debug.Log("GameManager/RandomBlinks setting random button error!");
                blueButton.ClickButton();
                taskList.Add(ValueTypes.ButtonType.Green);
                break;
        }

        yield return new WaitForSeconds(TIME_DELAY_BETWEEN_BUTTONS_BLINKS);

        if (--taskLoop > 0) {
            StartCoroutine(RandomButtonBlink());
        } else {
            SwitchGameState(ValueTypes.GameState.PlayerTurn);

            countManager.RestoreBonusScore();
        }
    }

    private void SwitchGameState(ValueTypes.GameState newGameState) {
        gameState = newGameState;
        interfaceManager.SwitchGameState(newGameState);
    }
}
