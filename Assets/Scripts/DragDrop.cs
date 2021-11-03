using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private Vector3 screenPoint;
    private int startPole;
    private int startPlace;
    private int currentDisk;
    private int endPole;
    

    private bool isCorrectPlace = false;

    private bool isWin = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        string diskIs = this.gameObject.name.Substring(4);
        currentDisk = int.Parse(diskIs);
        if (!GameFunction.Instance.isToppest(currentDisk) || isWin) {
            return;
        }
        GameFunction.CurrentPole currentPole = GameFunction.Instance.getCurrentPole(currentDisk);
        startPole = currentPole.pole;
        startPlace = currentPole.placeAt;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }

    //must have a collider attach it
    void OnMouseDrag()
    {
        if (!GameFunction.Instance.isToppest(currentDisk) || isWin) {
            return;
        }
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        this.transform.position = curPosition;
    }

    private void OnMouseUp() {
        if (!GameFunction.Instance.isToppest(currentDisk) || isWin) {
            return;
        }
        if (isCorrectPlace) {
            GameFunction.Instance.setEndResult(startPlace, startPole, endPole, currentDisk);
            int endPlace = GameFunction.Instance.getToppest(endPole).placeAt;
            GameFunction.Instance.setPlace(endPole, currentDisk, endPlace);
            //追加步數
            GameFunction.Instance.addStep();
            // 檢查是否完成
            isWin = GameFunction.Instance.isComplete();
        } else {
            GameFunction.Instance.setPlace(startPole, currentDisk, startPlace);
        }
        isCorrectPlace = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string poleName = other.gameObject.name;
        string poleIs = poleName.Substring(4);
        isCorrectPlace = false;
        if (int.Parse(poleIs) == startPole) {
            return;
        }
        if (!poleName.StartsWith("pole")) {
            return;
        }
        int getToppestDisk = GameFunction.Instance.getToppest(int.Parse(poleIs)).value;
        if (getToppestDisk < currentDisk && getToppestDisk != 0) {
            // 判斷新的pole最上層的disk是不是比自己小?否則不能放上面
            return;
        }
        isCorrectPlace = true;
        endPole = int.Parse(poleIs);
    }
}
