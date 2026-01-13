using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    public int score; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
    {
        score = 0;
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "score:0";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "score:" + score.ToString();
    }
}
