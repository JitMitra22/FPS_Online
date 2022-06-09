using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Game Settings")]
    public Text fpsCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        fpsCount.text = "FPS:" + GameManager.instance.frameRate.ToString();
    }


}
