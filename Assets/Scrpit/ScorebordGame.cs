using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorebordGame : MonoBehaviour
{
    [SerializeField] private GameObject Name1;
    [SerializeField] private GameObject Score1;
    [SerializeField] private GameObject Name2;
    [SerializeField] private GameObject Score2;
    [SerializeField] private GameObject Name3;
    [SerializeField] private GameObject Score3;
    [SerializeField] private GameObject Name4;
    [SerializeField] private GameObject Score4;
    [SerializeField] private GameObject Name5;
    [SerializeField] private GameObject Score5;
    [SerializeField] private GameObject Name6;
    [SerializeField] private GameObject Score6;
    [SerializeField] private GameObject Name7;
    [SerializeField] private GameObject Score7;
    [SerializeField] private GameObject Name8;
    [SerializeField] private GameObject Score8;

    public void SetScorebord(List<Scoreboard> scores)
    {
        List<GameObject> Name = new List<GameObject>() { Name1, Name2, Name3, Name4, Name5, Name6, Name7, Name8 };
        List<GameObject> Score = new List<GameObject>() { Score1, Score2, Score3, Score4, Score5, Score6, Score7, Score8 };
        for (int i = 0; i < scores.Count; i++)
        {
            Name[i].GetComponent<TMPro.TextMeshProUGUI>().text = scores[i].name;
            Score[i].GetComponent<TMPro.TextMeshProUGUI>().text = scores[i].score.ToString();
        }
    }
}
