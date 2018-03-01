using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AC;

public class SimpleInteractionSystem : MonoBehaviour {

    //The player
    private Char character;
    public bool isMainCharacter;
    public bool isHoldingObject = false;

    //The object data
    public SimpleInteractionObject interactionObject;
    public Transform leftHoldPosition;
    public Transform rightHoldPosition;
    public Marker dropMarker;
    public Transform emptyParent;
    public GameObject linkedPrefab;
    public GameObject objectBeingHeld;
    public GameObject instantiatedPrefab;
    
    //What hands are used in the interaction
    public enum HandsUsed { RightHand, LeftHand, BothHands};
    public HandsUsed handsUsed;

    //For the animation behaviour curves
    public Transform rightHandTarget;
    public Transform leftHandTarget;

    //Audio data
    private AudioSource audioSource;
    public AudioClip holdSound;
    public AudioClip dropSound;
    public AudioClip interactionSound;
    public AudioClip inventorySound;

    private void Start()
    {
        if (isMainCharacter)
        {
            character = KickStarter.player;
        }
        else
        {
            character = GetComponent<NPC>();
        }
        audioSource = character.GetComponent<AudioSource>();
        isHoldingObject = false;

    }

    //This will be called to pick up an object. It doesn't have a dependency, but should not be called if you're holding something.
    //Uses animation curve data for the interaction and you can only use one hand at a time.
    void PickUpObject()
    {
        GameObject toPickUp = interactionObject.gameObject;

        if (!isHoldingObject)
        {
            if (handsUsed == HandsUsed.RightHand)
            {
                interactionObject.rightHandTarget.parent = emptyParent.transform;
                character.HoldObject(toPickUp, Hand.Right);
                SetHoldPositionRotation(interactionObject.transform, rightHoldPosition);

            }
            else if (handsUsed == HandsUsed.LeftHand)
            {
                interactionObject.leftHandTarget.parent = emptyParent.transform;
                character.HoldObject(toPickUp, Hand.Left);
                SetHoldPositionRotation(interactionObject.transform, leftHoldPosition);
            }

            PlayHoldSound(holdSound);
            isHoldingObject = true;
        }
        else
        {
            Debug.Log("You can't pick up another object until you drop the one you're holding.");
        }
    }

    void DropObject()
    {
        if (!isHoldingObject)
        {
            Debug.Log("You aren't holding anything to drop.");
        }
        else
        {
            GameObject toDrop = interactionObject.gameObject;
            Transform eParent = interactionObject.emptyParent;
            character.ReleaseHeldObjects();
            SetReleasePositionRotation(toDrop.transform, eParent);
            PlayDropSound(dropSound);
        }

    }

    void TakeOutObject()
    {
        if (!isHoldingObject)
        {
            instantiatedPrefab = Instantiate(linkedPrefab) as GameObject;
            interactionObject = instantiatedPrefab.GetComponentInChildren<SimpleInteractionObject>();

            character.HoldObject(interactionObject.gameObject, Hand.Left);
            SetHoldPositionRotation(interactionObject.gameObject.transform, leftHoldPosition);
            isHoldingObject = true;
            PlaySFXSound(inventorySound);
        }
        else
        {
            Debug.Log("You can't take out another object until you drop the one you're holding.");
        }

    }

    void PutObjectAway()
    {
        //Get the object being held
        if (isHoldingObject)
        {
            Destroy(objectBeingHeld);
            if (instantiatedPrefab != null)
            {
                Destroy(instantiatedPrefab);
            }
            
            PlaySFXSound(inventorySound);
            isHoldingObject = false;
        }
    }

    void SetHoldPositionRotation(Transform _transform, Transform handHold)
    {
        _transform.localRotation = handHold.localRotation;
        _transform.localPosition = handHold.localPosition;
        
        //_transform.DOLocalMove(holdPosition.transform.localPosition, 0.25f, false);
        //_transform.DOLocalRotate(holdPosition.transform.localRotation.eulerAngles, 0.25f);
        objectBeingHeld = interactionObject.gameObject;

    }

    void PlaySFXSound(AudioClip _audioClip)
    {
        if (_audioClip != null)
        {
            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.Log("There is no INTERACTION sound identified for this object.");
        }
    }


    void PlayHoldSound(AudioClip _audioClip)
    {
        if (_audioClip != null)
        {
            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.Log("There is no HOLD sound identified for this object.");
        }

    }

    void PlayDropSound(AudioClip _audioClip)
    {
        if (_audioClip != null)
        {
            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.Log("There is no DROP sound identified for this object.");
        }

    }

    void SetReleasePositionRotation(Transform _transform, Transform _parent)
    {
        _transform.parent = _parent;
        _transform.transform.DOLocalMove(dropMarker.transform.localPosition, 0.5f, false);
        _transform.transform.DOLocalRotate(dropMarker.transform.localRotation.eulerAngles, 0.5f);
        isHoldingObject = false;

        ClearData();

    }

    public void ClearData()
    {
        this.interactionObject = null;
        this.rightHoldPosition = null;
        this.leftHoldPosition = null;
        this.dropMarker = null;
        this.linkedPrefab = null;
        this.rightHandTarget = null;
        this.leftHandTarget = null;
    }
}
