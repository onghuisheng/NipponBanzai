using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraHandler : Singleton<CameraHandler>
{

    public enum CameraType
    {
        MainMenu = 0,
        ThirdPerson,
        Cinematic,
    }

    [SerializeField]
    private List<GameObject> list_cameras_prefab;

    private List<GameObject> list_cameras = new List<GameObject>();

    private CameraType
        enum_currentCamType;

    private GameObject
        go_currentCamera;

    protected override void Awake()
    {
        base.Awake();

        foreach (GameObject go in list_cameras_prefab)
        {
            GameObject instantiated = Instantiate(go);
            instantiated.SetActive(false);
            list_cameras.Add(instantiated);
        }

    }

    private void Start()
    {
        // Sets up the camera type on start up
        switch (SceneHandler.GetInstance().GetCurrentSceneType())
        {
            case SceneHandler.SceneType.MainMenu:
                // TODO: change this back to main menu camera when not testing
                ChangeCamera(CameraType.MainMenu);
                //ObjectPool.GetInstance().GetEntityPlayer().SetActive(true);
                //ChangeCamera(CameraType.ThirdPerson);
                break;
            case SceneHandler.SceneType.Level1:
            case SceneHandler.SceneType.Level2:
            case SceneHandler.SceneType.JJPlayground:
                GameObject spawnPoint = GameObject.FindWithTag("Respawn");
                GameObject player = ObjectPool.GetInstance().GetEntityPlayer();
                player.SetActive(true);

                if (spawnPoint != null)
                    player.transform.position = spawnPoint.transform.position;

                ChangeCamera(CameraType.ThirdPerson);
                break;
            default:
                Debug.LogError("Unknown sceneType");
                break;
        }
    }

    protected override void Update()
    {
        base.Update();        
    }

    public void ChangeCamera(CameraType _type)
    {
        foreach (GameObject go in list_cameras)
        {
            go.SetActive(false);
        }
        go_currentCamera = list_cameras[(int)_type];
        go_currentCamera.SetActive(true);
        enum_currentCamType = _type;
    }

    public GameObject GetCurrentCamera()
    {
        return go_currentCamera;
    }

    public CameraType GetCameraType()
    {
        return enum_currentCamType;
    }

    public void DoCinematicPath(CinematicPathing paths, System.Action onComplete = null, System.Action<int> onWayPointChange = null, System.Action onUpdate = null)
    {
        var wayPointNodes = paths.GetPathNodes();

        // Abort if paths are empty or null
        if (paths == null || wayPointNodes.Count == 0)
        {
            Debug.LogError("Cinematic Paths cannot be null or empty!");
            return;
        }

        // Start the cinematic camera position at the previous camera's position
        GameObject cinematicCamera = list_cameras[(int)CameraType.Cinematic];
        cinematicCamera.transform.position = GetCurrentCamera().transform.position;
        cinematicCamera.transform.rotation = GetCurrentCamera().transform.rotation;

        List<Vector3> wayPoints = paths.GetPathPositions();

        Transform initialTransform = GetCurrentCamera().transform;

        if (paths.loopbackToInitial)
            wayPoints.Add(initialTransform.position);

        // Toggle cinematic camera before starting animation
        ChangeCamera(CameraType.Cinematic);

        CinematicPathNode firstNode = wayPointNodes[0];

        var tweener = go_currentCamera.transform.DOPath(wayPoints.ToArray(), paths.moveSpeed, paths.pathType, PathMode.Full3D).SetEase(paths.easeType).SetSpeedBased(true);

        tweener
        .OnUpdate(() =>
        {
            if (onUpdate != null)
                onUpdate.Invoke();

            if (paths.lookAtTarget != null)
                go_currentCamera.transform.DOLookAt(paths.lookAtTarget.position, 0.1f);
        })
        .OnComplete(() =>
        {
            if (paths.loopbackToInitial)
            {
                // Wait for the current camera to merge with the initial camera before invoking complete
                GetCurrentCamera().transform.DORotate(initialTransform.rotation.eulerAngles, 1).OnComplete(() =>
                {
                    if (onComplete != null)
                        onComplete.Invoke();
                });
            }
            else
            {
                if (onComplete != null)
                    onComplete.Invoke();
            }
        })
        .OnWaypointChange((index) =>
        {
            if (onWayPointChange != null)
                onWayPointChange.Invoke(index);


        });

    }

}
