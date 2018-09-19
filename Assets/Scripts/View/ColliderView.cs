using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum ColliderArea
{
    /// <summary>
    /// 未碰撞
    /// </summary>
    NONE,
    /// <summary>
    /// 货物前面
    /// </summary>
    GOOD_FRONT,
    /// <summary>
    /// 货物后面
    /// </summary>
    GOOD_BACK,
    /// <summary>
    /// 货物左侧
    /// </summary>
    GOOD_LEFT,
    /// <summary>
    /// 货物右侧
    /// </summary>
    GOOD_RIGHT,
    /// <summary>
    /// 货物上侧
    /// </summary>
    GOOD_UP,
    /// <summary>
    /// 货物下侧
    /// </summary>
    GOOD_DOWN,
    /// <summary>
    /// 货物左前腿
    /// </summary>
    GOODLEG_LEFTFRONT,
    /// <summary>
    /// 货物左后腿
    /// </summary>
    GOODLEG_LEFTBACK,
    /// <summary>
    /// 货物右前方腿
    /// </summary>
    GOODLEG_RIGHTFRONT,
    /// <summary>
    /// 货物右后方腿
    /// </summary>
    GOODLEG_RIGHTBACK,
    /// <summary>
    /// 桌子
    /// </summary>
    DESK,
    /// <summary>
    /// 自身
    /// </summary>
    SELF,
    /// <summary>
    /// 地面
    /// </summary>
    FLOOR,
}


public class ColliderView : MonoBehaviour {
    public ColliderArea colliderArea;

    public OnCollisionEnterEvent onCollisionEnterEvent;
    private void OnTriggerEnter(Collider collision)
    {
        onCollisionEnterEvent.Invoke((int)colliderArea);
    }

    [Serializable]
    public class OnCollisionEnterEvent : UnityEvent<int>
    {
       public OnCollisionEnterEvent() { }
    }
}
