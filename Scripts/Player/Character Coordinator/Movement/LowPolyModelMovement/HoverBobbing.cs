using UnityEngine;

public class HoverBobbing : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.15f;

    private Vector3 startPos;

    void Start() => startPos = transform.localPosition;

    void Update()
    {
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }
}
