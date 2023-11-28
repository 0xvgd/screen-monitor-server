using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Agent.Model.Record
{
    public class ImageRecordSet : DataRecordSet
    {

        private Image _image = null;

        public int message_type;

        public ImageRecordSet(byte[] imageBuf)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(imageBuf, 0, imageBuf.Count());

            try
            {

                _image = Image.FromStream(stream);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                _image = null;
            }
        }

      
        public Image RecordImage
        {
            get { return _image; }
            set { _image = value; }
        }
    }
}
