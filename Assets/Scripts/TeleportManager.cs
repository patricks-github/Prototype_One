using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour {

    public static TeleportManager Instance;

    [SerializeField] GameObject PlayerRig;
    [SerializeField] float FadeDuration = 1.0f;

    private Vector3 PositionToTeleportTo = Vector3.zero;



    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Destroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void TeleportToLocation(Vector3 NewPosition) {
        //Store the position to use
        PositionToTeleportTo = NewPosition;

        //Fade the screen
        SteamVR_Fade.Start(Color.clear, 0);
        SteamVR_Fade.Start(Color.black, FadeDuration);

        Invoke("TeleportPlayer", FadeDuration);
    }


    private void TeleportPlayer() {
        //Teleport to new location and clear the variable
        PlayerRig.transform.position = PositionToTeleportTo;                                                                        
        PositionToTeleportTo = Vector3.zero;

        SteamVR_Fade.Start(Color.clear, FadeDuration);
    }

}



//class TeleTest
//{
//    private TeleportMarkerBase[] teleportMarkers;
//    private TeleportMarkerBase pointedAtTeleportMarker;
//    private TeleportMarkerBase teleportingToMarker;
//    private Vector3 pointedAtPosition;
//    private Vector3 prevPointedAtPosition;
//    private bool teleporting = false;
//    public float teleportFadeTime = 0.1f;
//    public AudioClip teleportSound;
//    public AudioSource headAudioSource;
//    private float currentFadeTime = 0.0f;

//    /*
//    Classes to look at:
//    TeleportMarkerBase
//    TeleportPoint
//    */

//    //-------------------------------------------------IMPORTANT
//    private void TryTeleportPlayer()
//    {
//        if (!teleporting)
//        {
//            if (pointedAtTeleportMarker != null && pointedAtTeleportMarker.locked == false)
//            {
//                //Pointing at an unlocked teleport marker
//                teleportingToMarker = pointedAtTeleportMarker;
//                InitiateTeleportFade();

//                //CancelTeleportHint();
//            }
//        }
//    }


//    //-------------------------------------------------IMPORTANT
//    private void InitiateTeleportFade()
//    {
//        teleporting = true;

//        currentFadeTime = teleportFadeTime;

//        TeleportPoint teleportPoint = teleportingToMarker as TeleportPoint;
//        if (teleportPoint != null && teleportPoint.teleportType == TeleportPoint.TeleportPointType.SwitchToNewScene)
//        {
//            currentFadeTime *= 3.0f;
//            Teleport.ChangeScene.Send(currentFadeTime);
//        }

//        SteamVR_Fade.Start(Color.clear, 0);
//        SteamVR_Fade.Start(Color.black, currentFadeTime);

//        headAudioSource.transform.SetParent(player.hmdTransform);
//        headAudioSource.transform.localPosition = Vector3.zero;
//        PlayAudioClip(headAudioSource, teleportSound);

//        Invoke("TeleportPlayer", currentFadeTime);
//    }
//    (edited)
//    //-------------------------------------------------IMPORTANT
//    private void TeleportPlayer()
//    {
//        teleporting = false;

//        Teleport.PlayerPre.Send(pointedAtTeleportMarker);

//        SteamVR_Fade.Start(Color.clear, currentFadeTime);

//        TeleportPoint teleportPoint = teleportingToMarker as TeleportPoint;
//        Vector3 teleportPosition = pointedAtPosition;

//        if (teleportPoint != null)
//        {
//            teleportPosition = teleportPoint.transform.position;

//            //Teleport to a new scene
//            if (teleportPoint.teleportType == TeleportPoint.TeleportPointType.SwitchToNewScene)
//            {
//                teleportPoint.TeleportToScene();
//                return;
//            }
//        }
//        /*
//        // Find the actual floor position below the navigation mesh
//        TeleportArea teleportArea = teleportingToMarker as TeleportArea;
//        if (teleportArea != null)
//        {
//            if (floorFixupMaximumTraceDistance > 0.0f)
//            {
//                RaycastHit raycastHit;
//                if (Physics.Raycast(teleportPosition + 0.05f * Vector3.down, Vector3.down, out raycastHit, floorFixupMaximumTraceDistance, floorFixupTraceLayerMask))
//                {
//                    teleportPosition = raycastHit.point;
//                }
//            }
//        }
//        */
//        if (teleportingToMarker.ShouldMovePlayer())
//        {
//            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
//            player.trackingOriginTransform.position = teleportPosition + playerFeetOffset;
//        }
//        else
//        {
//            teleportingToMarker.TeleportPlayer(pointedAtPosition);
//        }

//        Teleport.Player.Send(pointedAtTeleportMarker);
//    }

//    private void PlayAudioClip(AudioSource source, AudioClip clip)
//    {
//        source.clip = clip;
//        source.Play();
//    }
//}