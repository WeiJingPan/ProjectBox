using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPanelInitRes : UI 
{
    public Text m_txt_schedule;
    public Slider m_slider;

    public override void Init()
    {
        base.Init();
        m_txt_schedule = m_root.FindChild("panel/txt").GetComponent<Text>();
        m_slider = m_root.FindChild("panel/slider").GetComponent<Slider>();
    }
    public void SetTxt(string value)
    {
        m_txt_schedule.text = value;
    }
    public void SetProgress(float value)
    {
        m_slider.value = (int)(value * 100);
    }
}
