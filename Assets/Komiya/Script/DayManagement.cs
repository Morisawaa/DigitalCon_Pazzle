using Unity.VisualScripting;
using UnityEngine;

public class DayManagement : MonoBehaviour
{
    [SerializeField] private ValueManagement ValueManagement_;

    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    private void InitialDay()
    {
        Debug.LogWarning("“ú•t‚ª‰Šú‰»‚³‚ê‚Ü‚µ‚½");
        ValueManagement_.WhatDay = ValueManagement_.InitialWhatDay;
    }

    /// <summary>
    /// Ÿ‚Ì“ú‚Ö
    /// </summary>
    public void NextDay()
    {
        ValueManagement_.WhatDay++;
    }

    /// <summary>
    /// ‘O‚Ì“ú‚Ö
    /// </summary>
    public void PreviousDay()
    {
        ValueManagement_.WhatDay--;
    }
}
