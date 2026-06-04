using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BoardingPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private BoardingController boardingController;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (boardingController == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                boardingController = player.GetComponent<BoardingController>();
            }
        }

        Hide();
    }

    private void Update()
    {
        if (PlayerState.IsDocked || boardingController == null)
        {
            Hide();
            return;
        }

        if (boardingController.CanBoardCurrentTarget)
        {
            int chance = boardingController.GetCurrentBoardingChancePercent();
            Show(chance);
        }
        else
        {
            Hide();
        }
    }

    private void Show(int chancePercent)
    {
        if (promptText != null)
        {
            promptText.text = $"Press B to Board — {chancePercent}% chance";
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}