using UnityEngine;
using TMPro;   // Dùng cho InputField kiểu TextMeshPro

public class AnswerChecker : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField answerInput;   // Ô nhập đáp án
    public GameObject puzzleUI;          // Panel UI puzzle
    public GameObject hintText;          // Gợi ý (ẩn lúc đầu)
    public GameObject gemReward;         // Gem sẽ được bật khi đúng

    [Header("Answer Settings")]
    public string correctAnswer = "16";  // Đáp án đúng
    private int wrongAttempts = 0;       // Đếm số lần sai
    public int maxAttempts = 3;          // Sai tối đa bao nhiêu lần thì gợi ý

    void Start()
    {
        if (puzzleUI != null) puzzleUI.SetActive(false);
        if (hintText != null) hintText.SetActive(false);
        if (gemReward != null) gemReward.SetActive(false);
    }

    // Hàm gọi khi nhấn nút "Submit"
    public void CheckAnswer()
    {
        string playerAnswer = answerInput.text.Trim();

        if (playerAnswer == correctAnswer)
        {
            Debug.Log("Đúng rồi!");
            if (gemReward != null) gemReward.SetActive(true); // hiện viên ngọc
            ClosePuzzle();
        }
        else
        {
            wrongAttempts++;
            Debug.Log("Sai rồi, thử lại!");

            if (wrongAttempts >= maxAttempts)
            {
                if (hintText != null) hintText.SetActive(true); // bật gợi ý
            }
        }
    }

    public void ClosePuzzle()
    {
        if (puzzleUI != null) puzzleUI.SetActive(false);

        // Bật lại điều khiển Player
        Player player = FindObjectOfType<Player>();
        if (player != null) player.enabled = true;
    }
}
