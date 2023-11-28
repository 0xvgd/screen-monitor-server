using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.Collections;
using System.Threading;

namespace Agent.Model
{
    class UserDataRecords
    {
        public Hashtable _installAppRecords = new Hashtable();
        public Hashtable _deviceRecords = new Hashtable();
        public Hashtable _imageRecords = new Hashtable();
        public Hashtable _processRecords = new Hashtable();
        public Hashtable _usbRecords = new Hashtable();


        private Queue<Model.Record.ImageRecordSet> _historyImageQueue = new Queue<Model.Record.ImageRecordSet>();

        Mutex mut = new Mutex(false, "UserDataRecords");

        private static UserDataRecords _instance = null;

        public static UserDataRecords Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserDataRecords();
                return _instance;
            }
        }

        private UserDataRecords()
        {
           
        }

        public void AddImageToQueue(Model.Record.ImageRecordSet imageRecord)
        {
          //  mut.WaitOne();
            _historyImageQueue.Enqueue(imageRecord);
          //  mut.ReleaseMutex();
        }

        public Model.Record.ImageRecordSet GetImageFromQueue()
        {
           // mut.WaitOne();
            Model.Record.ImageRecordSet imgRecord = null;
            if ( _historyImageQueue.Count > 0)
                imgRecord = _historyImageQueue.Dequeue();
            //mut.ReleaseMutex();
            return imgRecord;
        }



    }
}
