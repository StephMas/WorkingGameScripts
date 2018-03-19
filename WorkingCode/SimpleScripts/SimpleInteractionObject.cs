using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;


public class SimpleInteractionObject : MonoBehaviour
{
    public Animator animator;
    public AnimationClip clipToPlay;

    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform rightHoldPosition;
    public Transform leftHoldPosition;
    public Marker dropPosition;
    public Transform emptyParent;

    public AudioClip holdSound;
    public AudioClip dropSound;
    public AudioClip interactionSound;
    public AudioClip inventorySound;
}

