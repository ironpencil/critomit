using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T) FindObjectOfType(typeof(T));

                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load(typeof(T).Name) as GameObject).GetComponent<T>();
                }
            }

            return _instance;
        }
    }

    //destroy this object if an instance already exists
    public void Start()
    {
        Debug.Log("Singleton<" + typeof(T).Name + ">::Start()");
        if (this != Instance)
        {
            gameObject.SetActive(false);
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
