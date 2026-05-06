using UnityEngine;

public class Room : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            RoomCamera.instance.MoveToRoom(new Vector3(transform.position.x, 0, -10));
    }
}