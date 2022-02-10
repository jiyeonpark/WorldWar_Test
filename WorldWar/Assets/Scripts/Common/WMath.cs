using UnityEngine;
using System.Collections;

public class WMath
{
    #region singleton
    private static WMath _Inst = null;
    public static WMath Inst
    {
        get
        {
            if (_Inst == null)
            {
                _Inst = new WMath();
            }
            return _Inst;
        }
    }
    #endregion

    public float getAngle(float fOrgAngle)
    {
        float fAngle = fOrgAngle;
        int nCount = 0;
        while (true)
        {
            if (fAngle < 0f)
            {
                fAngle += 360f;
            }
            else
            {
                if (fAngle > 180f)
                {
                    fAngle -= 360f;
                }
            }
            if (fAngle <= 180f && fAngle >= -180f)
                break;

            nCount++;

            if (nCount > 10)
            {
                Debug.LogError("Error Too many Loop OrgAngle::" + fOrgAngle);
                return 0f;
            }
        }
        return fAngle;

    }

    public float ContAngle(Vector3 fwd, Vector3 targetDir)
    {
        float angle = Vector3.Angle(fwd, targetDir);

        if (AngleDir(fwd, targetDir, Vector3.up) == -1)
        {
            angle = 360.0f - angle;
            if (angle > 359.9999f)
                angle -= 360.0f;
            return angle;
        }
        else
            return angle;
    }

    public int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0)
            return 1;
        else if (dir < 0.0)
            return -1;
        else
            return 0;
    }

    public Quaternion FromToRotationY(Vector3 vFrome, Vector3 vTo)
    {
        Vector3 vFromeNorm = vFrome;
        vFromeNorm.y = 0f;
        vFromeNorm.Normalize();

        Vector3 vToNorm = vTo;
        vToNorm.y = 0f;
        vToNorm.Normalize();

        float fDotVal = Vector3.Dot(vFromeNorm, vToNorm);
        float fDegree = Mathf.Acos(fDotVal) * Mathf.Rad2Deg;

        if (float.IsNaN(fDegree))
            fDegree = 0f;

        if (Vector3.Cross(vFromeNorm, vToNorm).y < 0f)
            fDegree = -fDegree;

        fDegree = getAngle(fDegree);
        Quaternion result = Quaternion.AngleAxis(fDegree, Vector3.up);
        return result;
    }

    public float GetAngleBetweenTwoVector(Vector3 v1, Vector3 v2)	//XY
    {
        v1.Normalize();
        v2.Normalize();

        double dAngle1 = Mathf.Atan2(v1.x, v1.z) * Mathf.Rad2Deg;
        double dAngle2 = Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;

        double dDiffAngles = dAngle1 - dAngle2;

        if (dDiffAngles < 0)
            dDiffAngles = 360 + dDiffAngles;

        return (float)dDiffAngles;
    }

    public float GetAngleBetweenTwoVector_H(Vector3 v1, Vector3 v2)	//XY
    {
        v1.Normalize();
        v2.Normalize();

        double dAngle1 = Mathf.Atan2(v1.y, v1.z) * Mathf.Rad2Deg;
        double dAngle2 = Mathf.Atan2(v2.y, v2.z) * Mathf.Rad2Deg;

        double dDiffAngles = dAngle1 - dAngle2;

        if (dDiffAngles < 0)
            dDiffAngles = 360 + dDiffAngles;

        return (float)dDiffAngles;
    }

    public float GetRotateDegree(Vector3 vStart, Vector3 vTarget)
    {
        vTarget.y = 0; vStart.y = 0;
        Vector3 vDir = vTarget - vStart;
        return Mathf.Rad2Deg * (Mathf.Atan2(vDir.x, vDir.z));
    }
    public float GetRotateHDegree(Vector3 vStart, Vector3 vTarget)
    {
        vTarget.x = 0; vStart.x = 0;
        Vector3 vDir = vTarget - vStart;
        return Mathf.Rad2Deg * (Mathf.Atan2(vDir.y, vDir.z));
    }
    public Vector3 GetDirection(Vector3 vStart, Vector3 vEnd)
    {
        vStart.y = vEnd.y = 0f;
        Vector3 vDist = vEnd - vStart;
        return vDist.normalized;
    }

    public float GetRotX(Transform trans)
    {
        Quaternion q = trans.rotation;
        float fFwdY = (2 * (q.y * q.z - q.x * q.w));
        float fFwdZ = (2 * (q.z * q.z + q.w * q.w) - 1);
        return Mathf.Atan2(-fFwdY, fFwdZ);
    }

    public float GetRotY(Transform trans)
    {
        Quaternion q = trans.rotation;
        float fFwdX = (2 * (q.x * q.z - q.y * q.w));
        float fFwdZ = (2 * (q.z * q.z + q.w * q.w) - 1);
        return Mathf.Atan2(-fFwdX, fFwdZ);
    }

    public void SetRotTo(Transform trans, Vector3 vTarget)
    {
        Vector3 vPos = trans.position;
        float fRotate = WMath.Inst.GetRotateDegree(vPos, vTarget);
        trans.rotation = Quaternion.AngleAxis(fRotate, Vector3.up);
    }

    /// <summary>
    /// other위치에서 시작한 직선 A와 start 위치와 end 위치를 지나는 직선 B의 직교하는 위치를 계산합니다.
    /// </summary>
    /// <param name="start">직선B 시작점</param>
    /// <param name="end">직선B 끝점</param>
    /// <param name="other">직선A 시작점</param>
    /// <returns>직선A와 직선B의 직교점</returns>
    public Vector3 GetOrthogonalPoint(Vector3 start, Vector3 end, Vector3 other)
    {
        //A + DotProduct(norm(A->B) , A->C) * norm( A->B )
        return start + Vector3.Dot(Vector3.Normalize(end), other) * Vector3.Normalize(end);
    }

    public float GetDistXZ(Vector3 vPos1, Vector3 vPos2)
    {
        Vector3 vDiff = vPos1 - vPos2;
        vDiff.y = 0;
        return vDiff.magnitude;
    }

    public float ToMSSpeed(float fKmh)
    {
        return fKmh * 0.27777778f;
    }

    public float ToKMHSpeed(float fms)
    {
        return fms * 3.6f;
    }

    //Vector3 Rotation
    public void RotateX(ref Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float ty = v.y;
        float tz = v.z;
        v.y = (cos * ty) + (sin * tz);
        v.z = (cos * tz) - (sin * ty);
    }
    public void RotateY(ref Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) + (sin * tz);
        v.z = (cos * tz) - (sin * tx);
    }
    public void RotateZ(ref Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) + (sin * ty);
        v.y = (cos * ty) - (sin * tx);
    }
    //180도 밖에 지원해주지 않는 Vector3.angle 변환을 위해
    public float RightDot(Vector3 Target)
    {
        float Dot = Vector3.Dot(Target, Vector3.right);

        if (Dot < 0)
            return 360.0f;

        return 0.0f;
    }

    //Vector3 Lerp
    public Vector3 LerpVector3(Vector3 From, Vector3 To, float lerp)
    {
        Vector3 vLerp = Vector3.zero;
        vLerp.x = Mathf.Lerp(From.x, To.x, lerp);
        vLerp.y = Mathf.Lerp(From.y, To.y, lerp);
        vLerp.z = Mathf.Lerp(From.z, To.z, lerp);

        return vLerp;
    }

    //방향벡터의 값을 이용하여 aG, aH, v값을 추출한다.
    public void DirectionToElement(ref float rOutH, ref float rOutV, ref float rOutS, Vector3 vDir)
    {
        float fLen = Mathf.Sqrt(vDir.z * vDir.z + vDir.x * vDir.x);

        rOutH = Mathf.Atan2(vDir.x, vDir.z);
        rOutV = Mathf.Atan2(vDir.y, fLen);
        rOutS = vDir.sqrMagnitude;// D3DXVec3Length(&vDir);			
    }

    public Vector3 ElementToDirection(float fRadianV, float fRadianH, float fSpeed = 1f)
    {
        float fX = fSpeed * Mathf.Sin(fRadianH);
        float fY = fSpeed * Mathf.Sin(fRadianV); //y가 높이
        float fZ = fSpeed * Mathf.Cos(fRadianH);


        return new Vector3(fX, fY, fZ);
    }
}