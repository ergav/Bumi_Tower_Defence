using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public static GameManager               instance;

    [HideInInspector] public bool           prepareState = true;
    [HideInInspector] public bool           placingState;

    [SerializeField] private GameObject     turretToPlace;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Update()
    {
        if (placingState)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);

                if(IsMouseOverUI())
                    return;

                Instantiate(turretToPlace, worldPoint2d, Quaternion.identity);
            }
        }
    }

    public void StartGame()
    {
        prepareState = false;
        placingState = false;
    }

    public void SetPlaceTurret(bool value)
    {
        placingState = value;
    }

    public void SetTurretPrefab(GameObject prefab)
    {
        turretToPlace = prefab;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}