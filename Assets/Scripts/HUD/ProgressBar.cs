using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public int min;
    public int max;
    public int cur;
    public Image mask;
    public Image fill;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetFill();
    }

    void GetFill()
    {
        float curOffset = Player_Currency.money - min;
        float maxOffset = max - Player_Currency.money;
        float fillAmount = curOffset / maxOffset;
        mask.fillAmount = fillAmount;
        fill.color = color;
    }
}
