using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_M1911 : WeaponGun
{
    public int bulletCount = 15;
    public Transform magazine = null;   // 탄창..
    public Transform slide = null;      // 슬라이드 (장전시 당기는것)..

    private int bulletCountTemp = 0;
    private bool install = true;        // 탄창이 장작되어있는지 여부..
    private bool load = true;           // 장전이 되어있는지 여부..
    private Vector3 prevHand = Vector3.zero;
    private float slidemove = 0f;

    protected override void OnStart()
    {
        type = WeaponType.weapon_m911;
        bulletCountTemp = bulletCount;
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        //if (install == false && load == false)
        //{
        //    if (PlayerInput.Instance.GetTouchUpTriggerL())
        //    {
        //        float dis = Vector3.Distance(slide.position, PlayerInput.Instance.TrackedObjL.position);
        //        if (dis < 0.3f)
        //            Install();
        //    }
        //}
        //else if(install == true && load == false)
        //{
        //    if (PlayerInput.Instance.GetTouchTriggerL())
        //    {
        //        float dis = Vector3.Distance(slide.position, PlayerInput.Instance.TrackedObjL.position);
        //        if (dis < 0.2f)
        //            Reload();
        //    }
        //}
        base.OnUpdate();
    }

    protected override void Shot()
    {
        //if (load == false)
        //    return;

        //bulletCount -= 1;
        //if (bulletCount <= 0)
        //{
        //    // 탄창빠짐..
        //    if (ani) ani.SetTrigger("magazine_out");
        //    //magazine.gameObject.SetActive(false);
        //    install = false;
        //    load = false;
        //}
        base.Shot();
    }

    private void Install()
    {
        // 탄창 장착..
        if (bulletCount <= 0)
        {
            install = true;
            slidemove = 0f;
            if (ani) ani.SetTrigger("magazine_in");
            //magazine.gameObject.SetActive(true);
            bulletCount = bulletCountTemp;
        }
    }

    private void Reload()
    {
        // 장전..
        if(prevHand == Vector3.zero)
            prevHand = PlayerInput.Instance.TrackedObjL.position;
        float dot = Vector3.Dot(slide.forward, (PlayerInput.Instance.TrackedObjL.position - prevHand).normalized);
        float dis = Vector3.Distance(prevHand, PlayerInput.Instance.TrackedObjL.position);
        if(dot > 0.5f)
        {
            slidemove += dis;
            slide.localPosition = Define.WCVector3(0f, 0f, slidemove);
            if (slide.localPosition.z < 0f)
                slide.localPosition = Vector3.zero;
            else if (slide.localPosition.z > 0.01f)
            {
                load = true;
                prevHand = Vector3.zero;
                StartCoroutine("SlideMove", 0.2f);
            }
        }
        prevHand = PlayerInput.Instance.TrackedObjL.position;
    }

    IEnumerator SlideMove(float time)
    {
        yield return null;

         float alltime = 0;
        while (alltime < time)
        {
            alltime += Time.deltaTime;
            float value = alltime * 1 / time;
            slide.localPosition += Define.WCVector3(0f, 0f, Mathf.Lerp(slidemove, 0f, value));
            yield return null;
        }
        slide.localPosition = Vector3.zero;
    }
}
