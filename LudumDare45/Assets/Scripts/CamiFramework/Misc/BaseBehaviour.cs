using UnityEngine;
using System.Collections;

public class BaseBehaviour : MonoBehaviour
{
    public void Start()
    {
        BaseInit();
    }


    public void BaseInit()
    {
        if (initialised)
            return;

        Init();
        initialised = true;
    }

    public virtual void Init()
    {
    }

    public GameObject AddChild(string name)
    {
        var child = new GameObject(name);
        child.transform.SetParent(transform);
        child.transform.localPosition = Vector3.zero;
        return child;
    }

    public T AddChild<T>(GameObject prefab, bool maintainWorldPosition = false)
    {
        return AddChild(prefab, maintainWorldPosition).GetComponent<T>();
    }
    
    public GameObject AddChild(GameObject prefab, bool maintainWorldPosition = false)
    {
        GameObject gameObject = Instantiate(prefab) as GameObject;
        gameObject.transform.SetParent(transform, maintainWorldPosition);
        return gameObject;
    }
    
    public void RemoveChild(GameObject child)
    {
        GameObject.Destroy(child);
    }

    public T AddSibling<T>(GameObject prefab)
    {
        return AddSibling(prefab).GetComponent<T>();
    }

    public GameObject AddSibling(GameObject prefab)
    {
        GameObject gameObject = Instantiate(prefab) as GameObject;
        gameObject.transform.parent = transform.parent;
        return gameObject;
    }
    
    public Transform FindChild(System.Predicate<Transform> predicate)
    {
        foreach(Transform child in transform)
        {
            if (predicate(child))
                return child;
        }
        return null;
    }
    
    public void DestroyAllChildren()
    {
        DestroyAllChildren(transform);
    }

    public static void DestroyAllChildren(Transform transform)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
#if UNITY_EDITOR
        GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        --i;
#else
        GameObject.Destroy(transform.GetChild(i).gameObject);
#endif
        }
    }

    public static void DestroySafe(Object objectToDestroy)
    {
#if UNITY_EDITOR
        GameObject.DestroyImmediate(objectToDestroy);
#else
        GameObject.Destroy(objectToDestroy);
#endif
    }

    #region Properties
    public Vector3 LocalPosition
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }
    
    public float LocalX
    {
        get { return transform.localPosition.x; }
        set { transform.localPosition = new Vector3(value, LocalY, LocalZ); }
    }
    public float LocalY
    {
        get { return transform.localPosition.y; }
        set { transform.localPosition = new Vector3(LocalX, value, LocalZ); }
    }

    public float LocalZ
    {
        get { return transform.localPosition.z; }
        set { transform.localPosition = new Vector3(LocalX, LocalY, value); }
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector2 Position2
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float PosX
    {
        get { return transform.position.x; }
        set { transform.position = new Vector3(value, PosY, PosZ); }
    }

    public float PosY
    {
        get { return transform.position.y; }
        set { transform.position = new Vector3(PosX, value, PosZ); }
    }

    public float PosZ
    {
        get { return transform.position.z; }
        set { transform.position = new Vector3(PosX, PosY, value); }
    }

    public float Rotation
    {
        get { return transform.rotation.eulerAngles.z; }
        set { transform.rotation = Quaternion.Euler(0, 0, value); }
    }

    public float LocalRotation
    {
        get { return transform.localRotation.eulerAngles.z; }
        set { transform.localRotation = Quaternion.Euler(0, 0, value); }
    }

    public GameObject Parent
    {
        get { return transform.parent.gameObject; }
    }
    #endregion Properties

    #region Fields
    private bool initialised;
    #endregion Fields
}
