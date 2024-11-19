using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager instance = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

