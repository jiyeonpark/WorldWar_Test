using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSwingList : MonoBehaviour 
{
    List<Vector3> _listPosition = new List<Vector3>();

    void FixedUpdate()
    {
        SaveCurrentSpeed();
    }

    void SaveCurrentSpeed()
    {
        if (_listPosition.Count >= 20)
        {
            _listPosition.RemoveAt(0);
            _listPosition.Add(transform.position);
        }
        else
        {
            _listPosition.Add(transform.position);
        }
    }

    public List<Vector3> GetSwingList()
    {
        return _listPosition;
    }
}
