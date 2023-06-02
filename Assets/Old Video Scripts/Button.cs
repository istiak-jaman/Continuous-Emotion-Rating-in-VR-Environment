using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Button : MonoBehaviour
{
    [SerializeField] private VideoPlayer vidP;
    [SerializeField] private VideoClip clip2;
    [SerializeField] private VideoClip clip3;
    public bool distance = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeYes()
    {
       
        vidP.clip = clip2;
        vidP.Play();
        distance = true;
        
        

    }
    public void ChangeNo()
    {
        vidP.clip = clip3;
        vidP.Play();
        distance = true;

    }

}
