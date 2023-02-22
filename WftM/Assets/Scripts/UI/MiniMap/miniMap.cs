using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMap : MonoBehaviour
{
    public RectTransform parent;

    [Header("Map Elements")]
    public GameObject treePrefab;
    public GameObject boulderPrefab;
    public GameObject machinePrefab;

    [Header("Origin Point")]
    public Transform origin;

    [Header("Settings")]
    public float scale = 10;

    RectTransform rt;

    bool show = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateElements();

        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.anchoredPosition = (origin.position - Camera.main.transform.position) * scale;

        if(show)
        {
            parent.anchoredPosition -= parent.anchoredPosition / 4;
        } else
        {
            parent.anchoredPosition += (new Vector2(0, -630) - parent.anchoredPosition) / 4;
        }

        if (Input.GetKeyDown(KeyCode.M)) show = !show;
    }

    void GenerateElements()
    {
        GameObject machine = GameObject.FindGameObjectWithTag("Machine");
        SpawnMapElement(machinePrefab, machine);

        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        GameObject[] boulders = GameObject.FindGameObjectsWithTag("Boulder");

        for (int i = 0; i < boulders.Length; i++) SpawnMapElement(boulderPrefab, boulders[i]);
        for (int i = 0; i < trees.Length; i++) SpawnMapElement(treePrefab, trees[i]);
    }

    void SpawnMapElement(GameObject prefab, GameObject source)
    {
        RectTransform t = Instantiate(prefab, transform).GetComponent<RectTransform>();
        t.anchoredPosition = (source.transform.position - origin.position) * scale;

        t.transform.SendMessage("MakeLink", source);
    }
}
