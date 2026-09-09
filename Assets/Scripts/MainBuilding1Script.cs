using UnityEngine;

public class MainBuilding1Script : MonoBehaviour
{
    private GameObject firstBackground;
    private GameObject secondBackground;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        firstBackground = GameObject.Find("Main Building (1)");
        secondBackground = GameObject.Find("Main Building (2)");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    firstBackground.SetActive(false);
    //    firstBackground.SetActive(true);
    //}
}
