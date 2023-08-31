using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;

public class SpeedControl : MonoBehaviour
{

    public float walkSpeed;
    public float walkPosBias;
    public float walkDirBias;

    public float runSpeed;
    public float runPosBias;
    public float runDirBias;

    public float sprintSpeed;
    public float sprintPosBias;
    public float sprintDirBias;

    public float crouchSpeed;
    public float crouchPosBias;
    public float crouchDirBias;

    public float strafeSpeed;
    public float strafePosBias;
    public float strafeDirBias;
    
    private MxMAnimator mmAnimator;
    private MxMTrajectoryGenerator mmTrajGen;

    // Start is called before the first frame update
    void Start()
    {
        mmAnimator = gameObject.GetComponent<MxMAnimator>();
        mmTrajGen = gameObject.GetComponent<MxMTrajectoryGenerator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Strafe"))
        {
            mmTrajGen.MaxSpeed = strafeSpeed;
            mmTrajGen.PositionBias = strafePosBias;
            mmTrajGen.DirectionBias =  strafeDirBias;
            // mmAnimator.SetCalibrationData("Strafe");
        }

        else if (Input.GetButton("Sprint"))
        {
            mmTrajGen.MaxSpeed = sprintSpeed;
            mmTrajGen.PositionBias = sprintPosBias;
            mmTrajGen.DirectionBias = sprintDirBias;
            // mmAnimator.SetCalibrationData("Sprint");
        }
        else if (Input.GetButton("Run"))
        {
            mmTrajGen.MaxSpeed = runSpeed;
            mmTrajGen.PositionBias = runPosBias;
            mmTrajGen.DirectionBias = runDirBias;
            // mmAnimator.SetCalibrationData("Walk");
        }
        else if (Input.GetButton("Crouch"))
        {
            mmTrajGen.MaxSpeed = crouchSpeed;
            mmTrajGen.PositionBias = crouchPosBias;
            mmTrajGen.DirectionBias = crouchDirBias;
            // mmAnimator.SetCalibrationData("Crouch");
        }
        else
        {
            mmTrajGen.MaxSpeed = runSpeed;
            mmTrajGen.PositionBias = runPosBias;
            mmTrajGen.DirectionBias = runDirBias;
            // mmAnimator.SetCalibrationData("Run");

        }
 
    }
}
