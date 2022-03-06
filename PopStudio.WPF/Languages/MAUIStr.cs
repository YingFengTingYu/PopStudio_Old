using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopStudio.WPF.Languages
{
    internal class MAUIStr
    {
        static ILocalization obj;

        public static ILocalization Obj => obj ?? LoadLanguage();

        static ILocalization LoadLanguage()
        {
            switch (Setting.AppLanguage)
            {
                case Language.ZHCN: return obj = new MAUIZHCN();
                case Language.ENUS: return obj = new MAUIENUS();
                default: throw new Exception("Language Not Supported!");
            }
        }
    }
}
