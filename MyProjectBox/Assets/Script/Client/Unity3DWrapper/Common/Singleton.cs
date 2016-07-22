using UnityEngine;
using System.Collections;

/// <summary>
/// 单件基类，游戏所有的初始化，行为以及销毁都继承这个
/// 单例返回的静态参数无法生成一个更丰富的子类，所有每个子类都需要自己去new一次自己。
/// </summary>
public class Singleton
{
	public static Singleton Inst = null;

	public bool m_isAutoUpdate = false;
	
	public Singleton(bool isAuto)
	{
		m_isAutoUpdate = isAuto;
	}
	
	public bool IsAutoUpdate()
	{
		return m_isAutoUpdate;
	}
	
	public virtual void Init(){}

    public virtual void Update(float fTime, float fDTime) { }
	
	public virtual void Destroy(){}
	
}
