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
        ObjectPool.GetInstance().GetEntityPlayer().SetActive(true);
        ChangeCamera(CameraType.ThirdPerson);
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

    public void DoCinematicPath(Vector3 _startPos, Transform[] _wayPoints, float _moveSpeed, Ease easing = Ease.Linear, Transform _lookAtPos = null, System.Action onComplete = null, System.Action<int> onWayPointChange = null)
    {
        ChangeCamera(CameraType.Cinematic);

        List<Vector3> wayPoints = new List<Vector3>();

        foreach (Transform point in _wayPoints)
        {
            wayPoints.Add(point.position);
        }

        go_currentCamera.transform.DOPath(wayPoints.ToArray(), _moveSpeed, PathType.CatmullRom, PathMode.Full3D).SetSpeedBased(true).SetLookAt(_lookAtPos)
            .OnComplete(() => { if (onComplete != null) onComplete.Invoke(); })
            .OnWaypointChange((index) => { if (onWayPointChange != null) onWayPointChange.Invoke(index); });
    }

}
