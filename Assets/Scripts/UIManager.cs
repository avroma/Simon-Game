using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private const int SMALL_DELAY_POPUP_MESSAGE_FRAMES = 90;
    private const int LARGE_DELAY_POPUP_MESSAGE_FRAMES = 170;

    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;

    private GameManager gameManager;
    private ValueTypes.GameState gameState;
    private int messageTimer;
    private bool messageOnScreen;


    public void Start () {
        gameManager = GetComponent<GameManager>();

        SwitchGameState(ValueTypes.GameState.Start);

        RestoreMessageTimer();
        HideMessage();
    }
	

    public void Update () {
        NeedToShowMessage();
    }

    public void UpdateScore() {
        scoreText.text = "Результат: " + gameManager.Score;

    }

    public void DisplayNewBestScore(int newBestScore) {
        bestScoreText.text = "Лучший результат: " + newBestScore + "!";
    }

    public void ShowMessage(ValueTypes.GameState gameState) {
        messageText.transform.localScale = new Vector3(1, 1);
       
         switch(gameState) {
            case ValueTypes.GameState.Start:
                messageText.text = "Нажмите любую кнопку что бы начать игру!";
                break;
            case ValueTypes.GameState.PlayerTurn:
                messageText.text = "Повторите последовательность!";
                break;
            case ValueTypes.GameState.Win:
                messageText.text = "Победа!!! Нажмите любую кнопку что бы начать игру!";
                break;
            case ValueTypes.GameState.Lose:
                messageText.text = "Проигрыш(( Нажмите любую кнопку что бы начать игру!";
                break;
            default:
                messageText.text = "";
                messageText.transform.localScale = new Vector3(0, 0);
                Debug.Log("UIManager/ShowMessage gameStaterecognition error!");
                break;
        }
    }

    public void HideMessage() {
        messageText.transform.localScale = new Vector3(0, 0);
        messageOnScreen = false;
    }

    public void SwitchGameState(ValueTypes.GameState newGameState) {
        gameState = newGameState;
    }

    private void NeedToShowMessage() {
        if (gameState != ValueTypes.GameState.Task) {
            if (!messageOnScreen) {
                messageTimer--;

                if (messageTimer < 0) {
                    messageOnScreen = true;
                    ShowMessage(gameState);
                }
            }

            if (Input.anyKeyDown | Input.GetAxis("Mouse X") != 0 | Input.GetAxis("Mouse Y") != 0) {
                RestoreMessageTimer();
                if (messageOnScreen) {
                    HideMessage();
                }
            }
        } else{
            HideMessage();
        }
    }

    private void RestoreMessageTimer() {
        if (gameState == ValueTypes.GameState.Start | gameState == ValueTypes.GameState.Win | gameState == ValueTypes.GameState.Lose){
            messageTimer = SMALL_DELAY_POPUP_MESSAGE_FRAMES;
        } else{
            messageTimer = LARGE_DELAY_POPUP_MESSAGE_FRAMES;
        }
    }
}
