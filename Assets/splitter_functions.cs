using UnityEngine;

public class splitter_functions : MonoBehaviour
{
    public Splitter_list sl;
    public int curr_split;

    public void destroy_splitter()
    {
        if (sl == null || curr_split + 1 >= sl.splitter_list.Count)
        {
            Debug.LogWarning("No further splitter. Destroying.");
            Destroy(gameObject);
            return;
        }

        configured_splitters nextConfig = sl.splitter_list[curr_split + 1];
        splitter nextsplitter = nextConfig.mysplitter;

        if (nextsplitter.prefab == null)
        {
            Debug.LogWarning("Next splitter or its prefab is null. Destroying.");
            Destroy(gameObject);
            return;
        }

        int spawnCount = nextsplitter.issingle ? 1 : nextsplitter.num_splits * sl.splitter_list[curr_split].amount;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject next = Instantiate(nextsplitter.prefab, transform.position, transform.rotation);
            splitter_functions sf = next.GetComponent<splitter_functions>();
            if (sf != null)
            {
                sf.curr_split = curr_split + 1;
                sf.sl = sl; // Pass reference
            }
        }

        Destroy(gameObject);
    }
}