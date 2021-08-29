using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{    
    /*
    public bool IsAvailable { get; set; }
    public bool IsDirty { get; set; }

    [SerializeField]
    private float timeToClean = 2f;
    public float timeFromClean;

    List<GameObject> clickRecord;

    private void Start()
    {
        clickRecord = GameManager.Instance.clickRecord;
    }

    private void Update()
    {
        IsAvailable = (transform.childCount != 0) ? false : true;

        foreach(Transform tr in transform)
        {
            if (tr.GetComponent<DirtySheets>() != null)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (IsDirty)
        {
            if (clickRecord[clickRecord.Count - 2] != null && clickRecord[clickRecord.Count - 2].name == "CLEAN_SHEETS_CABINET")
            {
                if (clickRecord[clickRecord.Count - 1]!= null && clickRecord[clickRecord.Count - 1] == gameObject)
                {
                    if (Vector3.Distance(PlayerLocation.Instance.transform.position, transform.position) <= 1.5)
                    {
                        CleanSheets();
                    }
                }
            }
        }
        else
        {
            timeFromClean = 0f;
        }
    }

    private void CleanSheets()
    {
        timeFromClean += Time.deltaTime;

        if (Mathf.Clamp(timeFromClean, 0f, timeToClean) == timeToClean)
        {
            timeFromClean = 0f;

            GameObject dirtySheets;
            if(Utilities.HasChildWithComponent<DirtySheets>(gameObject, out dirtySheets))
            {
                dirtySheets.transform.SetParent(PlayerLocation.Instance.transform);
            }            
        }
    }

    */
}
