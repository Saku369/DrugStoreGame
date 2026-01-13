using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0, 11, -7);
    public float followSpeed = 8f;

    [Header("Camera Bounds")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;

        // ★カメラ位置を制限
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
        desiredPos.z = Mathf.Clamp(desiredPos.z, minZ, maxZ);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );
    }
}
