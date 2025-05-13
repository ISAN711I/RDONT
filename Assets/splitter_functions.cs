using UnityEngine;

public class splitter_functions : MonoBehaviour
{
    public Splitter_list sl;
    public int curr_split;

    public void destroy_splitter()
    {
        if (sl == null || curr_split + 1 >= sl.splitter_list.Count)
        {
            Destroy(gameObject);
            return;
        }

        configured_splitters nextConfig = sl.splitter_list[curr_split + 1];
        splitter nextsplitter = nextConfig.mysplitter;

        if (nextsplitter.prefab == null)
        {
            Destroy(gameObject);
            return;
        }

        if (!nextsplitter.issingle)
        {
            for (int i = 0; i < nextsplitter.num_splits * sl.splitter_list[curr_split].amount; i++)
            {
                GameObject next = Instantiate(nextsplitter.prefab, transform.position, transform.rotation);
                splitter_functions sf = next.GetComponent<splitter_functions>();
                if (sf != null)
                {
                    sf.curr_split = curr_split + 1;
                }
            }
        }
        else
        {
            GameObject next = Instantiate(nextsplitter.prefab, transform.position, transform.rotation);
            splitter_functions sf = next.GetComponent<splitter_functions>();
            if (sf != null)
            {
                sf.curr_split = curr_split + 1;
            }
        }

        Destroy(gameObject);
    }
}
