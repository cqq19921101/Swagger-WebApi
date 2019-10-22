using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 文件處理類
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 獲取文件大小(KB)
        /// </summary>
        /// <param name="fullFileName">包含路徑的完整文件名</param>
        /// <returns></returns>
        public static long GetFileSize(string fullFileName)
        {
            FileInfo fi = null;
            long fileSize = 0;
            try
            {
                fi = new FileInfo(fullFileName);
                fileSize = fi.Length / 1024;//KB
            }
            catch
            {
                fileSize = 0;
            }
            finally
            {
                fi = null;
            }
            return fileSize;
        }

        /// <summary>
        /// 將文件讀取為byte數組
        /// </summary>
        /// <param name="fullFileName">包含路徑的完整文件名</param>
        /// <returns></returns>
        public static byte[] FileToByteArray(string fullFileName)
        {
            try
            {
                using (FileStream fs = File.OpenRead(fullFileName))
                {
                    byte[] b = new byte[fs.Length];
                    fs.Read(b, 0, b.Length);
                    return b;
                }
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 保存byte數組到指定文件
        /// </summary>
        /// <param name="b">byte數組</param>
        /// <param name="fullFileName">包含路徑的完整文件名</param>
        public static void ByteArrayToFile(byte[] b, string fullFileName)
        {
            try
            {
                File.WriteAllBytes(fullFileName, b);
            }
            catch (Exception ex)
            {
                throw new Exception("保存文件失敗:\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 得到重新設置尺寸的圖片byte數組，如果原寬度比新寬度小，保持原寬度
        /// </summary>
        /// <param name="fullImageName">包含路徑的完整圖片名稱</param>
        /// <param name="targetWidth">目標寬度</param>
        /// <returns></returns>
        public static byte[] ResizeImage(string fullImageName, int targetWidth)
        {
            using (FileStream fs = new FileStream(fullImageName, FileMode.Open, FileAccess.Read))
            {
                return ResizeImage(fs, targetWidth);
            }
        }

        /// <summary>
        /// 得到重新設置尺寸的圖片byte數組，如果原寬度比新寬度小，保持原寬度
        /// </summary>
        /// <param name="fs">FileStream</param>
        /// <param name="targetWidth">目標寬度</param>
        /// <returns></returns>
        public static byte[] ResizeImage(FileStream fs, int targetWidth)
        {
            Bitmap b = new Bitmap(fs);
            return DoResizeImage(b, targetWidth);
        }

        /// <summary>
        /// 得到重新設置尺寸的圖片byte數組，如果原寬度比新寬度小，保持原寬度
        /// </summary>
        /// <param name="ms">MemoryStream</param>
        /// <param name="targetWidth">目標寬度</param>
        /// <returns></returns>
        public static byte[] ResizeImage(MemoryStream ms, int targetWidth)
        {
            Bitmap b = new Bitmap(ms);
            return DoResizeImage(b, targetWidth);
        }

        private static byte[] DoResizeImage(Bitmap b, int targetWidth)
        {
            Bitmap bn;
            int oHeight, oWidth, nHeight, nWidth;
            oHeight = b.Height;
            oWidth = b.Width;

            byte[] bt;

            if (oWidth <= targetWidth)
            {
                targetWidth = oWidth;
            }

            nHeight = oHeight * targetWidth / oWidth;
            nWidth = targetWidth;

            Rectangle rgO = new Rectangle(0, 0, oWidth, oHeight);
            Rectangle rgN = new Rectangle(0, 0, nWidth, nHeight);


            bn = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(bn);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(b, rgN, rgO, GraphicsUnit.Pixel);
            g.Dispose();
            //Image image = b.GetThumbnailImage(oWidth, oHeight, null, System.IntPtr.Zero);//图像质量损失大

            MemoryStream ms2 = new MemoryStream();
            bn.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
            bt = new byte[(int)ms2.Length];
            ms2.Position = 0;
            ms2.Read(bt, 0, (int)ms2.Length);
            ms2.Close();
            ms2.Dispose();
            ms2 = null;
            bn.Dispose();
            bn = null;
            g.Dispose();
            g = null;

            return bt;
        }
           

        /// <summary>
        /// Byte數組轉為Image
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Image ByteArrayToImage(byte[] b)
        {
            Image img = null;
            try
            {
                using (MemoryStream ms = new MemoryStream(b))
                {
                    img = Image.FromStream(ms);
                }
            }
            catch(Exception ex)
            {

            }
            return img;
        }

        /// <summary>
        /// 將Image保存成圖片
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="fullImagePath">包含路徑的完整圖片名稱</param>
        public static void ImageToFile(Image img, string fullImagePath)
        {
            Bitmap b = new Bitmap(img);
            try
            {
                b.Save(fullImagePath);
            }
            catch (Exception ex)
            {
                throw new Exception("保存圖片失敗：\r\n" + ex.Message);
            }
            finally
            {
                b = null;
            }
        }

        /// <summary>
        /// 將PNG格式的IMAGE轉為byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] PngImageToByteArray(Image image)
        {
            try
            {
                MemoryStream ms = new MemoryStream();

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                return ms.ToArray();
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 將JPG格式的IMAGE轉為byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] JpgImageToByteArray(Image image)
        {
            try
            {
                MemoryStream ms = new MemoryStream();

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
