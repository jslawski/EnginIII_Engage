using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public Dictionary<string, bool> skillDict;

    // Start is called before the first frame update
    public void SetupDict()
    {
        this.skillDict = new Dictionary<string, bool>();
        this.AddSkill("IdleState");
        this.AddSkill("MoveState");
        this.AddSkill("FallState");
        this.AddSkill("JumpState");
    }

    public void AddSkill(string newSkill)
    {
        this.skillDict.Add(newSkill, true);
    }

    public bool HasSkill(string checkedSkill)
    {
        return this.skillDict.ContainsKey(checkedSkill);
    }
}
