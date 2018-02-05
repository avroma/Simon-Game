using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public ValueTypes.buttonType thisButton;
    private Animator animator;
    private GameManager gameManager;

    public void ClickButton() {
        animator.SetTrigger("BlinkButton");
    }

    public void OnClick() {
        gameManager.PlayerClick(thisButton);
    }

    void Start () {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }
}
