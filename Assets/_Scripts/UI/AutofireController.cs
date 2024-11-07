using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutofireController : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    // Start is called before the first frame update

    public static Action<bool> ToggleValueChangedEvent = null;
    
    void Start()
    {
        toggle.onValueChanged.AddListener(arg0 =>
        {
            ToggleValueChangedEvent?.Invoke(arg0);
        });
    }
}
