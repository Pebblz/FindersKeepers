using Photon.Pun;
namespace com.pebblz.finderskeepers
{
    public class DisableButtonForNonMaster : MonoBehaviourPunCallbacks
    {

        void Update()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
