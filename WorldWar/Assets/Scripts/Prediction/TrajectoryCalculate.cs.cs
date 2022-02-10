using UnityEngine;
using System.Collections;

namespace BallUtil
{
    /// <summary>
    /// Trajectory calculate.
    /// </summary>
    public static class TrajectoryCalculate
    {
        public static Vector3 Force(
            Vector3 start,
            Vector3 force,
            float mass,
            Vector3 gravity,
            float gravityScale,
            float time
        )
        {
            var speedX = force.x / mass * Time.fixedDeltaTime;
            var speedY = force.y / mass * Time.fixedDeltaTime;
            var speedZ = force.z / mass * Time.fixedDeltaTime;

            var halfGravityX = gravity.x * 0.5f * gravityScale;
            var halfGravityY = gravity.y * 0.5f * gravityScale;
            var halfGravityZ = gravity.z * 0.5f * gravityScale;

            var positionX = speedX * time + halfGravityX * Mathf.Pow(time, 2);
            var positionY = speedY * time + halfGravityY * Mathf.Pow(time, 2);
            var positionZ = speedZ * time + halfGravityZ * Mathf.Pow(time, 2);

            return start + new Vector3(positionX, positionY, positionZ);
        }

        public static void GetDisplacement(ref float fdX, ref float fdY, ref float fdZ, Vector3 vStartPos, float fFx, float fFy, float fFz, 
            float fSpin, float fAirResistance, float fAy, float fMass, float fDurTime, bool bPit = false)//, float fDrag = 0f)
        {
            //변위 역추적
            float fVx = fFx / fMass * Time.fixedDeltaTime;
            float fVy = fFy / fMass * Time.fixedDeltaTime;
            float fVz = fFz / fMass * Time.fixedDeltaTime;

            Vector3 vForceTotal = new Vector3(fFx, fFy, fFz);
            //Vector3 vDir = vForceTotal;
            //vDir.y = 0;
            //vDir.Normalize();
            //float fSpinX = fSpin * vDir.z;
            //float fSpinZ = fSpin * vDir.x;

            //속도 및 저항
            float fR = 1f - fAirResistance;                                         //등비
            fR = Mathf.Pow(fR, Time.fixedDeltaTime / 0.01f);
            float fTotalX = fVx;
            float fTotalZ = fVz;
            if (fR != 1f)
            {
                fTotalX = fR == 1f ? fVx : fVx * (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) / (1f - fR);   //등비수열 합 X
                fTotalZ = fR == 1f ? fVz : fVz * (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) / (1f - fR);   //등비수열 합 Z
                fTotalX = fTotalX / (fDurTime / Time.fixedDeltaTime);
                fTotalZ = fTotalZ / (fDurTime / Time.fixedDeltaTime);
            }

            //스핀
            Vector3 vVelNormal = vForceTotal.normalized;
            Vector3 vRelativeSpin = vVelNormal * Mathf.Abs(fSpin);
            float fNewVelAngle = fSpin > 0 ? -90 : 90;
            WMath.Inst.RotateY(ref vRelativeSpin, -Mathf.Deg2Rad * fNewVelAngle);
            if (bPit)
            {
                vRelativeSpin = -vRelativeSpin;
            }
            //fdX = vStartPos.x + fVx * fDurTime + 0.5f * fSpinX * fDurTime * fDurTime;
            fdX = vStartPos.x + fTotalX * fDurTime + 0.5f * vRelativeSpin.x * fDurTime * fDurTime;
            //fdX = vStartPos.x + fTotalX * fDurTime + fTotalSpinX * fDurTime;

            fdY = vStartPos.y + fVy * fDurTime + 0.5f * (-fAy + Physics.gravity.y) * fDurTime * fDurTime;
            //fdZ = vStartPos.z + fVz * fDurTime + 0.5f * fSpinZ * fDurTime * fDurTime;
            fdZ = vStartPos.z + fTotalZ * fDurTime + 0.5f * vRelativeSpin.z * fDurTime * fDurTime;
            //fdZ = vStartPos.z + fTotalZ * fDurTime + fTotalSpinZ * fDurTime;
        }

        //public static void GetForceValue(ref float fFx, ref float fFy, ref float fFz, Vector3 vStartPos, Vector3 vTargetPos, float fSpin, float fAirResistance, float fAy, float fMass, float fDurTime)
        //{
        //    //속도(힘) 역추적
        //    float fDistX = vTargetPos.x - vStartPos.x;
        //    float fDistY = vTargetPos.y - vStartPos.y;
        //    float fDistZ = vTargetPos.z - vStartPos.z;

        //    Vector3 vForceTotal = new Vector3(fDistX, fDistY, fDistZ);
        //    //Vector3 vDir = vForceTotal;
        //    //vDir.y = 0;
        //    //vDir.Normalize();
        //    //float fSpinX = fSpin * vDir.z;
        //    //float fSpinZ = fSpin * vDir.x;

        //    //속도 및 저항 역추적
        //    if (fAirResistance == 0)
        //        fAirResistance = 0.000001f;
        //    float fR = 1f - fAirResistance;                                         //등비
        //    fR = Mathf.Pow(fR, Time.fixedDeltaTime / 0.01f);
        //    //float fTotalX = fVx * (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) / (1f - fR);   //등비수열 합 X
        //    //float fTotalZ = fVz * (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) / (1f - fR);   //등비수열 합 Z
        //    //fTotalX = fTotalX / (fDurTime / Time.fixedDeltaTime);
        //    //fTotalZ = fTotalZ / (fDurTime / Time.fixedDeltaTime);

        //    //스핀
        //    Vector3 vVelNormal = vForceTotal.normalized;
        //    Vector3 vRelativeSpin = vVelNormal * Mathf.Abs(fSpin);
        //    float fNewVelAngle = fSpin > 0 ? -90 : 90;
        //    WMath.Inst.RotateY(ref vRelativeSpin, -Mathf.Deg2Rad * fNewVelAngle);

        //    //fDistX -= (0.5f * vRelativeSpin.x * fDurTime * fDurTime * (Time.fixedDeltaTime / 0.01f));
        //    //fDistX -= (0.5f * vRelativeSpin.x * fDurTime * fDurTime);
        //    //fDistZ -= (0.5f * vRelativeSpin.z * fDurTime * fDurTime * (Time.fixedDeltaTime / 0.01f));
        //    //fDistZ -= (0.5f * vRelativeSpin.z * fDurTime * fDurTime);
        //    /*
        //    //float fVx = (fDistX / fDurTime - 0.5f * vRelativeSpin.x * fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime);
        //    float fVx = (fDistX / fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime);// -0.5f * vRelativeSpin.x * fDurTime;
        //    float fVy = fDistY / fDurTime - 0.5f * (-fAy + Physics.gravity.y) * fDurTime;
        //    //float fVz = (fDistZ / fDurTime - 0.5f * vRelativeSpin.z * fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime);
        //    float fVz = (fDistZ / fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime);// -0.5f * vRelativeSpin.z * fDurTime;
        //    */
        //    float fVx = fDistX / fDurTime - 0.5f * vRelativeSpin.x * fDurTime;
        //    float fVy = fDistY / fDurTime - 0.5f * (-fAy + Physics.gravity.y) * fDurTime;
        //    float fVz = fDistZ / fDurTime - 0.5f * vRelativeSpin.z * fDurTime;
        //    //float fVx = (fDistX - (0.5f * fAx * fDurTime * fDurTime)) / (fDurTime * Time.fixedDeltaTime);
        //    //float fVy = (fDistY - (0.5f * (-fAy + Physics.gravity.y) * fDurTime * fDurTime)) / (fDurTime * Time.fixedDeltaTime);
        //    //float fVz = (fDistZ - (0.5f * fAz * fDurTime * fDurTime)) / (fDurTime * Time.fixedDeltaTime);

        //    fVx = fVx * Mathf.Pow(fR, Mathf.Exp(fDurTime / Time.fixedDeltaTime));
        //    fVz = fVz * Mathf.Exp(fDurTime / Time.fixedDeltaTime) * fAirResistance;

        //    fFx = fVx * fMass / Time.fixedDeltaTime;
        //    fFy = fVy * fMass / Time.fixedDeltaTime;
        //    fFz = fVz * fMass / Time.fixedDeltaTime;
        //}

        public static void GetForceValue(ref float fFx, ref float fFy, ref float fFz, Vector3 vStartPos, Vector3 vTargetPos, 
            float fSpin, float fAirResistance, float fAy, float fMass, float fDurTime, bool bPit = false)
        {
            //속도(힘) 역추적
            float fDistX = vTargetPos.x - vStartPos.x;
            float fDistY = vTargetPos.y - vStartPos.y;
            float fDistZ = vTargetPos.z - vStartPos.z;

            Vector3 vForceTotal = new Vector3(fDistX, fDistY, fDistZ);
            //Vector3 vDir = vForceTotal;
            //vDir.y = 0;
            //vDir.Normalize();
            //float fSpinX = fSpin * vDir.z;
            //float fSpinZ = fSpin * vDir.x;

            //속도 및 저항 역추적
            if (fAirResistance == 0)
                fAirResistance = 0.000001f;
            float fR = 1f - fAirResistance;                                         //등비
            fR = Mathf.Pow(fR, Time.fixedDeltaTime / 0.01f);
            //float fTotalX = fVx * (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) / (1f - fR);   //등비수열 합 X
            //float fTotalZ = fVz * (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) / (1f - fR);   //등비수열 합 Z
            //fTotalX = fTotalX / (fDurTime / Time.fixedDeltaTime);
            //fTotalZ = fTotalZ / (fDurTime / Time.fixedDeltaTime);

            //스핀
            Vector3 vVelNormal = vForceTotal.normalized;
            Vector3 vRelativeSpin = vVelNormal * Mathf.Abs(fSpin);
            float fNewVelAngle = fSpin > 0 ? -90 : 90;
            WMath.Inst.RotateY(ref vRelativeSpin, -Mathf.Deg2Rad * fNewVelAngle);
            if(bPit)
            {
                vRelativeSpin = -vRelativeSpin;
            }
            //fDistX -= (0.5f * vRelativeSpin.x * fDurTime * fDurTime * (Time.fixedDeltaTime / 0.01f));
            ////fDistX -= (0.5f * vRelativeSpin.x * fDurTime * fDurTime);
            //fDistZ -= (0.5f * vRelativeSpin.z * fDurTime * fDurTime * (Time.fixedDeltaTime / 0.01f));
            ////fDistZ -= (0.5f * vRelativeSpin.z * fDurTime * fDurTime);

            //float fVx = (fDistX / fDurTime - 0.5f * vRelativeSpin.x * fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime);
            float fVx = (fDistX / fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime) -0.5f * vRelativeSpin.x * fDurTime;
            float fVy = fDistY / fDurTime - 0.5f * (-fAy + Physics.gravity.y) * fDurTime;
            //float fVz = (fDistZ / fDurTime - 0.5f * vRelativeSpin.z * fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime);
            float fVz = (fDistZ / fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime) -0.5f * vRelativeSpin.z * fDurTime;

            //float fVx = (fDistX - (0.5f * fAx * fDurTime * fDurTime)) / (fDurTime * Time.fixedDeltaTime);
            //float fVy = (fDistY - (0.5f * (-fAy + Physics.gravity.y) * fDurTime * fDurTime)) / (fDurTime * Time.fixedDeltaTime);
            //float fVz = (fDistZ - (0.5f * fAz * fDurTime * fDurTime)) / (fDurTime * Time.fixedDeltaTime);
            fFx = fVx * fMass / Time.fixedDeltaTime;
            fFy = fVy * fMass / Time.fixedDeltaTime;
            fFz = fVz * fMass / Time.fixedDeltaTime;
        }

        public static void GetForceValue_NoGravity(ref float fFx, ref float fFy, ref float fFz, Vector3 vStartPos, Vector3 vTargetPos,
            float fSpin, float fAirResistance, float fAy, float fMass, float fDurTime, bool bPit = false)
        {
            //속도(힘) 역추적
            float fDistX = vTargetPos.x - vStartPos.x;
            float fDistY = vTargetPos.y - vStartPos.y;
            float fDistZ = vTargetPos.z - vStartPos.z;

            Vector3 vForceTotal = new Vector3(fDistX, fDistY, fDistZ);

            //속도 및 저항 역추적
            if (fAirResistance == 0)
                fAirResistance = 0.000001f;
            float fR = 1f - fAirResistance;                                         //등비
            fR = Mathf.Pow(fR, Time.fixedDeltaTime / 0.01f);

            //스핀
            Vector3 vVelNormal = vForceTotal.normalized;
            Vector3 vRelativeSpin = vVelNormal * Mathf.Abs(fSpin);
            float fNewVelAngle = fSpin > 0 ? -90 : 90;
            WMath.Inst.RotateY(ref vRelativeSpin, -Mathf.Deg2Rad * fNewVelAngle);
            if (bPit)
            {
                vRelativeSpin = -vRelativeSpin;
            }

            float fVx = (fDistX / fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime) - 0.5f * vRelativeSpin.x * fDurTime;
            //float fVy = fDistY / fDurTime - 0.5f * (-fAy + Physics.gravity.y) * fDurTime;
            float fVy = (fDistY / fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime) - 0.5f * vRelativeSpin.y * fDurTime;
            float fVz = (fDistZ / fDurTime) * (1f - fR) / (1f - Mathf.Pow(fR, fDurTime / Time.fixedDeltaTime)) * (fDurTime / Time.fixedDeltaTime) - 0.5f * vRelativeSpin.z * fDurTime;

            fFx = fVx * fMass / Time.fixedDeltaTime;
            fFy = fVy * fMass / Time.fixedDeltaTime;
            fFz = fVz * fMass / Time.fixedDeltaTime;
        }

        public static Vector3 Velocity(
            Vector3 start,
            Vector3 velocity,
            Vector3 gravity,
            float gravityScale,
            float time
        )
        {
            var halfGravityX = gravity.x * 0.5f * gravityScale;
            var halfGravityY = gravity.y * 0.5f * gravityScale;
            var halfGravityZ = gravity.z * 0.5f * gravityScale;

            var positionX = velocity.x * time + halfGravityX * Mathf.Pow(time, 2);
            var positionY = velocity.y * time + halfGravityY * Mathf.Pow(time, 2);
            var positionZ = velocity.z * time + halfGravityZ * Mathf.Pow(time, 2);

            return start + new Vector3(positionX, positionY, positionZ);
        }

        public static float GetBoundTime(Vector3 vStartPos, Vector3 vVel)
        {
            float fD = vStartPos.y - 0f;
            fD = -fD;

            float fA = 0.5f * Physics.gravity.y;
            float fB = -vVel.y;
            float fC = fD;

            float fVal1 = (-fB + Mathf.Sqrt(fB*fB - 4f * fA*fC)) / (2f*fA);
            float fVal2 = (-fB - Mathf.Sqrt(fB * fB - 4f * fA * fC)) / (2f * fA);
            if (fVal1 < 0)
                fVal1 = -fVal1;
            return fVal1;
            //float fVal = 0;
            //float fA = 0;
            //float fTemp = Mathf.Pow(vVel.y, 2) - (4f * 0.5f * Physics.gravity.y * fD);
            //if (fTemp > 0f)
            //{
            //    //fVal = (-vVel.y + Mathf.Sqrt(fTemp)) / Physics.gravity.y;
            //    fVal = (-vVel.y - Mathf.Sqrt(fTemp)) / Physics.gravity.y;
            //    fA = (-vVel.y + Mathf.Sqrt(fTemp)) / Physics.gravity.y;
            //}
            //else
            //{
            //    fVal = (-vVel.y - Mathf.Sqrt(-fTemp)) / Physics.gravity.y;
            //    fA = (-vVel.y + Mathf.Sqrt(-fTemp)) / Physics.gravity.y;
            //}

            //else
            //{
            //    if (2f * fD / Physics.gravity.y < 0)
            //        fVal = Mathf.Sqrt(-2f * fD / Physics.gravity.y);
            //    else
            //        fVal = Mathf.Sqrt(2f * fD / Physics.gravity.y);
            //}
            //return fVal;
        }
    }
}