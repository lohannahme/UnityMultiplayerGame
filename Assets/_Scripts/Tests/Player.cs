using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror {
    public class Player : NetworkBehaviour
    {

        [SerializeField]
        private Vector3 movement = new Vector3();
        
        [Client]
        void Update()
        {
            if (!hasAuthority) { return; }
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.Translate(movement);
            }

        }
    }
}
