using UnityEngine;

public class GlowinAnimation : MonoBehaviour
{
    [SerializeField] private float glowSpeed = 2f;
    [SerializeField] private Color glowColor = Color.white;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float emission = Mathf.PingPong(Time.time * glowSpeed, 1.0f);
        Color finalColor = glowColor * Mathf.LinearToGammaSpace(emission);
        material.SetColor("_EmissionColor", finalColor);
    }
}