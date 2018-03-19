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
    public GameObject linkedPrefab;
    public GameObject objectBeingHeld;
    public GameObject instantiatedPrefab;

    //The scene parent to drop the transforms to
    public Transform sceneParent;

    //What hands are used in the interaction    
    public bool noHandIK;
    public bool isRightHand;
    public bool isLeftHand;
    public bool isBothHands;

    //Audio data
    private AudioSource audioSource;
    public AudioClip inventoryClipToPlay;
    public AudioClip holdClipToPlay;
    public AudioClip dropClipToPlay;
    public AudioClip interactionClipToPlay;


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

    void PickUpObject()
    {
        //GameObject toPickUp = interactionObject.gameObject;

        if (!isHoldingObject)
        {
            if (isRightHand)
            {
                interactionObject.rightHandTarget.parent = sceneParent.transform;
                character.HoldObject(interactionObject.gameObject, Hand.Right);
                SetHoldPositionRotation(interactionObject.transform, interactionObject.rightHoldPosition);

            }
            else if (isLeftHand)
            {
                interactionObject.leftHandTarget.parent = sceneParent.transform;
                character.HoldObject(interactionObject.gameObject, Hand.Left);
                SetHoldPositionRotation(interactionObject.transform, interactionObject.leftHoldPosition);
            }
            else if (isBothHands)
            {
                interactionObject.leftHandTarget.parent = sceneParent.transform;
                character.HoldObject(interactionObject.gameObject, Hand.Left);
                SetHoldPositionRotation(interactionObject.transform, interactionObject.leftHoldPosition);
                SetHoldPositionRotation(interactionObject.transform, interactionObject.rightHoldPosition);
            }

            PlayHoldSound(holdClipToPlay);
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

            PlayDropSound(dropClipToPlay);
        }

    }

    void TakeOutObject()
    {
        if (!isHoldingObject)
        {
            instantiatedPrefab = Instantiate(linkedPrefab) as GameObject;
            interactionObject = instantiatedPrefab.GetComponentInChildren<SimpleInteractionObject>();

            if (isLeftHand)
            {
                character.HoldObject(interactionObject.gameObject, Hand.Left);
                SetHoldPositionRotation(interactionObject.gameObject.transform, interactionObject.leftHoldPosition);
            }
            if (isRightHand)
            {
                character.HoldObject(interactionObject.gameObject, Hand.Right);
                SetHoldPositionRotation(interactionObject.gameObject.transform, interactionObject.rightHoldPosition);
            }

            isHoldingObject = true;
            PlaySFXSound(inventoryClipToPlay);
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
            
            PlaySFXSound(inventoryClipToPlay);
            isHoldingObject = false;
            ClearData();

        }
    }

    void SetHoldPositionRotation(Transform _transform, Transform handHold)
    {
        _transform.localRotation = handHold.localRotation;
        _transform.localPosition = handHold.localPosition;        

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
        _transform.transform.localPosition = interactionObject.dropPosition.transform.localPosition;
        _transform.transform.localRotation = interactionObject.dropPosition.transform.localRotation;
        
        //_transform.transform.DOLocalMove(interactionObject.dropPosition.transform.localPosition, 0.5f, false);
        //_transform.transform.DOLocalRotate(interactionObject.dropPosition.transform.localRotation.eulerAngles, 0.5f);
        isHoldingObject = false;

    }

    public void ClearData()
    {
        this.interactionObject = null;
        this.linkedPrefab = null;
        this.sceneParent = null;
        this.objectBeingHeld = null;
        this.noHandIK = true;
        this.isRightHand = false;
        this.isLeftHand = false;
        this.isBothHands = false;
        this.inventoryClipToPlay = null;
        this.holdClipToPlay = null;
        this.dropClipToPlay = null;
        this.interactionClipToPlay = null;

    }
}
