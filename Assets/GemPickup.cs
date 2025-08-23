using UnityEngine;

public class GemPickup : MonoBehaviour
{
    // Hàm gọi khi Player va chạm với gem
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // kiểm tra đối tượng có tag Player
        {
            GameManager.Instance.AddGem(); // cộng gem cho GameManager
            Destroy(gameObject);           // xóa gem khỏi scene
        }
    }
}
