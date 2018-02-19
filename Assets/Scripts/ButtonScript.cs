using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public ValueTypes.ButtonType thisButton;
    private Animator animator;
    private GameManager gameManager;

    public void Start() {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ClickButton() {
        animator.SetTrigger("BlinkButton");
    }

    public void OnClick() {
        gameManager.PlayerClick(thisButton);
    }
}
