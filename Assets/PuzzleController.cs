using UnityEngine;
using TMPro;

public class PuzzleController : MonoBehaviour
{
    public string correctAnswer;          // Đáp án đúng cho puzzle
    public GameObject puzzleUI;           // UI Panel puzzle
    public TMP_InputField answerInput;    // Ô nhập đáp án
    public GameObject gemPrefab;          // Prefab gem sẽ spawn khi giải đúng
    public Transform spawnPoint;          // Vị trí spawn gem
    public GameObject hintText;           // Gợi ý (ẩn ban đầu)

    private int wrongAttempts = 0;        // Đếm số lần trả lời sai
    private bool solved = false;          // Đánh dấu puzzle đã giải xong

    // Hàm gọi khi người chơi nhấn nút Submit
    public void CheckAnswer()
    {
        if (solved) return; // nếu đã giải rồi thì bỏ qua

        // Lấy đáp án người chơi nhập, bỏ khoảng trắng và đổi sang chữ thường
        string playerAnswer = answerInput.text.Trim().ToLower();

        if (playerAnswer == correctAnswer.ToLower()) // so sánh với đáp án đúng
        {
            solved = true;              // đánh dấu puzzle đã giải
            puzzleUI.SetActive(false);  // tắt UI puzzle

            // Spawn gem tại vị trí spawnPoint
            if (gemPrefab != null && spawnPoint != null)
                Instantiate(gemPrefab, spawnPoint.position, Quaternion.identity);

            // Báo cho GameManager biết player nhận gem
            GameManager.Instance.AddGem();
        }
        else
        {
            wrongAttempts++; // tăng số lần sai
            if (wrongAttempts >= 3 && hintText != null)
                hintText.SetActive(true); // hiện gợi ý khi sai >= 3 lần
        }
    }
}
