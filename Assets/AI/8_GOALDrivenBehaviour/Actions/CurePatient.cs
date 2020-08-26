using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurePatient : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemovePatientForDoctors();
        if (target == null)
        {
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("PatientTreated", 1);
        GWorld.Instance.GetWorld().ModifyState("WaitingDoctor", -1);
        if (target)
        {
            GAgent agentPatient = target.GetComponent<GAgent>();
            GameObject cubicle = agentPatient.inventory.FindItemWithTag("Cubicle");
            agentPatient.inventory.RemoveItem(cubicle);
            GWorld.Instance.AddCubicle(cubicle);
            agentPatient.beliefs.ModifyState("isCured", 1);
            GWorld.Instance.GetWorld().ModifyState("FreeCubicle", 1);
        }
        return true;
    }
}
