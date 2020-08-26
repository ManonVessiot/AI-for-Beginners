using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GAgent
{
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);


        SubGoal s2 = new SubGoal("isWaitingDoctor", 1, true);
        goals.Add(s2, 3);


        SubGoal s3 = new SubGoal("isCured", 1, true);
        goals.Add(s3, 3);


        SubGoal s4 = new SubGoal("isHome", 1, true);
        goals.Add(s4, 3);

    }
}
