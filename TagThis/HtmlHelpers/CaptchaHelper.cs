using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

namespace TagThis.HtmlHelpers
{
    public static class CaptchaHelper
    {
        public static string GenerateCaptcha( this HtmlHelper helper )
        {  
        var captchaControl = new Recaptcha.RecaptchaControl  
            {  
                    ID = "recaptcha",  
                    Theme = "white",
                    PublicKey = "6Lf_ogoAAAAAAPBvaasgc0XLR7y9IW1VfmqJGCk1",
                    PrivateKey = "6Lf_ogoAAAAAAI4Z01k8bwQ_1d05ZPWFV7_SNgMM"
             };  
  
    var htmlWriter = new HtmlTextWriter( new StringWriter() );  
  
    captchaControl.RenderControl(htmlWriter);  
  
    return htmlWriter.InnerWriter.ToString();  
       }  
    }
}
