using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private const int SMALL_DELAY_POPUP_MESSAGE_FRAMES = 90;
    private const int LARGE_DELAY_POPUP_MESSAGE_FRAMES = 170;
    private int messageTimer;

    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;

    private GameManager gameManager;
    private ValueTypes.gameState gameState;
    private bool messageOnScreen;


    void Start () {
        gameManager = GetComponent<GameManager>();

        SwitchGameState(ValueTypes.gameState.start);

        RestoreMessageTimer();
        HideMessage();
    }
	

    void Update () {
        NeedToShowMessage();
    }

    public void UpdateScore() {
        scoreText.text = "Результат: " + gameManager.GetScore();
    }

    public void DisplayNewBestScore(int newBestScore) {
        bestScoreText.text = "Лучший результат: " + newBestScore + "!";
    }

    public void ShowMessage(ValueTypes.gameState gameState) {
        messageText.transform.localScale = new Vector3(1, 1);
       
         switch(gameState) {
            case ValueTypes.gameState.start:
                messageText.text = "Нажмите любую кнопку что бы начать игру!";
                break;
            case ValueTypes.gameState.playerTurn:
                messageText.text = "Повторите последовательность!";
                break;
            case ValueTypes.gameState.win:
                messageText.text = "Победа!!! Нажмите любую кнопку что бы начать игру!";
                break;
            case ValueTypes.gameState.lose:
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

    public void SwitchGameState(ValueTypes.gameState newGameState) {
        gameState = newGameState;
    }

    private void NeedToShowMessage() {
        if (gameState != ValueTypes.gameState.task) {
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
        } else
            HideMessage();
    }

    private void RestoreMessageTimer() {
        if (gameState == ValueTypes.gameState.start | gameState == ValueTypes.gameState.win | gameState == ValueTypes.gameState.lose)
            messageTimer = SMALL_DELAY_POPUP_MESSAGE_FRAMES;
        else
            messageTimer = LARGE_DELAY_POPUP_MESSAGE_FRAMES;
    }
}
