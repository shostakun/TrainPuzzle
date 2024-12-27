using UnityEngine;

public class VaryColor : MonoBehaviour
{
    public float minFrequency = 0.1f;
    public float maxFrequency = 0.5f;
    public float amplitude = 0.5f;
    private float H, S, V;
    private float frequency = 1;
    private float phase = 0;
    [SerializeField]
    public Color highlightColor = Color.yellow;
    public float crossFade = 0.2f;
    private SpriteRenderer renderer_;

    void Start()
    {
        frequency = Random.Range(minFrequency, maxFrequency);
        phase = Random.Range(0, 2 * Mathf.PI);
        renderer_ = GetComponent<SpriteRenderer>();
        Color.RGBToHSV(renderer_.color, out H, out S, out V);
    }

    void Update()
    {
        bool isHighlighted = Board.inst.highlight.x == Mathf.RoundToInt(transform.position.x) &&
            Board.inst.highlight.y == Mathf.RoundToInt(transform.position.y);
        Color normal = Color.HSVToRGB(H, S, V + amplitude * Mathf.Sin(Time.time * frequency + phase));
        if (isHighlighted)
        {
            renderer_.color = Color.Lerp(renderer_.color, highlightColor, crossFade);
        }
        else
        {
            renderer_.color = Color.Lerp(renderer_.color, normal, crossFade);
        }
    }
}
