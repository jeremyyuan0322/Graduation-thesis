using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator Controller_box_ANI;
    public GameObject boxANI;

    public GameObject panelB;
    public GameObject panelC;

    private bool run = true;


    public void Controller_box_open()
    {
        Controller_box_ANI.SetBool("BoxOpen",true);

        panelB.SetActive(false);
        panelC.SetActive(false);

    }
    public void Controller_box_end()
    {

        panelB.SetActive(true);
        panelC.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (run == true)
        {
            if (Controller_box_ANI.GetCurrentAnimatorStateInfo(0).IsName("over"))
            {
                Controller_box_end();
                //print("??");
                run = false;
                Destroy(boxANI);
            }
        }
    }
}
