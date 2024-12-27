using System.Collections;
using UnityEngine;

public class TrainArrival : MonoBehaviour
{
    public float animationTime = 0.2f;
    protected float baseScale;
    public GameObject star;
    protected Track track;

    void Start()
    {
        star.SetActive(false);
        baseScale = star.transform.localScale.x;
        star.transform.localScale = Vector3.zero;
        track = GetComponentInParent<Track>();
        track.onTrainEnter += OnTrainEnter;
    }

    IEnumerator GrowStar()
    {
        while (star.transform.localScale.x < baseScale)
        {
            star.transform.localScale = Vector3.Lerp(star.transform.localScale, Vector3.one * baseScale, Time.deltaTime / animationTime);
            yield return null;
        }
        star.transform.localScale = Vector3.one * baseScale;
    }

    void OnTrainEnter()
    {
        star.SetActive(true);
        StartCoroutine(GrowStar());
    }
}
