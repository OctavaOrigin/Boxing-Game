using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCreation : MonoBehaviour
{
    [SerializeField] GameObject[] ropes;
    [SerializeField] GameObject[] coloms;
    [SerializeField] GameObject floor;

    Vector2 minCorner, maxCorner;
    float colomSize;

    void Awake(){
        colomSize = coloms[0].GetComponent<BoxCollider>().bounds.extents.x;
    }

    public void CreateRing(){
        FindRingMinMaxCorner();
        HightAndSizeOfRopes();


        float colomHight = coloms[0].GetComponent<BoxCollider>().bounds.extents.y;
        float ringHight = floor.GetComponent<BoxCollider>().bounds.extents.y;
        float colYPos = ringHight + colomHight;
        coloms[0].transform.position = new Vector3(minCorner.x, transform.position.y + colYPos, minCorner.y);
        coloms[1].transform.position = new Vector3(maxCorner.x, transform.position.y + colYPos, minCorner.y);
        coloms[2].transform.position = new Vector3(minCorner.x, transform.position.y + colYPos, maxCorner.y);
        coloms[3].transform.position = new Vector3(maxCorner.x, transform.position.y + colYPos, maxCorner.y);

        float colomStep = colomHight * 2 / 3;
        ropes[4].transform.position = new Vector3(minCorner.x + (maxCorner.x - minCorner.x)/2, transform.position.y + ringHight + colomStep, maxCorner.y);
        ropes[5].transform.position = new Vector3(minCorner.x + (maxCorner.x - minCorner.x)/2, transform.position.y + ringHight + colomStep*2, maxCorner.y);
        ropes[6].transform.position = new Vector3(minCorner.x + (maxCorner.x - minCorner.x)/2, transform.position.y + ringHight + colomStep, minCorner.y);
        ropes[7].transform.position = new Vector3(minCorner.x + (maxCorner.x - minCorner.x)/2, transform.position.y + ringHight + colomStep*2, minCorner.y);
        ropes[0].transform.position = new Vector3(minCorner.x, transform.position.y + ringHight + colomStep, minCorner.y + (maxCorner.y - minCorner.y)/2);
        ropes[1].transform.position = new Vector3(minCorner.x, transform.position.y + ringHight + colomStep*2, minCorner.y + (maxCorner.y - minCorner.y)/2);
        ropes[2].transform.position = new Vector3(maxCorner.x, transform.position.y + ringHight + colomStep, minCorner.y + (maxCorner.y - minCorner.y)/2);
        ropes[3].transform.position = new Vector3(maxCorner.x, transform.position.y + ringHight + colomStep*2, minCorner.y + (maxCorner.y - minCorner.y)/2);

        for(int i = 0; i < 4; i++){
            ropes[i].transform.localEulerAngles = new Vector3(0, 90, 90);
        }
    }

    private void FindRingMinMaxCorner(){
        Bounds bounds = floor.GetComponent<BoxCollider>().bounds;
        minCorner = new Vector2(bounds.min.x + colomSize, bounds.min.z + colomSize);
        maxCorner = new Vector2(bounds.max.x - colomSize, bounds.max.z - colomSize);
    }

    private void HightAndSizeOfRopes(){
        float ropeSize = (maxCorner.x - minCorner.x)/2;
        foreach(GameObject obj in ropes){
            obj.transform.localScale = new Vector3(obj.transform.localScale.x, ropeSize, obj.transform.localScale.z);
        }
    }

    public Bounds GetRingSize(){
        Bounds floorBounds = floor.GetComponent<BoxCollider>().bounds;
        Bounds bounds = new Bounds(floorBounds.center, floorBounds.extents * 2 - new Vector3(colomSize * 2, 0, colomSize * 2));
        return bounds;
    }
}
