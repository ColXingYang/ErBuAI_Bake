using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour  where T : MonoBehaviour{
    //定义静态成员（静态的可以通过类名访问）
    private static T instance;
    //封装属性
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //用代码生成一个GameObject
                GameObject obj = new GameObject();
               
                obj.name = "Singleton";
                //给生成的物体添加脚本(将这个方法的返回值赋给instance)
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = GetComponent<T>();
        Init();
    }

    public virtual void Init()
    {

    }
}
