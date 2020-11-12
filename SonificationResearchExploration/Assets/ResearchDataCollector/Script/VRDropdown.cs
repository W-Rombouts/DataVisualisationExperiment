using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    bool dropdownIsCLosed = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDropdown()
    {
        if (dropdownIsCLosed)
        {
            dropdown.Show();
            
        }
        else{
            dropdown.Hide();
        }
        dropdownIsCLosed = !dropdownIsCLosed;
    }
}

