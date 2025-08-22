using UnityEngine;
using TMPro;

public class TriggerPuzzle : MonoBehaviour
{
    [Header("Puzzle UI Panel")]
    public GameObject puzzleUI;       // UI Puzzle Panel
    public GameObject pressEHint;     // Thông báo "Nhấn E"
    public TextMeshProUGUI questionText; // Text câu hỏi
    [TextArea] public string puzzleQuestion; // Nội dung câu hỏi

    private bool playerInRange = false;
    private bool puzzleOpen = false;

    private Player playerScript;  // script Player.cs

    void Start()
    {
        if (puzzleUI != null) puzzleUI.SetActive(false);
        if (pressEHint != null) pressEHint.SetActive(false);

        // cache Player.cs để khóa khi mở puzzle
        playerScript = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (playerInRange && !puzzleOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle();
            Input.ResetInputAxes(); // tránh bị double trigger
        }

        if (puzzleOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
            Input.ResetInputAxes();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (pressEHint != null) pressEHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pressEHint != null) pressEHint.SetActive(false);
        }
    }

    void OpenPuzzle()
    {
        puzzleOpen = true;
        puzzleUI?.SetActive(true);
        pressEHint?.SetActive(false);

        // Gán câu hỏi
        if (questionText != null) questionText.text = puzzleQuestion;

        // Khóa Player.cs
        if (playerScript != null) playerScript.enabled = false;
    }

    public void ClosePuzzle()
    {
        puzzleOpen = false;
        puzzleUI?.SetActive(false);

        // Mở lại Player.cs
        if (playerScript != null) playerScript.enabled = true;
    }
}
