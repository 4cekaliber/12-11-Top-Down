using UnityEngine;

public class fogOfWar : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        
    }
    private void Awake()
    {
        player = GameObject.Find("Player");

    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
