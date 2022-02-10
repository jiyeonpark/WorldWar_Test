using UnityEngine;
using System.Collections;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

//Note : This script does not support mobile platform
[RequireComponent(typeof(AudioSource))]
public class MovieTexturePlayer : MonoBehaviour
{
		public bool playOnStart = true;

        public bool bPlay=false;
		public MovieTexture movieTexture;

		public AudioSource audioSource;

		public IEnumerator StartPlay ()
		{
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
		
			audioSource.Stop ();
            Play ();

			yield return 0;
		}
		
		/// <summary>
		/// Play the movie
		/// </summary>
		public void Play ()
		{
			if(movieTexture == null){
				Debug.Log("Movie texture is undefined");
				return;
		    }
			GetComponent<Renderer>().enabled = true;
			GetComponent<Renderer>().material.mainTexture = movieTexture;
			movieTexture.Play ();
			audioSource.clip = movieTexture.audioClip;
			audioSource.Play ();

            bPlay = true;
		}

        void Update()
        {
            if (bPlay)
            {
                if ( !movieTexture.isPlaying)
                {
                    bPlay = false;
                    GameEvents.FinishCI();
                }
            }
        }
}