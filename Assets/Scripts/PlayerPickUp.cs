using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerPickUp : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    public Transform PickUp;

    public bool isHoldingOBJ;
    public bool isPickingUpOBJ;

    float pickUpTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pickUpTimer -= Time.deltaTime;
        //if the player holds q
        if (Input.GetKeyDown(KeyCode.Q) && pickUpTimer <= 0)
        {
            if (PickUp == null)
            {
                isPickingUpOBJ = true;
            }
            else
            {
                PickUp.GetComponent<PickUpAbles>().DropPickUp();
                PickUp = null;
                isHoldingOBJ = false;
            }
            pickUpTimer = 1;
        }
        else
        {
            isPickingUpOBJ = false;
        }
   
    }
    //you'll never guess what this func does 
    public void DestroyPickUp()
    {
        if (PickUp != null)
            Destroy(PickUp);
    }
    //or this one
    public void SetPickUpOBJ(Transform OBJ)
    {
        PickUp = OBJ;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            //data that gets sent to other players
            stream.SendNext(PickUp);
            //stream.SendNext(PlayerThatPickUpOBJ);

        }
        else
        {
            //data recieved from other players
            PickUp = (Transform)stream.ReceiveNext();
            //PlayerThatPickUpOBJ.transform.position = (Vector3)stream.ReceiveNext();


        }

    }
}
