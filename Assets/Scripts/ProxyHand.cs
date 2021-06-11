using UnityEngine;

public class ProxyHand : HandVisuals
{
    [SerializeField]
    private HandPoses fixedPose;
    //public Transform someVariable;


    public void Activate()
    {
        gameObject.SetActive(true);
        LockPose(fixedPose);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public void Start()
    {
        //if (someVariable == null)
            //Debug.LogError($"{this.ToString()} has not been assigned.", this);
        // Notice, that we pass 'this' as a context object so that Unity will highlight this object when clicked.

        Deactivate();
    }

    

}
