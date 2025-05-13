using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "new_splitter_list", menuName = "OSL")] //universally acessable splitter list UASL
public class Splitter_list : ScriptableObject //this object exists to be hold an ordered list of splitters.
{
    public List<configured_splitters> splitter_list = new List<configured_splitters>();



    public void AddSplitter(splitter ms)
    {
        bool hasitem = false;
        for (int i = 0; i < splitter_list.Count; i++)
        {
            if (splitter_list[i].mysplitter.prefab == ms.prefab)
            {
                splitter_list[i].AddAmount(1);
                hasitem = true;
                break;
            }
        }
        if (!hasitem)
        {
            splitter_list.Add(new configured_splitters(ms, 1));
        }
    }
}

[System.Serializable]
public class configured_splitters
{
    public splitter mysplitter;
    public int amount;

    public configured_splitters(splitter splirs, int _amount)
    {
        mysplitter = splirs;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}