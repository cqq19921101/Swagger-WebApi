using System;
using System.Collections.Generic;
using System.Text;
using System.Media;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 多媒體類
    /// </summary>
    public class Media
    {
        /// <summary>
        /// 播放wav格式的聲音
        /// </summary>
        /// <param name="soundFileName">wav聲音文件的完整路徑</param>
        public static void PlaySound(string soundFileName)
        {
            PlaySound(0, soundFileName);
        }

        /// <summary>
        /// 播放wav格式的聲音
        /// </summary>
        /// <param name="soundType">soundType=0時播放soundFileName中指定的聲音</param>
        /// <param name="soundFileName">wav聲音文件的完整路徑</param>
        public static void PlaySound(int soundType, string soundFileName)
        {

            SoundPlayer sp = new SoundPlayer();
            switch (soundType)
            {
                case 0:
                    sp.SoundLocation = soundFileName;
                    break;
                default:
                    //sp = new SoundPlayer("");
                    sp.SoundLocation = soundFileName;
                    break;
            }
            //SP.Load();
            try
            {
                if (soundType == 0)
                {
                    if (System.IO.File.Exists(soundFileName) == false)
                    {
                        SystemSounds.Beep.Play();
                        return;
                    }
                }
                sp.Play();
            }
            catch
            {
                SystemSounds.Beep.Play();
            }
        }
    }
}
