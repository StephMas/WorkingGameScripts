using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using RootMotion.FinalIK;
using AC;


public class ObjectInteractControl : MonoBehaviour {

    //Main data
    public ObjectInteractControl controller;
    public InteractionSystem interactionSystem;
    public bool isMainCharacter = true;
    public Char player;
    public bool objectBeingHeld = false;

    //Object data
    public InteractionObject interactionObject;
    public FullBodyBipedEffector effector;
    public Transform holdPosition;
    public Marker dropMarker;
    public GameObject linkedPrefab;

    //Other data
    public enum InteractionType { PickUp, Drop, PutAway, TakeOut, InScene };
    public InteractionType interactionType;

    //Audio data
    public AudioSource audioSource;
    public AudioClip holdSound;
    public AudioClip dropSound;
    public AudioClip interactionSound;
    public AudioClip inventorySound;


    private void Start()
    {
        if (isMainCharacter)
        {
            player = KickStarter.player;
        }
        else
        {
            player = GetComponent<NPC>();
        }

        interactionSystem = player.GetComponent<InteractionSystem>();
        controller = player.GetComponent<ObjectInteractControl>();
        audioSource = player.GetComponent<AudioSource>();
        objectBeingHeld = false;

    }

    void PlayInteractionNow()
    {
        interactionSystem.StartInteraction(effector, interactionObject, true);
    }

    void PickUpObject()
    {
        GameObject toPickUp = interactionObject.gameObject;

        if (!objectBeingHeld)
        {
            if (effector == FullBodyBipedEffector.RightHand)
            {
                player.HoldObject(toPickUp, Hand.Right);
                SetHoldPositionRotation(interactionObject.transform);
            }
            else
            {
                player.HoldObject(toPickUp, Hand.Left);
                SetHoldPositionRotation(interactionObject.transform);
            }

            PlaySFXSound(holdSound);
        }
        else
        {
            Debug.Log("You can't pick up another object until you drop the one you're holding.");
        }    
    }
      

    void DropObject()
    {
        if (!objectBeingHeld)
        {
            Debug.Log("You aren't holding anything to drop.");
        }
        else
        {
            GameObject toDrop = interactionObject.gameObject;
            player.ReleaseHeldObjects();
            SetReleasePositionRotation(toDrop.transform);
            PlaySFXSound(dropSound);
        }

    }

    void TakeOutObject()
    {
        if (!objectBeingHeld)
        {
            Instantiate(linkedPrefab);
            SetHoldPositionRotation(linkedPrefab.transform);
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
        if (objectBeingHeld)
        {
            GameObject beingHeld = interactionObject.gameObject;
            Destroy(interactionObject.gameObject);
            PlaySFXSound(inventorySound);
        }
    }

    void SetHoldPositionRotation(Transform _transform)
    {
        _transform.transform.DOLocalMove(holdPosition.transform.localPosition, 0.5f, false);
        _transform.transform.DOLocalRotate(holdPosition.transform.localRotation.eulerAngles, 0.5f);
        objectBeingHeld = true;
    }

    void PlaySFXSound(AudioClip _audioClip)
    {
        if (interactionSound != null)
        {
            audioSource.clip = _audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.Log("There is no sound identified for this object.");
        }

    }

    void SetReleasePositionRotation(Transform _transform)
    {
        _transform.position = dropMarker.transform.position;
        _transform.rotation = dropMarker.transform.rotation;
        objectBeingHeld = false;

        ClearData();
        
    }

    void ClearData()
    {
        this.interactionObject = null;
        this.holdPosition = null;
        this.dropMarker = null;
        this.linkedPrefab = null;
    }




}
