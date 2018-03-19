using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class InteractionSystem : MonoBehaviour {

    //The player
    private Char character;
    public bool isMainCharacter;
    public bool isHoldingObject = false;
    public GameObject objectBeingHeld;

    //The Interaction Object List to load from
    public InteractionObjectList interactionList;

    //The Interaction Object 
    public InteractionObject interactionObject;

    //The associated inventory item
    public GameObject instantiatedPrefab;
    public int invID;

    //What hands are used in the interaction    
    public bool noHandIK;
    public bool isRightHand;
    public bool isLeftHand;
    public bool isBothHands;

    //Audio data
    private AudioSource audioSource;

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
        if (!isHoldingObject)
        {
            if (isRightHand)
            {
                character.HoldObject(interactionObject.interactionObject, Hand.Right);
                SetHoldPositionRotation(interactionObject.interactionObject.transform, interactionObject.rHandPosition);

            }
            else if (isLeftHand)
            {
                character.HoldObject(interactionObject.interactionObject, Hand.Left);
                SetHoldPositionRotation(interactionObject.interactionObject.transform, interactionObject.lHandPosition);
            }
            else if (isBothHands)
            {
                character.HoldObject(interactionObject.interactionObject, Hand.Left);
                SetHoldPositionRotation(interactionObject.interactionObject.transform, interactionObject.lHandPosition);
                SetHoldPositionRotation(interactionObject.interactionObject.transform, interactionObject.rHandPosition);
            }
            PlayHoldSound(interactionObject.pickUpSound);
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
            Transform eParent = interactionObject.objectParent;
            character.ReleaseHeldObjects();
            SetReleasePositionRotation(interactionObject, eParent);

            PlayDropSound(interactionObject.dropSound);
        }

    }

    void TakeOutObject()
    {

        if (!isHoldingObject)
        {
            //Find the right object to instantiate from the inventory. This will be the whole object, including the parent.
            instantiatedPrefab = Instantiate(AC.KickStarter.runtimeInventory.GetItem(interactionObject.invID).linkedPrefab) as GameObject;

            //Parent is instantiated, now grab the object from the children
            interactionObject = instantiatedPrefab.GetComponentInChildren<InteractionObject>();

            if (isLeftHand)
            {
                character.HoldObject(interactionObject.interactionObject, Hand.Left);
                SetHoldPositionRotation(interactionObject.transform, interactionObject.lHandPosition);
            }
            if (isRightHand)
            {
                character.HoldObject(interactionObject.interactionObject, Hand.Right);
                SetHoldPositionRotation(interactionObject.transform, interactionObject.rHandPosition);
            }

            isHoldingObject = true;
            PlaySFXSound(interactionObject.inventorySound);
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
            Destroy(interactionObject.objectParent.gameObject);

            PlaySFXSound(interactionObject.inventorySound);
            isHoldingObject = false;
            ClearData();

        }
    }

    void SetHoldPositionRotation(Transform _transform, Transform handHold)
    {
        _transform.localRotation = handHold.localRotation;
        _transform.localPosition = handHold.localPosition;

        objectBeingHeld = interactionObject.interactionObject;

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

    void SetReleasePositionRotation(InteractionObject _object, Transform _parent)
    {
        _object.interactionObject.transform.parent = _parent;
        _object.interactionObject.transform.localPosition = interactionObject.dropPosition.localPosition;
        _object.interactionObject.transform.localRotation = interactionObject.dropPosition.localRotation;
        isHoldingObject = false;
    }

    public void ClearData()
    {
        this.interactionObject = null;
        this.objectBeingHeld = null;
        this.noHandIK = true;
        this.isRightHand = false;
        this.isLeftHand = false;
        this.isBothHands = false;

    }
}
