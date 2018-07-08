using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWVR_InteractionController : MonoBehaviour {
    public Transform snapColliderOrigin; // 1
    public GameObject ControllerModel; // 2
    public bool pressed;

    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public Vector3 angularVelocity;
    private RWVR_InteractionObject objectBeingInteractedWith; // 5

    private SteamVR_TrackedObject trackedObj; // 6

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public RWVR_InteractionObject InteractionObject
    {
        get { return objectBeingInteractedWith; }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void CheckForInteractionObject()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(snapColliderOrigin.position, snapColliderOrigin.lossyScale.x / 2f);
        print("entered function");
        print(overlappedColliders);
        foreach (Collider overlappedCollider in overlappedColliders)
        {
            print("entered for loop");
            if (overlappedCollider.CompareTag("InteractionObject") && overlappedCollider.GetComponent<RWVR_InteractionObject>().IsFree())
            {
                print("entered if statement");
                objectBeingInteractedWith = overlappedCollider.GetComponent<RWVR_InteractionObject>();
                objectBeingInteractedWith.OnTriggerWasPressed(this);
                return;
            }
        }
    }

    void Update()
    {
        if (Controller.GetHairTriggerDown())
        {
            print("triggered - in more ways than one");
            pressed = true;
            CheckForInteractionObject();
        }

        if (Controller.GetHairTrigger())
        {
            if (objectBeingInteractedWith)
            {
                objectBeingInteractedWith.OnTriggerIsBeingPressed(this);
            }
        }

        if (Controller.GetHairTriggerUp())
        {
            if (objectBeingInteractedWith)
            {
                objectBeingInteractedWith.OnTriggerWasReleased(this);
                objectBeingInteractedWith = null;
            }
        }
    }

    private void UpdateVelocity()
    {
        velocity = Controller.velocity;
        angularVelocity = Controller.angularVelocity;
    }

    void FixedUpdate()
    {
        UpdateVelocity();
    }

    public void Vibrate(ushort strength)
    {
        Controller.TriggerHapticPulse(strength);
    }

    public void SwitchInteractionObjectTo(RWVR_InteractionObject interactionObject)
    {
        objectBeingInteractedWith = interactionObject;
        objectBeingInteractedWith.OnTriggerWasPressed(this);
    }
}
