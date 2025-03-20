using UnityEngine;
using System.Collections.Generic;

public class HeartManager : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private float heartSpacing = 50f;
    private List<GameObject> hearts = new List<GameObject>();
    private PlayerStatus ps;

    void Start()
    {
        ps = FindFirstObjectByType<PlayerStatus>();
        if (ps != null)
        {
            UpdateHearts(ps.GetLives());
        }
    }

    void Update()
    {
        if (ps != null && hearts.Count != ps.GetLives())
        {
            UpdateHearts(ps.GetLives());
        }
    }

    private void UpdateHearts(int lives)
    {
        // Remove excess hearts if lives decreased
        while (hearts.Count > lives)
        {
            GameObject heartToRemove = hearts[hearts.Count - 1];
            hearts.RemoveAt(hearts.Count - 1);
            Destroy(heartToRemove);
        }

        while (hearts.Count < lives)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            hearts.Add(newHeart);
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].transform.position = new Vector3(i * heartSpacing, 0, 0);
        }
    }
}