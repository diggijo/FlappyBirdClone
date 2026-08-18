using Unity.VisualScripting;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private Vector3 startPos;
    private float repeatWidth;
    

    private void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update()
    {
        if (!GameManager.Instance.GetTimerActive())
        {
            return;
        }

        transform.Translate(Vector3.left * Time.deltaTime * parallaxEffect);

        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
