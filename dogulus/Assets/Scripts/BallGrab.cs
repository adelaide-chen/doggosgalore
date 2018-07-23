using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGrab : RWVR_SimpleGrab {

    public DogController dogController;

    public override void OnTriggerWasPressed(RWVR_InteractionController controller)
    {
        base.OnTriggerWasPressed(controller);
        dogController.Stop();
    }

    public override void OnTriggerWasReleased(RWVR_InteractionController controller)
    {
        base.OnTriggerWasReleased(controller);
        dogController.Go();
    }

}
