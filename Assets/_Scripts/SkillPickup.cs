using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillPickup : MonoBehaviour
{
    public string skillStateName;

    private TextMeshProUGUI skillText;

    [SerializeField]
    private string skillDisplayName;

    // Start is called before the first frame update
    void Start()
    {
        this.skillText = GetComponentInChildren<TextMeshProUGUI>();
        this.skillText.text = this.skillDisplayName;
    }    
}
