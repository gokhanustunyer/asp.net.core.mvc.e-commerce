using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mkiyafetleri.webui.SiteOperations
{
    public static class PageSettings
    {
        public static bool isOffline { get{
                PageModel pageModel;
                using (StreamReader r = new StreamReader("./pagesettings.json"))
                {
                    string json = r.ReadToEnd();
                    pageModel = JsonConvert.DeserializeObject<PageModel>(json);
                };
                return pageModel.isOffline;
        }}

        public static void ChangePageStatus(bool isOffline)
        {
            string serilize;
            serilize = JsonConvert.SerializeObject
                    (new PageModel() { isOffline = isOffline });
            File.WriteAllText("./pagesettings.json", serilize);
        }
    }
}
