using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    public static RoomCamera instance;
    public float smoothSpeed = 5f;
    Vector3 targetPos;

    void Awake()
    {
        instance = this;
        targetPos = transform.position;
    }

    void LateUpdate()
{
    if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        transform.position = targetPos;
    else
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
}

    public void MoveToRoom(Vector3 newPos)
    {
        targetPos = new Vector3(newPos.x, newPos.y, -10);
    }
}