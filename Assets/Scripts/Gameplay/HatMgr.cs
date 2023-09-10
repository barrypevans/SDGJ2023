using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    enum HatType
    {
        kWatermelon,
        kBow,
        kFrog,
        kNone
    }
    
    class HatMgr : MonoBehaviour
    {
        public List<GameObject> hatObjs;
        public void SetHat(HatType hatType )
        {
            DisableAll();
            if(HatType.kNone != hatType)
                hatObjs[(int)hatType].SetActive(true);
        }

        private void DisableAll()
        {
            foreach(var obj in hatObjs)
            {
                obj.SetActive(false);
            }
        }
    }
}
