namespace json2ts.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xamasoft.JsonClassGenerator;
    using Xamasoft.JsonClassGenerator.CodeWriters;

    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        //http://www.json.org/example.html
        //http://carbonads.net/dev_code.php

        [HttpPost]
        public async Task<JsonResult> GetTypeScriptDefinition(string code, string ns, string root)
        {
            try
            {
                string x;
                if (code.StartsWith("http"))
                {
                    var request = (HttpWebRequest) WebRequest.Create(code);
                    request.UserAgent = "Mozilla/5.0";
                    request.Method = "GET";
                    request.ContentType = "application/json;charset=UTF-8";
                    var response = (HttpWebResponse) request.GetResponse();
                    string result = string.Empty;
                    if (response.ContentLength < 5000)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                        result = await readStream.ReadToEndAsync();
                    }
                    else
                    {
                        return new JsonResult
                        {
                            Data =
                                new {error = "Requested data is either too large (5kb max) or too small (2 byte min)"}
                        };
                    }
                    x = Prepare(result, "namespace", "RootObject");
                }
                else
                {
                    x = Prepare(code, "namespace", "RootObject");
                }
                return new JsonResult {Data = x};
            }
            catch (Exception ex)
            {
                return new JsonResult {Data = new {error = ex.Message}};
            }

          
        }

        private string Prepare(string code, string ns, string mainClass)
        {
            var gen = new JsonClassGenerator();
            gen.Example = code;
            TextWriter txtStream = new StringWriter();
            gen.OutputStream = txtStream;
            //gen.InternalVisibility = this.radInternal.Checked;
            gen.CodeWriter = new TypeScriptCodeWriter();

            gen.Namespace = ns;
            //gen.NoHelperClass = this.chkNoHelper.Checked;
            //gen.SecondaryNamespace = this.radDifferentNamespace.Checked && !string.IsNullOrEmpty(this.edtSecondaryNamespace.Text) ? this.edtSecondaryNamespace.Text : null;
            //gen.TargetFolder = this.edtTargetFolder.Text;
            //gen.UseProperties = true;
            gen.MainClass = mainClass;
            //gen.UsePascalCase = false;
            gen.UseNestedClasses = true; //this.radNestedClasses.Checked;
            //gen.ApplyObfuscationAttributes = this.chkApplyObfuscationAttributes.Checked;
            gen.SingleFile = true; //this.chkSingleFile.Checked;
            //gen.ExamplesInDocumentation = true;
            gen.GenerateClasses();
            return txtStream.ToString();
        }
    }
}