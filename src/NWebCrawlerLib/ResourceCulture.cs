using System.Reflection;  
using System.Resources;  
using System.Threading;  
using System.Globalization; 

namespace NWebCrawlerLib
{
    public class ResourceCulture
    {
         /// <summary>  
         /// Set current culture by name  
         /// </summary>  
         /// <param name="name">name</param>  
         public static void SetCurrentCulture(string strLanguageName)  
         {
             if (string.IsNullOrEmpty(strLanguageName))  
             {
                 strLanguageName = "en-US";  
             }

             Thread.CurrentThread.CurrentCulture = new CultureInfo(strLanguageName);  
         }  
   
         /// <summary>  
         /// Get string by id  
         /// </summary>  
         /// <param name="id">id</param>  
         /// <returns>current language string</returns>  
         public static string GetString(string strId)  
         {  
             string strCurLanguage = "";

             try  
             {
                 ResourceManager rm = new ResourceManager("NWebCrawlerLib.Properties.Resource", Assembly.GetExecutingAssembly());
                 CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                 strCurLanguage = rm.GetString(strId, cultureInfo);  
             }  
             catch  
             {  
                 strCurLanguage = "No such id:" + strId;  
             }  
   
             return strCurLanguage;  
         }  
    }
}
