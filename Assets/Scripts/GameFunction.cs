using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用UI

public class GameFunction : MonoBehaviour
{
    public GameObject pole1;
    public GameObject pole2;
    public GameObject pole3;
    public GameObject disk1;
    public GameObject disk2;
    public GameObject disk3;
    public GameObject disk4;
    public GameObject disk5;
    public GameObject disk6;
    public GameObject disk7;

    public static GameFunction Instance; // 設定Instance，讓其他程式能讀取GameFunction裡的東西

    public int[] pole1Value;
    public int[] pole2Value;
    public int[] pole3Value;
    public int diskCount = 7;

    public Vector3[] polePlace;

    public Vector3 poleSize;
    public Vector3 diskSize;

    private int countStep;
    public Text StepText;
    public GameObject WinText;

    public class CurrentPole
    {
        public int pole;
        public int placeAt;
    }

    public class DiskInfo
    {
        public int value;
        public int placeAt;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this; //指定Instance這個程式

        countStep = 0;

        pole1Value = new int[] {1, 2, 3, 4, 5, 6, 7};
        pole2Value = new int[7];
        pole3Value = new int[7];

        polePlace = new Vector3[] {pole1.transform.position, pole2.transform.position, pole3.transform.position};

        poleSize = pole1.GetComponent<Collider2D>().bounds.size;
        diskSize = disk1.GetComponent<Collider2D>().bounds.size;
        for(int ind = 0; ind < diskCount; ind++) {
            setPlace(1, pole1Value[ind], ind);
        }

        WinText.SetActive(false); //設定WinText不顯示
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * set each disk position by place at
     * toppest = 6, lowest = 0
     */
    public void setPlace(int pole, int disk, int placeAt)
    {
        float placeBottom = polePlace[(pole - 1)].y - poleSize.y / 2;
        float placeX = polePlace[(pole - 1)].x;
        float diskAt = diskSize.y * (diskCount - placeAt);
        Vector3 pos = new Vector3(placeX, placeBottom + diskAt, 0);
        GameObject current = GameObject.Find("disk" + disk);
        current.transform.position = pos;
    }

    /**
     * Get current pole of disk
     */
    public CurrentPole getCurrentPole(int disk)
    {
        CurrentPole cp = new CurrentPole();
        for(int i = 0; i < diskCount; i++)
        {
            if (pole1Value[i] == disk) {
                cp.pole = 1;
                cp.placeAt = i;
                return cp;
            }
            if (pole2Value[i] == disk) {
                cp.pole = 2;
                cp.placeAt = i;
                return cp;
            }
            if (pole3Value[i] == disk) {
                cp.pole = 3;
                cp.placeAt = i;
                return cp;
            }
        }
        cp.pole = 0;
        cp.placeAt = 0;
        return cp;
    }

    public bool isToppest(int disk)
    {
        int toppest1 = 0;
        int toppest2 = 0;
        int toppest3 = 0;
        for(int i = 0; i < diskCount; i++)
        {
            if (pole1Value[i] != 0 && toppest1 == 0) {
                toppest1 = pole1Value[i];
            }
            if (pole2Value[i] != 0 && toppest2 == 0) {
                toppest2 = pole2Value[i];
            }
            if (pole3Value[i] != 0 && toppest3 == 0) {
                toppest3 = pole3Value[i];
            }
        }
        if (toppest1 == disk || toppest2 == disk || toppest3 == disk) {
            return true;
        }
        return false;
    }

    public DiskInfo getToppest(int pole)
    {
        DiskInfo di = new DiskInfo();
        for(int i = 0; i < diskCount; i++)
        {
            if (pole == 1 && pole1Value[i] != 0) {
                di.value = pole1Value[i];
                di.placeAt = i;
                return di;
            }
            if (pole == 2 && pole2Value[i] != 0) {
                di.value = pole2Value[i];
                di.placeAt = i;
                return di;
            }
            if (pole == 3 && pole3Value[i] != 0) {
                di.value = pole3Value[i];
                di.placeAt = i;
                return di;
            }
        }
        di.value = 0;
        di.placeAt = 0;
        return di;
    }

    public void setEndResult(int startPlace, int startPole, int endPole, int startDisk)
    {
        //原本的位置變為0
        if (startPole == 1) {
            pole1Value[startPlace] = 0;
        }
        if (startPole == 2) {
            pole2Value[startPlace] = 0;
        }
        if (startPole == 3) {
            pole3Value[startPlace] = 0;
        }
        for(int i = diskCount - 1; i >= 0; i--) {
            //從陣列的尾巴找到0的位置替換
            if (endPole == 1) {
                if (pole1Value[i] != 0) {
                    continue;
                }
                pole1Value[i] = startDisk;
                break;
            }
            if (endPole == 2) {
                if (pole2Value[i] != 0) {
                    continue;
                }
                pole2Value[i] = startDisk;
                break;
            }
            if (endPole == 3) {
                if (pole3Value[i] != 0) {
                    continue;
                }
                pole3Value[i] = startDisk;
                break;
            }
        }
    }

    public bool isComplete()
    {
        for(int i = 0; i < diskCount; i++)
        {
            if (pole3Value[i] != (i + 1)) {
                return false;
            }
        }
        WinText.SetActive(true);
        return true;
    }

    public void addStep()
    {
        countStep++;
        StepText.text = "Step: " + countStep;
    }

}
