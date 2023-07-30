using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerController.transform.position + new Vector3(0, 10, -10);
    }
}
