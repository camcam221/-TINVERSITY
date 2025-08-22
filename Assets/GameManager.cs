using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton → để script khác có thể gọi GameManager dễ dàng

    public int totalGems = 4;            // Tổng số gem cần để thắng (ở đây là 4)
    private int collectedGems = 0;       // Biến đếm số gem đã thu thập

    [Header("HUD")]
    public TMP_Text gemCounterText;      // UI Text hiển thị số gem
    public GameObject winPanel;          // UI Panel thông báo thắng game

    void Awake()
    {
        // Singleton: đảm bảo chỉ có 1 GameManager tồn tại
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Khởi tạo HUD và ẩn Win Panel
        UpdateHUD();
        if (winPanel != null) winPanel.SetActive(false);
    }

    // Hàm gọi khi player nhặt gem
    public void AddGem()
    {
        collectedGems++;     // tăng số gem đã nhặt
        UpdateHUD();         // cập nhật HUD

        if (collectedGems >= totalGems)  // nếu đủ gem
        {
            WinGame();       // gọi hàm thắng game
        }
    }

    // Cập nhật text trên HUD
    void UpdateHUD()
    {
        if (gemCounterText != null)
            gemCounterText.text = $"Gems: {collectedGems}/{totalGems}";
    }

    // Xử lý khi thắng
    void WinGame()
    {
        if (winPanel != null) winPanel.SetActive(true);  // bật bảng thắng
        Debug.Log("Tuyệt vời! Bạn đã thu thập đủ 4 viên ngọc!");
        // Có thể load scene thắng ở đây: SceneManager.LoadScene("WinScene");
    }
}
