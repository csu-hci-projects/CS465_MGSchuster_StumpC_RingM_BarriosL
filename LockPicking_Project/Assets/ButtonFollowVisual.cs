using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    public Transform visualTarget;
    public Vector3 localAxis;

    private Vector3 offset;
    private Transform pokeAttachTransform;

    private XRBaseInteractable interactable;
    private bool isFollowing = false;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            isFollowing = true;
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
        }
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (isFollowing)
    //     {
    //         Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
    //         Vector3 contrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

    //         visualTarget.position = visualTarget.TransformPoint(contrainedLocalTargetPosition);
    //     }

    // }

    // Define limits and AudioSource
    public float minLimit = 0f;
    public float maxLimit = 1f;
    public AudioSource clickSound;

    //start at x=0, y=0, z =-0.0858
    //end at x=0, y=0, z =0.029

    void Update()
    {
        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 contrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

            // Clamp the position along the axis
            float constrainedValue = Mathf.Clamp(Vector3.Dot(contrainedLocalTargetPosition, localAxis.normalized), minLimit, maxLimit);
            contrainedLocalTargetPosition = localAxis.normalized * constrainedValue;

            // Apply the constrained position
            visualTarget.position = visualTarget.TransformPoint(contrainedLocalTargetPosition);

            // Play click sound if the limit is reached
            if (Mathf.Approximately(constrainedValue, minLimit) || Mathf.Approximately(constrainedValue, maxLimit))
            {
                if (!clickSound.isPlaying)
                {
                    clickSound.Play();
                }
            }
        }
    }

}