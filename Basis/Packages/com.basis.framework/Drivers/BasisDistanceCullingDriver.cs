using Basis.Scripts.BasisSdk;
using Basis.Scripts.BasisSdk.Players;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Basis.Scripts.Drivers
{
    public class BasisDistanceCullingDriver : MonoBehaviour
    {
        private BasisAvatar Avatar;
        public float Dist;
        public void Initialize(BasisAvatar avatar)
        {
            Avatar = avatar;
            InvokeRepeating("DistanceCull", 0f, 0.3f);
        }
        private void DistanceCull()
        {
            if(Avatar!=null){
                Dist = (Vector3.Distance(BasisLocalPlayer.Instance.transform.GetChild(0).transform.position, this.transform.GetChild(0).transform.position));
                Avatar.transform.gameObject.SetActive((Dist<10));
            }
        }
    }
}
