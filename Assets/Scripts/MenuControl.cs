using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public GameObject mainMenuObj;
    public GameObject cfgMenuObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    goMenu(string menu){
        if(menu == "cfg"){
            mainMenuObj.setActive(false);
            cfgMenuObj.setActive(true);
        }else{
            cfgMenuObj.setActive(false);
            mainMenuObj.setActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
