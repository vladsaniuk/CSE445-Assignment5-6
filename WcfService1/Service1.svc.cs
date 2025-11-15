using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string WebDownload(string url)
        {
            try
            {
                WebClient channel = new WebClient();
                string content = channel.DownloadString(url);
                return content;
            }
            catch (Exception ex)
            {
                return "Error downloading content: " + ex.Message;
            }
        }
    }
}
