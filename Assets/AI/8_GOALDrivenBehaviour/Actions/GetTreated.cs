using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreated : GAction
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
        {
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("WaitingDoctor", 1);
        GWorld.Instance.AddPatientForDoctors(gameObject);
        beliefs.ModifyState("isWaitingDoctor", 1);
        
        return true;
    }
}
