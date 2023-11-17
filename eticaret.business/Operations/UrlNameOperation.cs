using eticaret.data.Abstract.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Operations
{
    public static class UrlNameOperation
    {
        public delegate bool Contains(string url);
        public static string CharacterRegulatory(string url, Contains contains)
        {
            url = FileNameOperation.CharacterRegulatory(url);
            url = url.ToLower();
            int counter = 0;
            string tempUrl = url;
            while (contains(url))
            {
                url = $"{tempUrl}-{counter}";
                counter += 1;
            }

            return (counter == 0) ? url : $"{tempUrl}-{counter}";
        }
    }
}
