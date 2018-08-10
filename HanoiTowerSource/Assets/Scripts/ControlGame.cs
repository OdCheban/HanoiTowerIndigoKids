using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGame : MonoBehaviour {
    public Transform cameraObj;
    public Transform plane;
    public int kDisk;
    private static int kColumn = 3;
    private GameObject[] columnGO = new GameObject[kColumn];
    private List<GameObject> diskGO = new List<GameObject>();

    public GameObject columnPrefab;
    public GameObject diskPrefab;

    public float scaleDiskY = 0.2f;
    public float scaleDiskX = 0.4f;

    public void StartGame(int k)
    {
        kDisk = k;
        CreateColumns();
        CreateDisks();
        SetPosCamera();
        SetTransformPlane();
    }
    void CreateColumns()
    {
        float sizeColumnY = (scaleDiskY * kDisk + scaleDiskY) * 2;
        for (int i = 0; i < kColumn;i++)
        {
            columnGO[i] = Instantiate(columnPrefab);
            columnGO[i].transform.localScale = new Vector3(1, sizeColumnY, 1);
            columnGO[i].transform.position = new Vector3(kDisk*i, sizeColumnY, 0);
        }
    }
    void CreateDisks()
    {
        for(int i = 0; i<kDisk; i++)
        {
            diskGO.Add(Instantiate(diskPrefab));
            float sizeXZ = (1 + scaleDiskX) + scaleDiskX * (kDisk - i);
            diskGO[i].transform.localScale = new Vector3(sizeXZ, scaleDiskY, sizeXZ);
            diskGO[i].transform.position = new Vector3(0, scaleDiskY + scaleDiskY*2*i , 0);
            diskGO[i].GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            diskGO[i].transform.SetParent(columnGO[0].transform);
        }
    }
    void SetPosCamera()
    {
        cameraObj.position = new Vector3(columnGO[1].transform.position.x, columnGO[1].transform.position.y*4, -1*kDisk*2);
    }
    void SetTransformPlane()
    {
        plane.transform.position = new Vector3(kDisk, 0, kDisk);
        plane.transform.localScale = new Vector3(kDisk, 1, kDisk);
    }


    public void StopGame()
    {
        foreach (GameObject column in columnGO)
        {
            Destroy(column);
        }
        diskGO.Clear();
    }
}
