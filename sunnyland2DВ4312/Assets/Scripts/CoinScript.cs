using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int cost = 1;

    void OnTriggerEnter2D(Collider2D other) //функция запускается при столкновении с другим объектом
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().Score += cost;
            Destroy(gameObject);
        }
    }
}
