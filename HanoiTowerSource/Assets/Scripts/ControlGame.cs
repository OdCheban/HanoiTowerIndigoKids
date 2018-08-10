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

    public float speed;

    public void StartGame(int k)
    {
        kDisk = k;
        CreateColumns();
        CreateDisks();
        SetPosCamera();
        SetTransformPlane();

        StartCoroutine(SolutTask());
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
            diskGO[i].name = "Disk" + i.ToString();
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

    IEnumerator SolutTask()
    {
        int countDisk = diskGO.Count;
        while (columnGO[2].transform.childCount != countDisk)
        {
            if (countDisk % 2 == 0)
            {
                yield return StartCoroutine(MoveFull(columnGO[0].transform, columnGO[1].transform));
                if (columnGO[2].transform.childCount == countDisk) break;
                yield return StartCoroutine(MoveFull(columnGO[0].transform, columnGO[2].transform));
            }
            else
            {
                yield return StartCoroutine(MoveFull(columnGO[0].transform, columnGO[2].transform));
                if (columnGO[2].transform.childCount == countDisk) break;
                yield return StartCoroutine(MoveFull(columnGO[0].transform, columnGO[1].transform));
            }
            yield return StartCoroutine(MoveFull(columnGO[1].transform, columnGO[2].transform));
        }
        Debug.LogError("решено!");
        yield return null;
    }

    IEnumerator MoveFull(Transform column1,Transform column2)
    {
        Transform diskColumn1 = GetLastDiskColumn(column1);
        Transform diskColumn2 = GetLastDiskColumn(column2);
        if (diskColumn1 && (!diskColumn2 || diskColumn1.localScale.x < diskColumn2.localScale.x))
        {
            yield return StartCoroutine(MoveTo(diskColumn1, new Vector3(column1.position.x,column1.position.y * 2, 0)));
            yield return StartCoroutine(MoveTo(diskColumn1, new Vector3(column2.position.x,column2.position.y * 2, 0)));
            Vector3 endPos = diskColumn2 ? diskColumn2.position + new Vector3(0, scaleDiskY, 0) : new Vector3(column2.position.x, scaleDiskY, 0);
            yield return StartCoroutine(MoveTo(diskColumn1, endPos));
            diskColumn1.SetParent(column2);
        }
        else
        {
            yield return StartCoroutine(MoveTo(diskColumn2, new Vector3(column2.position.x, column2.position.y * 2, 0)));
            yield return StartCoroutine(MoveTo(diskColumn2, new Vector3(column1.position.x, column1.position.y * 2, 0)));
            Vector3 endPos = diskColumn1 ? diskColumn1.position + new Vector3(0, scaleDiskY, 0) : new Vector3(column1.position.x, scaleDiskY, 0);
            yield return StartCoroutine(MoveTo(diskColumn2, endPos));
            diskColumn2.SetParent(column1);
        }
    }


    IEnumerator MoveTo(Transform disk,Vector3 target)
    {
        while ((target - disk.position).magnitude > 0.001f)
        {
            disk.position = Vector3.MoveTowards(disk.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    Transform GetLastDiskColumn(Transform column)
    {
        if (column.childCount != 0)
            return column.GetChild(column.childCount - 1);
        else
            return null;
    }


}
