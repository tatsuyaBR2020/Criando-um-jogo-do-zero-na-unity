using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] ParametersNames parameters;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ExecuteAnimJump(bool value)
    {
        anim.SetBool(parameters.Jump, value);
    }

    public void ExecuteAnimMove(float hspd)
    {
        var isMoving = (hspd != 0) ? true : false;
        if (anim.GetBool(parameters.Movement) != isMoving)
            anim.SetBool(parameters.Movement, isMoving);
    }
}

[System.Serializable]
public struct ParametersNames
{
    public string Movement;
    public string Jump;
}