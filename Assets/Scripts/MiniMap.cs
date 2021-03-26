using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.pebblz.finderskeepers
{
    public class MiniMap : MonoBehaviour
    {
        GameObject Player;
        [SerializeField] GameObject Diamond;
        List<GameObject> ActiveDiamondList = new List<GameObject>();
        List<GameObject> todoobj = new List<GameObject>();
        float timer = 32;
        // Update is called once per frame
        void LateUpdate()
        {
            //if (timer <= 0)
            //{
                if (todoobj.Count < 3)
                {
                    todoobj.AddRange(GameObject.FindGameObjectsWithTag("PointsPickUp"));
                }
                if (Player == null)
                {
                    Player = GameObject.FindGameObjectWithTag("Player");
                }

                transform.position = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);

            //    if (ActiveDiamondList.Count < 3)
            //    {
            //        GameObject temp = Instantiate(Diamond,new Vector3(0,9,0),Quaternion.identity);
            //        ActiveDiamondList.Add(temp);
            //        SetDiamondPos();
            //    }
            //}
            //timer -= Time.deltaTime;
        }
        //void SetDiamondPos()
        //{
        //    for (int i = 0; i < ActiveDiamondList.Count; i++)
        //    {
        //        GameObject tempobj = ActiveDiamondList[i].gameObject;
        //        tempobj.transform.position = new Vector3(todoobj[i].transform.position.x, 8, todoobj[i].transform.position.z);
        //        tempobj.transform.localRotation = new Quaternion(90, 0, 0, 0);
        //        tempobj.transform.SetParent(todoobj[i].transform);
        //    }
        //}
    }
}
