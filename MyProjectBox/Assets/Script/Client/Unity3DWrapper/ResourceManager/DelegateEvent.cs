using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugEvent : MonoBehaviour
{
    public Button m_btn_event;
    public delegate void ThisDelegate(int i);

    void Start()
    {
        ReceiveDelegateArgsFunc(new ThisDelegate(DelegateFunction));
        Debug.Log("按钮的基本类型是" + m_btn_event.onClick);
    }
    public void ReceiveDelegateArgsFunc(ThisDelegate func)
    {
        func(5);
    }
    public void DelegateFunction(int i)
    {
        Debug.Log("传递的参数是：" + i);
    }
}
public class DelegateEvent
{
    public delegate void ThisDelegateEvent();
    public void AddEvent(Button button, ThisDelegateEvent thisEvent)
    {
        //button.onClick=thisEvent;
    }
}
