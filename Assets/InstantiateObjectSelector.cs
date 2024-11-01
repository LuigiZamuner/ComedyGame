using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class InstantiateObjectSelector : MonoBehaviour

{
    public GameObject horizontalObjectPrefab;
    public GameObject verticalObjectPrefab;
    public ARRaycastManager raycastManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        Vector2 touchPosition = Vector2.zero;
        bool isTouch = false;

        // Verifica se é um toque em tela (Novo Input System) ou um clique com o mouse (Editor)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isTouch = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            touchPosition = Mouse.current.position.ReadValue();
            isTouch = true;
        }

        if (isTouch)
        {
            // Faz o Raycast
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinBounds))
            {
                ARRaycastHit hit = hits[0];
                Pose hitPose = hit.pose;
                ARPlane plane = hit.trackable as ARPlane;

                if (plane != null)
                {
                    Debug.Log("Clique ou toque detectado em um ARPlane!");

                    if (plane.alignment == PlaneAlignment.HorizontalUp || plane.alignment == PlaneAlignment.HorizontalDown)
                    {
                        Debug.Log("Plano Horizontal Detectado!");
                        Instantiate(horizontalObjectPrefab, hitPose.position, hitPose.rotation);
                    }
                    else if (plane.alignment == PlaneAlignment.Vertical)
                    {
                        Debug.Log("Plano Vertical Detectado!");
                        Instantiate(verticalObjectPrefab, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        Debug.Log("Outro tipo de plano detectado!");
                    }
                }
            }
            else
            {
                Debug.Log("Nenhum plano detectado no toque/clique.");
            }
        }
    }
}