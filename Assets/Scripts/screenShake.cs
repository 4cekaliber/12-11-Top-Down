using System.Collections;
using UnityEngine;

public class screenShake : MonoBehaviour
{
    public bool start = false;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float duration = 1f;
    private float elapsedTime;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shake());
        }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
