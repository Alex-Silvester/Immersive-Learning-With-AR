using UnityEngine;

public class HeightAdjustor : MonoBehaviour
{
    public void Up(float amount)
    {
        gameObject.transform.position += new Vector3(0,amount,0);
    }

    public void Down(float amount) 
    { 
        gameObject.transform.position -= new Vector3(0,amount,0); 
    }
}
