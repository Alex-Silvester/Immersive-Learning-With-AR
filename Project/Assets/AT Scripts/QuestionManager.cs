using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text points_text;

    int points;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points = 0;
    }

    public void addPoints(int added_points)
    {
        points += added_points;
        points_text.SetText($"Points: {points}");
    }
}
