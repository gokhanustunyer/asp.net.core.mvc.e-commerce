using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.business.Operations
{
    public class FileNameOperation
    {
        public static string CharacterRegulatory(string input)
        {
            string metaCharset = "+*/@'^()&=?*{}[]%$½><£#!,,.`|\"€₺´`ß¨:;";
            foreach (char c in metaCharset) { input = input.Replace(c.ToString(), ""); }
            var alphabetChars = new List<(string key, string value)>(){
                ("Ü","u"), ("Ö","o"), ("Ğ","g"),
                ("İ","i"), ("ı","i"), ("Ç","c"),
                ("Ş","s"), ("ğ","g"), ("ç","c"),
                ("ş","s"), ("ü","u"), ("ö","o"),
                ("~","-"), ("_","-"), (" ","-")
            };
            foreach (var kw in alphabetChars) { input = input.Replace(kw.key, kw.value); }
            return input;
        }
    }
}
