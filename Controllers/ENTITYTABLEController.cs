using appBLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
//using Microsoft.Data.SqlClient;


namespace appAPP.Controllers
{

    //[Authorize]
    public class ENTITYTABLEController : Controller
    {

        private IHostingEnvironment _Env;
        private IHttpContextAccessor _Htp;
        IConfiguration _iconfiguration;

        public ENTITYTABLEController(IHostingEnvironment envrtmnt, IHttpContextAccessor htp, IConfiguration iconfiguration)
        {
            _Env = envrtmnt;
            _Htp = htp;
            _iconfiguration = iconfiguration;
        }

        /// Display master Grid Page
        /// Initial start point of displaying the Grid
        public ActionResult DisplayENTITYTABLE()
        {
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();

            ThisViewModel.TotalPages = 0;
            ThisViewModel.TotalRows = 0;
            ThisViewModel.CurrentPageNumber = 0;
            ThisViewModel.SortAscendingDescending = "DESC";  //default init setting

            ViewData.Model = ThisViewModel;

            return View("ENTITYTABLEGrid");   //must match View name
        }


        /// Display Tab Page
        public ActionResult ENTITYTABLETab()
        {
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();
            ViewData.Model = ThisViewModel;

            return View();  //goes to ScriptTab View by default
        }


        /// Display other Detail Tab Page
        public ActionResult ENTITYTABLEShow()
        {
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();
            ViewData.Model = ThisViewModel;

            return View();  //goes to ScriptTab View by default
        }



        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int Size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        private string RandomNumbersString(int size)
        {
            StringBuilder builder = new StringBuilder();
            int ch;
            for (int i = 0; i < size; i++)
            {
                ch = random.Next(9);
                builder.Append(ch);
            }

            return builder.ToString().Substring(0, size);
        }


        public JsonResult GenerateCodeCHILD()
        {
            bool returnStatus;
            string returnErrorMessage;
            //List<string> returnMessage;
            List<string> returnMessage = new List<string>();
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();

            try
            {

                //this.TryUpdateModel(ThisViewModel);
                this.TryUpdateModelAsync(ThisViewModel);
                //string projstr = ThisViewModel.PHY_NAME;
                string projstr = "";
                //string strDBname = ThisViewModel.dbname;
                //string strServername = ThisViewModel.servername;
                //string strwebconcombo = ThisViewModel.webconfigcombo;   //Database type to use (ie local, express or full)
                //string strapsxrazor = ThisViewModel.aspxrazorcombo;   //ASPX or RAZOR pages to be used
                //string stradoefcombo = ThisViewModel.adoefcombo;   //ADO or EF(Entity Framework 6)
                //string strvscombo = ThisViewModel.vscombo;   //Visual Studio Version to use
                string stradoefcombo = "";
                string progstr = "";
                string childtable1str = "";  //TOPINNERGRID table name

                //string progstr = "";
                string strpkid = "";
                string childprog1str = "";  //TOPINNERGRID prog name
                string childchildprogstr = "";    //INNERGRID prog name
                string strchild1pkid = "";
                string child1column1str = "";
                string childcolumn1type = "";
                string child1column2str = "";
                string childcolumn2type = "";
                string child1column3str = "";
                string childcolumn3type = "";
                string child1column4str = "";
                string childcolumn4type = "";

                //tablestr = ThisViewModel.tablename;
                progstr = ThisViewModel.ParentProgName;   //Parent prog
                //strpkid = ThisViewModel.ParentPKID;    //parent PKID  number works
                strpkid = ThisViewModel.ParentProgPKID;    //parent PKID
                projstr = ThisViewModel.ChildNumber;    //Child Number

                //column1str = ThisViewModel.column1;
                //column1type = ThisViewModel.column1type;
                //column2str = ThisViewModel.column2;
                //column2type = ThisViewModel.column2type;
                //column3str = ThisViewModel.column3;
                //column3type = ThisViewModel.column3type;
                //column4str = ThisViewModel.column4;
                //column4type = ThisViewModel.column4type;

                //use below
                childtable1str = ThisViewModel.tablename;   //OK
                childprog1str = ThisViewModel.progname;
                strchild1pkid = ThisViewModel.primarykey;
                child1column1str = ThisViewModel.column1;
                childcolumn1type = ThisViewModel.column1type;
                child1column2str = ThisViewModel.column2;
                childcolumn2type = ThisViewModel.column2type;
                child1column3str = ThisViewModel.column3;
                childcolumn3type = ThisViewModel.column3type;
                child1column4str = ThisViewModel.column4;
                childcolumn4type = ThisViewModel.column4type;

                //First check for Duplicate program name
                string chk = "";
                //string infile = "~/appAPP.csproj";
                //var FileName = HttpContext.Server.MapPath(infile);
                //var FileName = Path.Combine(_Env.ContentRootPath, infile);
                //var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string newline = childprog1str + "Controller.cs";

                string getContFolder = "Controllers/";
                string[] filescontfound = System.IO.Directory.GetFiles(Path.Combine(_Env.ContentRootPath, getContFolder));

                for (int i = 0; i < filescontfound.Length; i++)
                {
                    string ss = filescontfound[i];
                    if (ss.Contains(newline))
                    {
                        chk = "foundDuplicate";  //so found a duplicate
                    }
                }

                if (chk == "foundDuplicate")  //so found a duplicate, do not proceed
                {
                    returnStatus = false;
                    returnErrorMessage = "Program Name, is a Duplicate, Please rename it.";
                    returnMessage.Add(returnErrorMessage);

                }
                else
                {
                    //---------------------------------------------------------------------------------------------------------------------
                    //Create new View folder in this VS Solution - TOPINNER GRID  -  CHILD
                    //---------------------------------------------------------------------------------------------------------------------
                    string newViewFolder = "Views/" + childprog1str;
                    System.IO.Directory.CreateDirectory(Path.Combine(_Env.ContentRootPath, newViewFolder));

                    GenerateTOPINNERGRIDRAZNEW(projstr, progstr, childprog1str, strchild1pkid, child1column1str, childcolumn1type, child1column2str, childcolumn2type, child1column3str, childcolumn3type, child1column4str, childcolumn4type, childchildprogstr, strpkid, stradoefcombo, childtable1str);

                    returnStatus = true;
                    returnErrorMessage = "Done";
                    returnMessage.Add(returnErrorMessage);
                }



                ThisViewModel.ReturnMessage = returnMessage;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }
            catch (Exception ex)
            {
                List<string> outputMessages = new List<string>();

                returnStatus = false;
                outputMessages.Add(ex.Message);

                ThisViewModel.ReturnMessage = outputMessages;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }

        }


        public JsonResult GenerateCodeGRANDCHILD()
        {
            bool returnStatus;
            string returnErrorMessage;
            //List<string> returnMessage;
            List<string> returnMessage = new List<string>();
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();

            try
            {

                //this.TryUpdateModel(ThisViewModel);
                this.TryUpdateModelAsync(ThisViewModel);
                //string projstr = ThisViewModel.PHY_NAME;
                string projstr = "";
                //string strDBname = ThisViewModel.dbname;
                //string strServername = ThisViewModel.servername;
                //string strwebconcombo = ThisViewModel.webconfigcombo;   //Database type to use (ie local, express or full)
                //string strapsxrazor = ThisViewModel.aspxrazorcombo;   //ASPX or RAZOR pages to be used
                //string stradoefcombo = ThisViewModel.adoefcombo;   //ADO or EF(Entity Framework 6)
                //string strvscombo = ThisViewModel.vscombo;   //Visual Studio Version to use
                string stradoefcombo = "";
                string progstr = "";
                string childchildtablestr = "";   //INNERGRID table name

                //string progstr = "";
                string strpkid = "";
                string childprog1str = "";  //TOPINNERGRID prog name
                string childchildprogstr = "";    //INNERGRID prog name
                string strchild1pkid = "";
                string strchildchild1pkid = "";
                string childchild1column1str = "";
                string childchildcolumn1type = "";
                string childchild1column2str = "";
                string childchildcolumn2type = "";
                string childchild1column3str = "";
                string childchildcolumn3type = "";
                string childchild1column4str = "";
                string childchildcolumn4type = "";

                //tablestr = ThisViewModel.tablename;
                progstr = ThisViewModel.TopProgName;   //now top program name

                childprog1str = ThisViewModel.ParentProgName;   //Parent prog
                //strpkid = ThisViewModel.ParentPKID;    //parent PKID  number works
                strpkid = ThisViewModel.ParentProgPKID;    //parent PKID
                strchild1pkid = ThisViewModel.ParentProgPKID;    //parent PKID

                projstr = ThisViewModel.ChildNumber;    //Child Number

                //column1str = ThisViewModel.column1;
                //column1type = ThisViewModel.column1type;
                //column2str = ThisViewModel.column2;
                //column2type = ThisViewModel.column2type;
                //column3str = ThisViewModel.column3;
                //column3type = ThisViewModel.column3type;
                //column4str = ThisViewModel.column4;
                //column4type = ThisViewModel.column4type;

                //use below
                //childtable1str = ThisViewModel.tablename;   //OK
                //childprog1str = ThisViewModel.progname;
                //strchild1pkid = ThisViewModel.primarykey;
                //child1column1str = ThisViewModel.column1;
                //childcolumn1type = ThisViewModel.column1type;
                //child1column2str = ThisViewModel.column2;
                //childcolumn2type = ThisViewModel.column2type;
                //child1column3str = ThisViewModel.column3;
                //childcolumn3type = ThisViewModel.column3type;
                //child1column4str = ThisViewModel.column4;
                //childcolumn4type = ThisViewModel.column4type;


                //##################
                childchildtablestr = ThisViewModel.tablename;
                childchildprogstr = ThisViewModel.progname;
                strchildchild1pkid = ThisViewModel.primarykey;
                childchild1column1str = ThisViewModel.column1;
                childchildcolumn1type = ThisViewModel.column1type;
                childchild1column2str = ThisViewModel.column2;
                childchildcolumn2type = ThisViewModel.column2type;
                childchild1column3str = ThisViewModel.column3;
                childchildcolumn3type = ThisViewModel.column3type;
                childchild1column4str = ThisViewModel.column4;
                childchildcolumn4type = ThisViewModel.column4type;


                //First check for Duplicate program name
                string chk = "";
                //string infile = "~/appAPP.csproj";
                //var FileName = HttpContext.Server.MapPath(infile);
                //var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string newline = childchildprogstr + "Controller.cs";

                string getContFolder = "Controllers/";
                string[] filescontfound = System.IO.Directory.GetFiles(Path.Combine(_Env.ContentRootPath, getContFolder));

                for (int i = 0; i < filescontfound.Length; i++)
                {
                    string ss = filescontfound[i];
                    if (ss.Contains(newline))
                    {
                        chk = "foundDuplicate";  //so found a duplicate
                    }
                }

                if (chk == "foundDuplicate")  //so found a duplicate, do not proceed
                {
                    returnStatus = false;
                    returnErrorMessage = "Program Name, is a Duplicate, Please rename it.";
                    returnMessage.Add(returnErrorMessage);

                }
                else
                {
                    //---------------------------------------------------------------------------------------------------------------------
                    //Create new View folder in this VS Solution - TOPINNER GRID  -  CHILD
                    //---------------------------------------------------------------------------------------------------------------------
                    //string newViewFolder = "Views/" + childprog1str;
                    string newViewFolder = "Views/" + childchildprogstr;

                    Directory.CreateDirectory(Path.Combine(_Env.ContentRootPath, newViewFolder));

                    GenerateINNERGRIDRAZNEW(projstr, progstr, childprog1str, childchildprogstr, strchildchild1pkid, childchild1column1str, childchildcolumn1type, childchild1column2str, childchildcolumn2type, childchild1column3str, childchildcolumn3type, childchild1column4str, childchildcolumn4type, childchildprogstr, strchild1pkid, stradoefcombo, childchildtablestr);

                    returnStatus = true;
                    returnErrorMessage = "Done";
                    returnMessage.Add(returnErrorMessage);
                }



                ThisViewModel.ReturnMessage = returnMessage;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }
            catch (Exception ex)
            {
                List<string> outputMessages = new List<string>();

                returnStatus = false;
                outputMessages.Add(ex.Message);

                ThisViewModel.ReturnMessage = outputMessages;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }

        }



        //[ValidateAntiForgeryToken]
        public JsonResult GenerateCodeNEW()
        {
            bool returnStatus;
            string returnErrorMessage;
            //List<string> returnMessage;
            List<string> returnMessage = new List<string>();
            //PHYLUMBLL ThisBLL = new PHYLUMBLL();
            //Models.PHYLUMViewModel ThisViewModel = new Models.PHYLUMViewModel();
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();

            try
            {

                //test Random String
                //Random randomk = new Random((int)DateTime.Now.Ticks);
                //string rand1 = RandomString(5);
                //string rand2 = RandomNumbersString(4);  //4 digits that is unique. using this method for progname suffix
                //string rand3 = System.IO.Path.GetRandomFileName().Replace(".", "");   //this is 8 chars long, could use it

                //Random rand = new Random((int)DateTime.Now.Ticks);
                //int randomIndex1 = rand.Next(100, 999);
                //int randomIndex2 = rand.Next(10000, 99999);
                //string randstring = RandomString(3);
                //string result = "#" + randstring + "-" + randomIndex1.ToString() + "-" + randomIndex2.ToString();


                //string rand3 = RandomString(5);
                //string randit = rand1 + rand2 + rand3;

                //this.TryUpdateModel(ThisViewModel);
                this.TryUpdateModelAsync(ThisViewModel);
                //string projstr = ThisViewModel.PHY_NAME;
                string projstr = "";
                //string strDBname = ThisViewModel.dbname;
                //string strServername = ThisViewModel.servername;
                //string strwebconcombo = ThisViewModel.webconfigcombo;   //Database type to use (ie local, express or full)
                //string strapsxrazor = ThisViewModel.aspxrazorcombo;   //ASPX or RAZOR pages to be used
                //string stradoefcombo = ThisViewModel.adoefcombo;   //ADO or EF(Entity Framework 6)
                //string strvscombo = ThisViewModel.vscombo;   //Visual Studio Version to use
                string stradoefcombo = "";
                string progstr = "";
                string tablestr = "";   //new  TOPGRID table

                //string progstr = "";
                string strpkid = "";
                string column1str = "";
                string column1type = "";
                string column2str = "";
                string column2type = "";
                string column3str = "";
                string column3type = "";
                string column4str = "";
                string column4type = "";
                string childprog2str = "Child";  //another TOPINNERGRID table name
                string childprog1str = "";  //TOPINNERGRID prog name
                string comboprogstr = "";
                string strcomboclicked = "false"; //COMBOGRID flagged(checked) to be used - within TOPGRID Detail page
                string childchildprogstr = "";    //INNERGRID prog name
                string strchild1pkid = "";
                string strchildchild1pkid = "";

                tablestr = ThisViewModel.tablename;
                progstr = ThisViewModel.progname;
                //progstr = ThisViewModel.tablename + "7";
                strpkid = ThisViewModel.primarykey;
                column1str = ThisViewModel.column1;
                column1type = ThisViewModel.column1type;
                column2str = ThisViewModel.column2;
                column2type = ThisViewModel.column2type;
                column3str = ThisViewModel.column3;
                column3type = ThisViewModel.column3type;
                column4str = ThisViewModel.column4;
                column4type = ThisViewModel.column4type;

                //First check for Duplicate program name
                string chk = "";
                string infile = "appAPP.csproj";
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);


                string newline = progstr + "Controller.cs";

                string getContFolder = "Controllers/";
                string[] filescontfound = System.IO.Directory.GetFiles(Path.Combine(_Env.ContentRootPath, getContFolder));

                for (int i = 0; i < filescontfound.Length; i++)
                {
                    string ss = filescontfound[i];
                    if (ss.Contains(newline))
                    //if (ss == newline)
                    {
                        chk = "foundDuplicate";  //so found a duplicate
                    }
                }
                //StreamReader sr = System.IO.File.OpenText(FileName);
                //while ((old = sr.ReadLine()) != null)   //read all lines
                //{
                // if (old.Contains(newline))
                // {
                //do not add new line now
                //n += newline + Environment.NewLine;   //add new line, after found target line
                //  chk = "foundDuplicate";  //so found a duplicate
                // }
                // else
                // {
                //n += old + Environment.NewLine;
                // }

                //}
                //sr.Close();
                //System.IO.File.WriteAllText(FileName, n);


                if (chk == "foundDuplicate")  //so found a duplicate, do not proceed
                {
                    returnStatus = false;
                    returnErrorMessage = "Program Name, is a Duplicate, Please rename it.";
                    returnMessage.Add(returnErrorMessage);

                }
                else
                {
                    //---------------------------------------------------------------------------------------------------------------------
                    //Create new View folder in this VS Solution - TOPGRID - there always be a TOPGRID
                    //---------------------------------------------------------------------------------------------------------------------
                    string newViewFolder = "Views/" + progstr;
                    //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CreateDirectory(HttpContext.Server.MapPath(newViewFolder));
                    //Directory.CreateDirectory(Path.Combine(_Env.ContentRootPath, newViewFolder));
                    System.IO.Directory.CreateDirectory(Path.Combine(_Env.ContentRootPath, newViewFolder));

                    GenerateTOPGRIDRAZNEW(projstr, progstr, strpkid, column1str, column1type, column2str, column2type, column3str, column3type, column4str, column4type, childprog1str, childprog2str, strcomboclicked, comboprogstr, stradoefcombo, tablestr, strchild1pkid, childchildprogstr, strchildchild1pkid);

                    returnStatus = true;
                    returnErrorMessage = "Done";
                    returnMessage.Add(returnErrorMessage);
                }



                ThisViewModel.ReturnMessage = returnMessage;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }
            catch (Exception ex)
            {
                List<string> outputMessages = new List<string>();

                returnStatus = false;
                outputMessages.Add(ex.Message);

                ThisViewModel.ReturnMessage = outputMessages;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }

        }


        //KAL7
        public JsonResult GenerateCodeDEL()
        {
            bool returnStatus;
            string returnErrorMessage;
            //List<string> returnMessage;
            List<string> returnMessage = new List<string>();
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();

            try
            {

                this.TryUpdateModelAsync(ThisViewModel);
                string progstr = "";
                //int readcounter = 0;

                //tablestr = ThisViewModel.tablename;
                //progstr = ThisViewModel.tablename + "7";
                progstr = ThisViewModel.progname;

                /*
                string newViewFolder = "Views/" + progstr;
                Directory.Delete(newViewFolder, true);

                string jsdelit = "wwwroot/js/" + progstr + "Results.js";
                System.IO.File.Delete(jsdelit);

                jsdelit = "wwwroot/js/" + progstr + ".js";
                System.IO.File.Delete(jsdelit);

                jsdelit = "wwwroot/js/" + progstr + "ShowDetail.js";
                System.IO.File.Delete(jsdelit);

                string controldelit = "Controllers/" + progstr + "Controller.cs";
                System.IO.File.Delete(controldelit);

                string modelsdelit = "Models/" + progstr + "ViewModel.cs";
                System.IO.File.Delete(modelsdelit);

                string BLLdelit = "" + progstr + "BLL.cs";
                System.IO.File.Delete(BLLdelit);

                string MODdelit = "" + progstr + "Mod.cs";
                System.IO.File.Delete(MODdelit);

                */


                string newViewFolder = "Views/" + progstr;
                var ViewFolderDelk = Path.Combine(_Env.ContentRootPath, newViewFolder);
                Directory.Delete(ViewFolderDelk, true);

                string jsdelit = "wwwroot/js/" + progstr + "Results.js";
                var jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                jsdelit = "wwwroot/js/" + progstr + ".js";
                jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                jsdelit = "wwwroot/js/" + progstr + "ShowDetail.js";
                jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                string controldelit = "Controllers/" + progstr + "Controller.cs";
                var controldelitk = Path.Combine(_Env.ContentRootPath, controldelit);
                System.IO.File.Delete(controldelitk);

                string modelsdelit = "Models/" + progstr + "ViewModel.cs";
                var modelsdelitk = Path.Combine(_Env.ContentRootPath, modelsdelit);
                System.IO.File.Delete(modelsdelitk);

                string BLLdelit = "" + progstr + "BLL.cs";
                var BLLdelitk = Path.Combine(_Env.ContentRootPath, BLLdelit);
                System.IO.File.Delete(BLLdelitk);

                string MODdelit = "" + progstr + "Mod.cs";
                var MODdelitk = Path.Combine(_Env.ContentRootPath, MODdelit);
                System.IO.File.Delete(MODdelitk);


                returnStatus = true;
                returnErrorMessage = "Done";
                returnMessage.Add(returnErrorMessage);

                ThisViewModel.ReturnMessage = returnMessage;
                ThisViewModel.ReturnStatus = returnStatus;

                //var modelJson = JsonSerializer.Serialize(ThisViewModel);
                //var modelJsonj = System.Text.Json.Serialization.JsonConverterFactory(ThisViewModel);

                //var modelJsonk = JsonSerializer.Deserialize<ThisViewModel>(jsonString, null);
                //return Json(ThisViewModel);
                return new JsonResult(ThisViewModel);
                //return modelJson;

            }
            catch (Exception ex)
            {
                List<string> outputMessages = new List<string>();

                returnStatus = false;
                outputMessages.Add(ex.Message);

                ThisViewModel.ReturnMessage = outputMessages;
                ThisViewModel.ReturnStatus = returnStatus;

                //return Json(ThisViewModel);
                return new JsonResult(ThisViewModel);

            }

        }


        public JsonResult GenerateCodeDELChild()
        {
            bool returnStatus;
            string returnErrorMessage;
            //List<string> returnMessage;
            List<string> returnMessage = new List<string>();
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();

            try
            {

                this.TryUpdateModelAsync(ThisViewModel);
                string progstr = "";
                string currentChild = "";
                //int readcounter = 0;
                string parenterprogstr = "";

                //tablestr = ThisViewModel.tablename;
                //progstr = ThisViewModel.tablename + "7";
                progstr = ThisViewModel.CurrentProgName;
                currentChild = ThisViewModel.CurrentChildNo;
                parenterprogstr = ThisViewModel.ParenterProgName;

                /*
                string newViewFolder = "Views/" + progstr;
                Directory.Delete(newViewFolder, true);

                string jsdelit = "wwwroot/js/" + progstr + "Results.js";
                System.IO.File.Delete(jsdelit);

                jsdelit = "wwwroot/js/" + progstr + ".js";
                System.IO.File.Delete(jsdelit);

                jsdelit = "wwwroot/js/" + progstr + "ShowDetail.js";
                System.IO.File.Delete(jsdelit);

                string controldelit = "Controllers/" + progstr + "Controller.cs";
                System.IO.File.Delete(controldelit);

                string modelsdelit = "Models/" + progstr + "ViewModel.cs";
                System.IO.File.Delete(modelsdelit);

                string BLLdelit = "" + progstr + "BLL.cs";
                System.IO.File.Delete(BLLdelit);

                string MODdelit = "" + progstr + "Mod.cs";
                System.IO.File.Delete(MODdelit);

                */

                string newViewFolder = "Views/" + progstr;
                var ViewFolderDelk = Path.Combine(_Env.ContentRootPath, newViewFolder);
                Directory.Delete(ViewFolderDelk, true);

                string jsdelit = "wwwroot/js/" + progstr + "Results.js";
                var jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                jsdelit = "wwwroot/js/" + progstr + ".js";
                jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                jsdelit = "wwwroot/js/" + progstr + "ShowDetail.js";
                jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                string controldelit = "Controllers/" + progstr + "Controller.cs";
                var controldelitk = Path.Combine(_Env.ContentRootPath, controldelit);
                System.IO.File.Delete(controldelitk);

                string modelsdelit = "Models/" + progstr + "ViewModel.cs";
                var modelsdelitk = Path.Combine(_Env.ContentRootPath, modelsdelit);
                System.IO.File.Delete(modelsdelitk);

                string BLLdelit = "" + progstr + "BLL.cs";
                var BLLdelitk = Path.Combine(_Env.ContentRootPath, BLLdelit);
                System.IO.File.Delete(BLLdelitk);

                string MODdelit = "" + progstr + "Mod.cs";
                var MODdelitk = Path.Combine(_Env.ContentRootPath, MODdelit);
                System.IO.File.Delete(MODdelitk);



                //change childshow & childTab & prog.cshtml files
                //Edit Parent existing progShow.cshtml 
                string parentProgShowk = "Views/" + parenterprogstr + "/" + parenterprogstr + "Show.cshtml";  //parent progShow.cshtml
                var FileNameparentprogShowk = Path.Combine(_Env.ContentRootPath, parentProgShowk);

                string oldparentprogShowk;
                string nparentprogShowk = "";
                string search_textChild1 = "";
                //string newlineShow = "";
                //string childstr = "";
                //int firstsearchcount = 0;
                StreamReader srparentprogShowk = System.IO.File.OpenText(FileNameparentprogShowk);
                while ((oldparentprogShowk = srparentprogShowk.ReadLine()) != null)   //read all lines
                {
                    oldparentprogShowk = oldparentprogShowk.Replace(parenterprogstr + "Show" + progstr + "", parenterprogstr + "Show" + "~" + progstr + "~");

                    search_textChild1 = parenterprogstr + "Show" + currentChild;
                    if (oldparentprogShowk.Contains(search_textChild1))  //do not keep this line in file
                    {
                        oldparentprogShowk = "";   //blank out line, as not needed now
                    }

                    nparentprogShowk += oldparentprogShowk + Environment.NewLine;


                }
                srparentprogShowk.Close();
                System.IO.File.WriteAllText(FileNameparentprogShowk, nparentprogShowk);



                //Try and do Tab.cshtml, if exists (not in Child or granChilds)
                try
                {
                    //Edit Parent existing progTab.cshtml 
                    var parentProgTabk = "Views/" + parenterprogstr + "/" + parenterprogstr + "Tab.cshtml";  //parent progTab.cshtml
                    var FileNameparentprogTabk = Path.Combine(_Env.ContentRootPath, parentProgTabk);
                    //var FileNameparentprogTabk = HttpContext.Server.MapPath(parentProgTabk);

                    string oldparentprogTabk;
                    string nparentprogTabk = "";
                    string search_textTabChild1 = "";
                    //newlineShow = "";
                    //string childstr = "";
                    //int firstsearchcount = 0;
                    StreamReader srparentprogTabkl = System.IO.File.OpenText(FileNameparentprogTabk);
                    while ((oldparentprogTabk = srparentprogTabkl.ReadLine()) != null)   //read all lines
                    {
                        oldparentprogTabk = oldparentprogTabk.Replace(parenterprogstr + "Tab" + progstr + "", parenterprogstr + "Tab" + "~" + progstr + "~");

                        search_textTabChild1 = parenterprogstr + "Tab" + currentChild;
                        if (oldparentprogTabk.Contains(search_textTabChild1))  //do not keep this line in file
                        {
                            oldparentprogTabk = "";   //blank out line, as not needed now
                        }

                        nparentprogTabk += oldparentprogTabk + Environment.NewLine;


                    }
                    srparentprogTabkl.Close();
                    System.IO.File.WriteAllText(FileNameparentprogTabk, nparentprogTabk);

                }
                catch (Exception)
                {
                    //alert("err message " + ex.message);
                }



                //Edit Parent existing prog.cshtml 
                var parentProgk = "Views/" + parenterprogstr + "/" + parenterprogstr + ".cshtml";  //top prog.cshtml
                //var FileNameparentprogk = HttpContext.Server.MapPath(parentProgk);
                var FileNameparentprogk = Path.Combine(_Env.ContentRootPath, parentProgk);

                string oldparentprogk;
                string nparentprogk = "";
                //string search_textChild1prog = "";
                //string newlineShow = "";
                //string childstr = "";
                //int firstsearchcount = 0;
                StreamReader srparentprogk = System.IO.File.OpenText(FileNameparentprogk);
                while ((oldparentprogk = srparentprogk.ReadLine()) != null)   //read all lines
                {
                    oldparentprogk = oldparentprogk.Replace("Overlay" + progstr + "Detail", "Overlay" + "~" + progstr + "~Detail");

                    //search_textChild1 = parenterprogstr + "Show" + currentChild;
                    //if (oldparentprogShowk.Contains(search_textChild1))  //do not keep this line in file
                    //{
                    //oldparentprogShowk = "";   //blank out line, as not needed now
                    //}

                    nparentprogk += oldparentprogk + Environment.NewLine;


                }
                srparentprogk.Close();
                System.IO.File.WriteAllText(FileNameparentprogk, nparentprogk);




                returnStatus = true;
                returnErrorMessage = "Done";
                returnMessage.Add(returnErrorMessage);

                ThisViewModel.ReturnMessage = returnMessage;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }
            catch (Exception ex)
            {
                List<string> outputMessages = new List<string>();

                returnStatus = false;
                outputMessages.Add(ex.Message);

                ThisViewModel.ReturnMessage = outputMessages;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }

        }


        public JsonResult GenerateCodeDELGrandChild()
        {
            bool returnStatus;
            string returnErrorMessage;
            //List<string> returnMessage;
            List<string> returnMessage = new List<string>();
            Models.ENTITYTABLEViewModel ThisViewModel = new Models.ENTITYTABLEViewModel();

            try
            {

                //this.TryUpdateModel(ThisViewModel);
                this.TryUpdateModelAsync(ThisViewModel);
                string progstr = "";
                string currentChild = "";
                //int readcounter = 0;
                string parenterprogstr = "";
                string toperprogstr = "";

                //tablestr = ThisViewModel.tablename;
                //progstr = ThisViewModel.tablename + "7";
                progstr = ThisViewModel.CurrentProgName;
                currentChild = ThisViewModel.CurrentChildNo;
                parenterprogstr = ThisViewModel.ParenterProgName;
                toperprogstr = ThisViewModel.ToperProgName;  //for Grand Child deletion ONLY


                //string newViewFolder = "~/Views/" + progstr;

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteDirectory(HttpContext.Server.MapPath(newViewFolder),
                // Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);

                //string jsdelit = "~/Scripts/" + progstr + "Results.js";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(jsdelit));

                //jsdelit = "~/Scripts/" + progstr + ".js";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(jsdelit));

                //jsdelit = "~/Scripts/" + progstr + "ShowDetail.js";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(jsdelit));


                //string controldelit = "~/Controllers/" + progstr + "Controller.cs";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(controldelit));


                //string modelsdelit = "~/Models/" + progstr + "ViewModel.cs";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(modelsdelit));


                //string BLLdelit = "~/" + progstr + "BLL.cs";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(BLLdelit));


                //string MODdelit = "~/" + progstr + "Mod.cs";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(MODdelit));



                string newViewFolder = "Views/" + progstr;
                var ViewFolderDelk = Path.Combine(_Env.ContentRootPath, newViewFolder);
                Directory.Delete(ViewFolderDelk, true);
              
                string jsdelit = "wwwroot/js/" + progstr + "Results.js";
                var jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                jsdelit = "wwwroot/js/" + progstr + ".js";
                jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                jsdelit = "wwwroot/js/" + progstr + "ShowDetail.js";
                jsdelitk = Path.Combine(_Env.ContentRootPath, jsdelit);
                System.IO.File.Delete(jsdelitk);

                string controldelit = "Controllers/" + progstr + "Controller.cs";
                var controldelitk = Path.Combine(_Env.ContentRootPath, controldelit);
                System.IO.File.Delete(controldelitk);

                string modelsdelit = "Models/" + progstr + "ViewModel.cs";
                var modelsdelitk = Path.Combine(_Env.ContentRootPath, modelsdelit);
                System.IO.File.Delete(modelsdelitk);

                string BLLdelit = "" + progstr + "BLL.cs";
                var BLLdelitk = Path.Combine(_Env.ContentRootPath, BLLdelit);
                System.IO.File.Delete(BLLdelitk);

                string MODdelit = "" + progstr + "Mod.cs";
                var MODdelitk = Path.Combine(_Env.ContentRootPath, MODdelit);
                System.IO.File.Delete(MODdelitk);




                //change childshow & childTab & prog.cshtml files
                //Edit Parent existing progShow.cshtml 
                var parentProgShowk = "Views/" + parenterprogstr + "/" + parenterprogstr + "Show.cshtml";  //parent progShow.cshtml
                //var FileNameparentprogShowk = HttpContext.Server.MapPath(parentProgShowk);
                var FileNameparentprogShowk = Path.Combine(_Env.ContentRootPath, parentProgShowk);

                string oldparentprogShowk;
                string nparentprogShowk = "";
                string search_textChild1 = "";
                //string newlineShow = "";
                //string childstr = "";
                //int firstsearchcount = 0;
                StreamReader srparentprogShowk = System.IO.File.OpenText(FileNameparentprogShowk);
                while ((oldparentprogShowk = srparentprogShowk.ReadLine()) != null)   //read all lines
                {
                    oldparentprogShowk = oldparentprogShowk.Replace(parenterprogstr + "Show" + progstr + "", parenterprogstr + "Show" + "~" + progstr + "~");

                    search_textChild1 = parenterprogstr + "Show" + currentChild;
                    if (oldparentprogShowk.Contains(search_textChild1))  //do not keep this line in file
                    {
                        oldparentprogShowk = "";   //blank out line, as not needed now
                    }

                    nparentprogShowk += oldparentprogShowk + Environment.NewLine;


                }
                srparentprogShowk.Close();
                System.IO.File.WriteAllText(FileNameparentprogShowk, nparentprogShowk);



                //Try and do Tab.cshtml, if exists (not in Child or granChilds)
                try
                {
                    //Edit Parent existing progTab.cshtml 
                    var parentProgTabk = "Views/" + parenterprogstr + "/" + parenterprogstr + "Tab.cshtml";  //parent progTab.cshtml
                    //var FileNameparentprogTabk = HttpContext.Server.MapPath(parentProgTabk);
                    var FileNameparentprogTabk = Path.Combine(_Env.ContentRootPath, parentProgTabk);

                    string oldparentprogTabk;
                    string nparentprogTabk = "";
                    string search_textTabChild1 = "";
                    //newlineShow = "";
                    //string childstr = "";
                    //int firstsearchcount = 0;
                    StreamReader srparentprogTabkl = System.IO.File.OpenText(FileNameparentprogTabk);
                    while ((oldparentprogTabk = srparentprogTabkl.ReadLine()) != null)   //read all lines
                    {
                        oldparentprogTabk = oldparentprogTabk.Replace(parenterprogstr + "Tab" + progstr + "", parenterprogstr + "Tab" + "~" + progstr + "~");

                        search_textTabChild1 = parenterprogstr + "Tab" + currentChild;
                        if (oldparentprogTabk.Contains(search_textTabChild1))  //do not keep this line in file
                        {
                            oldparentprogTabk = "";   //blank out line, as not needed now
                        }

                        nparentprogTabk += oldparentprogTabk + Environment.NewLine;


                    }
                    srparentprogTabkl.Close();
                    System.IO.File.WriteAllText(FileNameparentprogTabk, nparentprogTabk);

                }
                catch (Exception)
                {
                    //alert("err message " + ex.message);
                }



                //Edit Parent existing prog.cshtml 
                var parentProgk = "Views/" + toperprogstr + "/" + toperprogstr + ".cshtml";  //top prog.cshtml
                //var FileNameparentprogk = HttpContext.Server.MapPath(parentProgk);
                var FileNameparentprogk = Path.Combine(_Env.ContentRootPath, parentProgk);

                string oldparentprogk;
                string nparentprogk = "";
                //string search_textChild1prog = "";
                //string newlineShow = "";
                //string childstr = "";
                //int firstsearchcount = 0;
                StreamReader srparentprogk = System.IO.File.OpenText(FileNameparentprogk);
                while ((oldparentprogk = srparentprogk.ReadLine()) != null)   //read all lines
                {
                    oldparentprogk = oldparentprogk.Replace("Overlay" + progstr + "Detail", "Overlay" + "~" + progstr + "~Detail");

                    //search_textChild1 = parenterprogstr + "Show" + currentChild;
                    //if (oldparentprogShowk.Contains(search_textChild1))  //do not keep this line in file
                    //{
                    //oldparentprogShowk = "";   //blank out line, as not needed now
                    //}

                    nparentprogk += oldparentprogk + Environment.NewLine;


                }
                srparentprogk.Close();
                System.IO.File.WriteAllText(FileNameparentprogk, nparentprogk);




                returnStatus = true;
                returnErrorMessage = "Done";
                returnMessage.Add(returnErrorMessage);

                ThisViewModel.ReturnMessage = returnMessage;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }
            catch (Exception ex)
            {
                List<string> outputMessages = new List<string>();

                returnStatus = false;
                outputMessages.Add(ex.Message);

                ThisViewModel.ReturnMessage = outputMessages;
                ThisViewModel.ReturnStatus = returnStatus;

                return Json(ThisViewModel);

            }

        }




        public bool CheckDupTableName(long PHYID, string sectorName, out bool returnStatus,
            out string returnErrorMessage)
        {

            try
            {
                bool chkname = false;

                StringBuilder sqlBuilder = new StringBuilder();

                string sqlString = "SELECT sector_name, COUNT(1) as CNT FROM ENTITYTABLE WHERE PHY_ID = @PK_ID AND sector_name = @SectName group by sector_name";

                UtilitiesBLL UtilitiesBLLget = new UtilitiesBLL();
                SqlConnection connection = UtilitiesBLLget.CreateConnectionRMSPROD(out returnStatus, out returnErrorMessage); //SQL Server

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = sqlString;

                SqlParameter param1 = new SqlParameter("@PK_ID", SqlDbType.BigInt);
                param1.Value = PHYID;
                //param1.Value = 312;
                sqlCommand.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@SectName", SqlDbType.Char);
                param2.Value = sectorName;
                sqlCommand.Parameters.Add(param2);


                SqlDataReader dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read() == true)
                {
                    if (Convert.ToInt32(dataReader["CNT"]) > 1)
                    {
                        chkname = true;
                    }

                }

                connection.Close();

                returnStatus = chkname;
                returnErrorMessage = "Error";

                return chkname;
                //return true;

            }
            catch (Exception ex)
            {
                returnStatus = false;
                returnErrorMessage = ex.Message;

                return false;

            }


        }


        private void GenerateTOPGRIDRAZNEW(string projstr, string progstr, string strpkid, string column1str, string column1type, string column2str, string column2type, string column3str, string column3type, string column4str, string column4type, string childprog1str, string childprog2str, string strcomboclicked, string comboprogstr, string stradoefcombo, string tablestr, string strchild1pkid, string childchildprogstr, string strchildchild1pkid)
        {

            try
            {
                string inputDecrpt = "";
                string outputDecrpt = "";
                string text = "";
                string newcsproj = "";

                //--------------------------------------------------------------------------------------------------------------------
                //TOP GRID
                //--------------------------------------------------------------------------------------------------------------------

                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progGrid.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progGrid.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                ConfigureColumn("Gen_Code/TOPGRID/progGrid.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/progGrid.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/progGrid.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/progGrid.cshtml", "col4", column4type);

                string progrealname = "Gen_Code/TOPGRID/" + progstr + "Grid.cshtml";
                string outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                text = System.IO.File.ReadAllText(outputDecrpt);

                //text = text.Replace("~proj~", projstr);   //do need now, as using appAPP all the time
                text = text.Replace("~prog~", progstr);
                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                string destfolderViews = "Views/" + progstr + "/" + progstr + "Grid.cshtml";
                string outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------



                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progResults.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progResults.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                ConfigureColumn("Gen_Code/TOPGRID/progResults.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/progResults.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/progResults.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/progResults.cshtml", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + "Results.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                //text = text.Replace("~tablemodel~", tablestr);
                text = text.Replace("~PK_ID~", strpkid);
                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + progstr + "/" + progstr + "Results.cshtml";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------



                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progResults.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progResults.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPGRID/progResults.js", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/progResults.js", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/progResults.js", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/progResults.js", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + "Results.js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                text = text.Replace("~PK_ID~", strpkid);

                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + progstr + "Results.js";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------





                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/prog.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/prog.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPGRID/prog.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/prog.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/prog.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/prog.cshtml", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + ".cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                //text = text.Replace("~prog~", progstr);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~PK_ID~", strpkid);
                //text = text.Replace("~PK_ID~", strpkid);
                text = text.Replace("~childprog~", childprog1str);   //Child progname
                //text = text.Replace("~childPK_ID~", strchild1pkid);   //Child PKID
                text = text.Replace("~childchildprog~", childchildprogstr);   //Grand Child progname
                //text = text.Replace("~childchildPK_ID~", strchildchild1pkid);   //Grand Child PKID
                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + progstr + "/" + progstr + ".cshtml";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/prog.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/prog.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPGRID/prog.js", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/prog.js", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/prog.js", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/prog.js", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + ".js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                text = text.Replace("~PK_ID~", strpkid);

                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + progstr + ".js";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progDetail.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progDetail.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPGRID/" + progstr + "Detail.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + progstr + "/" + progstr + "Detail.cshtml";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progController.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progController.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPGRID/" + progstr + "Controller.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                text = text.Replace("~tablemodel~", tablestr);
                text = text.Replace("~table~", tablestr);
                text = text.Replace("~PK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Controllers/" + progstr + "Controller.cs";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/projMod.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/projMod.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPGRID/projMod.cs", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/projMod.cs", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/projMod.cs", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/projMod.cs", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + "Mod.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                text = System.IO.File.ReadAllText(outputDecrpt);

                text = text.Replace("~prog~", progstr);
                text = text.Replace("~tablemodel~", tablestr);
                //text = text.Replace("~table~", progstr);
                text = text.Replace("~PK_ID~", strpkid);
                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "" + progstr + "Mod.cs";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------





                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progViewModel.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progViewModel.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPGRID/" + progstr + "ViewModel.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                text = text.Replace("~tablemodel~", tablestr);
                //text = text.Replace("~table~", tablestr);
                //text = text.Replace("~PK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Models/" + progstr + "ViewModel.cs";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------





                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progBLL.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progBLL.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPGRID/progBLL.cs", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/progBLL.cs", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/progBLL.cs", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/progBLL.cs", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + "BLL.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                text = System.IO.File.ReadAllText(outputDecrpt);

                text = text.Replace("~prog~", progstr);
                text = text.Replace("~tablemodel~", tablestr);
                text = text.Replace("~table~", tablestr);
                text = text.Replace("~PK_ID~", strpkid);
                text = text.Replace("~column1type~", column1type);
                text = text.Replace("~column2type~", column2type);
                text = text.Replace("~column3type~", column3type);
                text = text.Replace("~column4type~", column4type);

                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "" + progstr + "BLL.cs";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                //inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progTab.czhtml");
                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progTabStart.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progTab.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPGRID/" + progstr + "Tab.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                //text = text.Replace("~tablemodel~", tablestr);
                //text = text.Replace("~table~", tablestr);
                text = text.Replace("~PK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + progstr + "/" + progstr + "Tab.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                //inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progShow.czhtml");
                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progShowStart.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progShow.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPGRID/" + progstr + "Show.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                //text = text.Replace("~tablemodel~", tablestr);
                //text = text.Replace("~table~", tablestr);
                text = text.Replace("~PK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + progstr + "/" + progstr + "Show.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------






                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progShowDetail.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progShowDetail.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.cshtml", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + "ShowDetail.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                //text = text.Replace("~tablemodel~", tablestr);
                //text = text.Replace("~table~", tablestr);
                text = text.Replace("~PK_ID~", strpkid);
                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                //if (strcomboclicked.Trim() == "true")   //clicked on TOP COMBO checkbox
                //{
                //text = text.Replace("~combotable~", comboprogstr);   //if using COMBO
                //text = text.Replace("~combobool~", "true");   //if using COMBO
                //}

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + progstr + "/" + progstr + "ShowDetail.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progShowDetail.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPGRID/progShowDetail.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.js", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.js", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.js", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPGRID/progShowDetail.js", "col4", column4type);

                progrealname = "Gen_Code/TOPGRID/" + progstr + "ShowDetail.js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", progstr);
                text = text.Replace("~PK_ID~", strpkid);

                text = text.Replace("~column1~", column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + progstr + "ShowDetail.js";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                string kprog1 = progstr;
                string kprog2 = "Display" + progstr;
                //newcsproj = "<li>@Html.ActionLink(" + "\"" + kprog1 + "\"" + "," + "\"" + kprog2 + "\"" + "," + "\"" + kprog1 + "\"" + ")</li>";
                newcsproj = "<li>@Html.ActionLink(" + "\"" + kprog1 + "\"" + "," + "\"" + kprog2 + "\"" + "," + "\"" + kprog1 + "\"" + ", null, new { onclick = " + "\"" + "return LayoutShowProgressBar();" + "\"" + " })</li>";
                UpdateAHIMproj("Views/Shared/" + "_Layout.cshtml", "ADDLINKHERE", newcsproj);


                //END Bundling
                //-----------------------------------

                /*
            //--------------------------------------------------------------------------------------------------------------------
            //TOP GRID
            //--------------------------------------------------------------------------------------------------------------------

            */

            }
            catch (Exception ex)
            {

                string err = ex.Message;
            }

        }


        private void GenerateTOPINNERGRIDRAZNEW(string projstr, string progstr, string childprog1str, string strchild1pkid, string child1column1str, string column1type, string child1column2str, string column2type, string child1column3str, string column3type, string child1column4str, string column4type, string childchildprogstr, string strpkid, string stradoefcombo, string childtable1str)
        {

            try
            {

                string inputDecrpt = "";
                string outputDecrpt = "";
                //--------------------------------------------------------------------------------------------------------------------
                //TOPINNERGRID
                //--------------------------------------------------------------------------------------------------------------------

                //Edit Parent existing prog.cshtml  (ie replace ~childprog~ with progstr)
                //var parentProgk = HttpContext.Server.MapPath("~/Views/" + progstr + "/" + progstr + ".cshtml");  //parent prog.cshtml
                var parentProgk = "Views/" + progstr + "/" + progstr + ".cshtml";  //parent prog.cshtml
                //var FileNameparentprogk = HttpContext.Server.MapPath(parentProgk);
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileNameparentprogk = Path.Combine(_Env.ContentRootPath, parentProgk);

                string oldparentprogk;
                string nparentprogk = "";
                StreamReader srparentprogk = System.IO.File.OpenText(FileNameparentprogk);
                while ((oldparentprogk = srparentprogk.ReadLine()) != null)   //read all lines
                {

                    if (projstr == "Child1")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child1proguse~", childprog1str);
                    }
                    if (projstr == "Child2")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child2proguse~", childprog1str);
                    }
                    if (projstr == "Child3")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child3proguse~", childprog1str);
                    }
                    if (projstr == "Child4")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child4proguse~", childprog1str);
                    }
                    if (projstr == "Child5")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child5proguse~", childprog1str);
                    }
                    if (projstr == "Child6")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child6proguse~", childprog1str);
                    }
                    if (projstr == "Child7")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child7proguse~", childprog1str);
                    }
                    if (projstr == "Child8")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child8proguse~", childprog1str);
                    }

                    nparentprogk += oldparentprogk + Environment.NewLine;

                }
                srparentprogk.Close();
                System.IO.File.WriteAllText(FileNameparentprogk, nparentprogk);



                string nextchildstrindx = "";
                string nextindxuse = "";
                if (projstr == "Child1")
                {
                    nextchildstrindx = "Child2";
                    nextindxuse = "1";
                }
                if (projstr == "Child2")
                {
                    nextchildstrindx = "Child3";
                    nextindxuse = "2";
                }
                if (projstr == "Child3")
                {
                    nextchildstrindx = "Child4";
                    nextindxuse = "3";
                }
                if (projstr == "Child4")
                {
                    nextchildstrindx = "Child5";
                    nextindxuse = "4";
                }
                if (projstr == "Child5")
                {
                    nextchildstrindx = "Child6";
                    nextindxuse = "5";
                }
                if (projstr == "Child6")
                {
                    nextchildstrindx = "Child7";
                    nextindxuse = "6";
                }
                if (projstr == "Child7")
                {
                    nextchildstrindx = "Child8";
                    nextindxuse = "7";
                }

                //Edit Parent existing progShow.cshtml 
                var parentProgShowk = "Views/" + progstr + "/" + progstr + "Show.cshtml";  //parent prog.cshtml
                //var FileNameparentprogShowk = HttpContext.Server.MapPath(parentProgShowk);
                var FileNameparentprogShowk = Path.Combine(_Env.ContentRootPath, parentProgShowk);

                string oldparentprogShowk;
                string nparentprogShowk = "";
                string search_textChild1 = "";
                string newlineShow = "";
                string childstr = "";
                StreamReader srparentprogShowk = System.IO.File.OpenText(FileNameparentprogShowk);
                while ((oldparentprogShowk = srparentprogShowk.ReadLine()) != null)   //read all lines
                {

                    oldparentprogShowk = oldparentprogShowk.Replace("~childprog" + nextindxuse + "~", childprog1str);

                    search_textChild1 = progstr + "ShowStart" + projstr;
                    if (oldparentprogShowk.Contains(search_textChild1))  //do not keep this line in file
                    {
                        // n += newline + Environment.NewLine;   //add new line, after found target line
                        //Add new lines for show child and add new child tabs
                        newlineShow = "<button class=" + "\"" + progstr + "Showtablinks" + "\"" + " ";
                        newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowOpenTab(event, '" + progstr + "Show" + childprog1str + "', '" + childprog1str + "')" + "\"" + " ";
                        newlineShow = newlineShow + "id=" + "\"" + progstr + "Show" + projstr + "\"" + ">" + childprog1str + "</button>";
                        //newlineShow = newlineShow + ">" + childprog1str + "</button>";

                        nparentprogShowk += newlineShow + Environment.NewLine;   //add new line


                        newlineShow = "<button class=" + "\"" + progstr + "Showtablinks" + "\"" + " ";
                        //newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowOpenTab(event, '" + progstr + "Show" + childprog1str + "', '" + childprog1str + "')" + "\"" + " ";
                        newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowDelChild('" + childprog1str + "', '" + projstr + "')" + "\"" + " ";
                        newlineShow = newlineShow + "id=" + "\"" + progstr + "Show" + projstr + "Del" + "\"" + ">Del</button>";

                        nparentprogShowk += newlineShow + Environment.NewLine;   //add new line


                        childstr = nextchildstrindx;
                        newlineShow = "";
                        newlineShow = "<button class=" + "\"" + progstr + "Showtablinks" + "\"" + " ";
                        newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowAddChild('" + progstr + "ShowStart" + "', '" + childstr + "')" + "\"" + " ";
                        newlineShow = newlineShow + "id=" + "\"" + progstr + "ShowStart" + childstr + "\"" + ">Add Child</button>";

                        nparentprogShowk += newlineShow + Environment.NewLine;   //add new line

                        oldparentprogShowk = "";   //blank out line, as not needed now
                    }

                    nparentprogShowk += oldparentprogShowk + Environment.NewLine;


                }
                srparentprogShowk.Close();
                System.IO.File.WriteAllText(FileNameparentprogShowk, nparentprogShowk);




                //Edit Parent existing progTab.cshtml 
                var parentProgTabk = "Views/" + progstr + "/" + progstr + "Tab.cshtml";  //parent prog.cshtml
                //var FileNameparentprogTabk = HttpContext.Server.MapPath(parentProgTabk);
                var FileNameparentprogTabk = Path.Combine(_Env.ContentRootPath, parentProgTabk);

                string oldparentprogTabk;
                string nparentprogTabk = "";
                search_textChild1 = "";
                string newlineTab = "";
                childstr = "";
                StreamReader srparentprogTabk = System.IO.File.OpenText(FileNameparentprogTabk);
                while ((oldparentprogTabk = srparentprogTabk.ReadLine()) != null)   //read all lines
                {

                    //oldparentprogTabk = oldparentprogTabk.Replace("~childprog2~", childprog1str);

                    oldparentprogTabk = oldparentprogTabk.Replace("~childprog" + nextindxuse + "~", childprog1str);


                    //search_textChild1 = progstr + "TabStartChild2";
                    search_textChild1 = progstr + "TabStart" + projstr;
                    if (oldparentprogTabk.Contains(search_textChild1))  //do not keep this line in file
                    {
                        // n += newline + Environment.NewLine;   //add new line, after found target line
                        //Add new lines for Tab child and add new child tabs
                        //newlineTab = "<button class=" + "\"" + progstr + "Tabtablinks" + "\"" + " ";
                        //newlineTab = newlineTab + "onclick=" + "\"" + progstr + "TabOpenTab(event, '" + progstr + "Tab" + childprog1str + "', '" + childprog1str + "')" + "\"" + " ";
                        //newlineTab = newlineTab + "id=" + "\"" + progstr + "TabChild2" + "\"" + ">" + childprog1str + "</button>";
                        //newlineTab = newlineTab + ">" + childprog1str + "</button>";

                        newlineTab = "<button class=" + "\"" + progstr + "Tabtablinks" + "\"" + " ";
                        newlineTab = newlineTab + "onclick=" + "\"" + progstr + "TabOpenTab(event, '" + progstr + "Tab" + childprog1str + "', '" + childprog1str + "')" + "\"" + " ";
                        newlineTab = newlineTab + "id=" + "\"" + progstr + "Tab" + projstr + "\"" + ">" + childprog1str + "</button>";
                        //newlineShow = newlineShow + ">" + childprog1str + "</button>";

                        nparentprogTabk += newlineTab + Environment.NewLine;   //add new line


                        newlineTab = "<button class=" + "\"" + progstr + "Tabtablinks" + "\"" + " ";
                        //newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowOpenTab(event, '" + progstr + "Show" + childprog1str + "', '" + childprog1str + "')" + "\"" + " ";
                        newlineTab = newlineTab + "onclick=" + "\"" + progstr + "TabDelChild('" + childprog1str + "', '" + projstr + "')" + "\"" + " ";
                        newlineTab = newlineTab + "id=" + "\"" + progstr + "Tab" + projstr + "Del" + "\"" + ">Del</button>";

                        nparentprogTabk += newlineTab + Environment.NewLine;   //add new line


                        childstr = nextchildstrindx;
                        newlineTab = "";
                        newlineTab = "<button class=" + "\"" + progstr + "Tabtablinks" + "\"" + " ";
                        //newlineTab = newlineTab + "onclick=" + "\"" + progstr + "TabAddChild('" + progstr + "TabStart" + "', '" + childstr + "')" + "\"" + " ";
                        //newlineTab = newlineTab + "id=" + "\"" + progstr + "TabStartChild3" + "\"" + ">Add Child</button>";

                        newlineTab = newlineTab + "onclick=" + "\"" + progstr + "TabAddChild('" + progstr + "TabStart" + "', '" + childstr + "')" + "\"" + " ";
                        newlineTab = newlineTab + "id=" + "\"" + progstr + "TabStart" + childstr + "\"" + ">Add Child</button>";

                        nparentprogTabk += newlineTab + Environment.NewLine;   //add new line

                        oldparentprogTabk = "";   //blank out line, as not needed now
                    }

                    nparentprogTabk += oldparentprogTabk + Environment.NewLine;

                }
                srparentprogTabk.Close();
                System.IO.File.WriteAllText(FileNameparentprogTabk, nparentprogTabk);




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progGrid.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progGrid.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                string progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/progGrid.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progGrid.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progGrid.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progGrid.cshtml", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "Grid.cshtml";
                string outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                string text = System.IO.File.ReadAllText(outputDecrpt);

                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                string destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Grid.cshtml";

                string outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/prog.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/prog.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.cshtml", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + ".cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);

                text = text.Replace("~topprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }


                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + ".cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/prog.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/prog.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.js", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.js", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.js", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/prog.js", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + ".js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~topprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~FPK_ID~", strpkid);


                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + childprog1str + ".js";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progResults.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progResults.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.cshtml", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "Results.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);

                //text = text.Replace("~tablemodel~", childtable1str);
                //text = text.Replace("~proj~", projstr);
                //text = text.Replace("~topprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                //text = text.Replace("~progtop~", progstr);   //TOP GRID Table name
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Results.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------




                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progResults.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progResults.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.js", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.js", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.js", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progResults.js", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "Results.js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~tablemodel~", childtable1str);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~topprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~progtop~", progstr);   //TOP GRID Table name
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + childprog1str + "Results.js";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------






                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progController.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progController.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "Controller.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~tablemodel~", childtable1str);
                //text = text.Replace("~table~", childtable1str);
                text = text.Replace("~PK_ID~", strchild1pkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Controllers/" + childprog1str + "Controller.cs";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------





                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/projMod.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/projMod.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/projMod.cs", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/projMod.cs", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/projMod.cs", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/projMod.cs", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "Mod.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~tablemodel~", childtable1str);
                //text = text.Replace("~table~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "" + childprog1str + "Mod.cs";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progViewModel.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progViewModel.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "ViewModel.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~tablemodel~", childtable1str);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Models/" + childprog1str + "ViewModel.cs";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------






                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progBLL.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progBLL.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/progBLL.cs", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progBLL.cs", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progBLL.cs", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progBLL.cs", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "BLL.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~tablemodel~", childtable1str);
                text = text.Replace("~table~", childtable1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1type~", column1type);
                text = text.Replace("~column2type~", column2type);
                text = text.Replace("~column3type~", column3type);
                text = text.Replace("~column4type~", column4type);

                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "" + childprog1str + "BLL.cs";
                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------








                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progDetail.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progDetail.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "Detail.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Detail.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progShowDetail.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progShowDetail.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.cshtml", "col4", column4type);
                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "ShowDetail.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~topprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~childprog~", childchildprogstr);   //TOP CHILD CHILD table name - please check   ????? do in TOP CHILD CHILD
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "ShowDetail.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------






                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progShowDetail.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progShowDetail.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.js", "col1", column1type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.js", "col2", column2type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.js", "col3", column3type);
                ConfigureColumn("Gen_Code/TOPINNERGRID/progShowDetail.js", "col4", column4type);

                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "ShowDetail.js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~topprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~childprog~", childchildprogstr);   //TOP CHILD CHILD table name - please check   ????? do in TOP CHILD CHILD
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + childprog1str + "ShowDetail.js";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------





                //inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progShow.czhtml");
                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progShowStart.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/TOPINNERGRID/progShow.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                progrealname = "Gen_Code/TOPINNERGRID/" + childprog1str + "Show.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~topprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                //text = text.Replace("~childprog1~", childchildprogstr);
                //text = text.Replace("~childprog2~", childchildprogstr);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Show.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                //--------------------------------------------------------------------------------------------------------------------
                //TOPINNERGRID coding
                //--------------------------------------------------------------------------------------------------------------------


            }
            catch (Exception)
            {


            }

        }



        private void GenerateINNERGRIDRAZNEW(string projstr, string topprog, string progstr, string childprog1str, string strchild1pkid, string child1column1str, string column1type, string child1column2str, string column2type, string child1column3str, string column3type, string child1column4str, string column4type, string childchildprogstr, string strpkid, string stradoefcombo, string childchildtablestr)
        {

            try
            {

                string inputDecrpt = "";
                string outputDecrpt = "";
                //--------------------------------------------------------------------------------------------------------------------
                //INNERGRID coding  
                //--------------------------------------------------------------------------------------------------------------------


                //Edit Parent existing prog.cshtml  (ie replace ~childprog~ with progstr)
                var parentProgk = "Views/" + topprog + "/" + topprog + ".cshtml";  //top prog.cshtml
                                                                                   // var FileNameparentprogk = HttpContext.Server.MapPath(parentProgk);
                var FileNameparentprogk = Path.Combine(_Env.ContentRootPath, parentProgk);
                string oldparentprogk;
                string nparentprogk = "";
                StreamReader srparentprogk = System.IO.File.OpenText(FileNameparentprogk);
                while ((oldparentprogk = srparentprogk.ReadLine()) != null)   //read all lines
                {

                    if (projstr == "Child1")
                    {
                        oldparentprogk = oldparentprogk.Replace("~childchild1proguse~", childprog1str);
                    }
                    if (projstr == "Child2")
                    {
                        oldparentprogk = oldparentprogk.Replace("~childchild2proguse~", childprog1str);
                    }
                    if (projstr == "Child3")
                    {
                        oldparentprogk = oldparentprogk.Replace("~childchild3proguse~", childprog1str);
                    }
                    if (projstr == "Child4")
                    {
                        oldparentprogk = oldparentprogk.Replace("~child4proguse~", childprog1str);
                    }
                    if (projstr == "Child5")
                    {
                        oldparentprogk = oldparentprogk.Replace("~childchild5proguse~", childprog1str);
                    }
                    if (projstr == "Child6")
                    {
                        oldparentprogk = oldparentprogk.Replace("~childchild6proguse~", childprog1str);
                    }
                    if (projstr == "Child7")
                    {
                        oldparentprogk = oldparentprogk.Replace("~childchild7proguse~", childprog1str);
                    }
                    if (projstr == "Child8")
                    {
                        oldparentprogk = oldparentprogk.Replace("~childchild8proguse~", childprog1str);
                    }

                    nparentprogk += oldparentprogk + Environment.NewLine;

                }
                srparentprogk.Close();
                System.IO.File.WriteAllText(FileNameparentprogk, nparentprogk);



                string nextchildstrindx = "";
                string nextindxuse = "";
                if (projstr == "Child1")
                {
                    nextchildstrindx = "Child2";
                    nextindxuse = "1";
                }
                if (projstr == "Child2")
                {
                    nextchildstrindx = "Child3";
                    nextindxuse = "2";
                }
                if (projstr == "Child3")
                {
                    nextchildstrindx = "Child4";
                    nextindxuse = "3";
                }
                if (projstr == "Child4")
                {
                    nextchildstrindx = "Child5";
                    nextindxuse = "4";
                }
                if (projstr == "Child5")
                {
                    nextchildstrindx = "Child6";
                    nextindxuse = "5";
                }
                if (projstr == "Child6")
                {
                    nextchildstrindx = "Child7";
                    nextindxuse = "6";
                }
                if (projstr == "Child7")
                {
                    nextchildstrindx = "Child8";
                    nextindxuse = "7";
                }

                //Edit Parent existing progShow.cshtml 
                var parentProgShowk = "Views/" + progstr + "/" + progstr + "Show.cshtml";  //parent prog.cshtml
                //var FileNameparentprogShowk = HttpContext.Server.MapPath(parentProgShowk);
                var FileNameparentprogShowk = Path.Combine(_Env.ContentRootPath, parentProgShowk);
                string oldparentprogShowk;
                string nparentprogShowk = "";
                string search_textChild1 = "";
                string newlineShow = "";
                string childstr = "";
                StreamReader srparentprogShowk = System.IO.File.OpenText(FileNameparentprogShowk);
                while ((oldparentprogShowk = srparentprogShowk.ReadLine()) != null)   //read all lines
                {

                    oldparentprogShowk = oldparentprogShowk.Replace("~childprog" + nextindxuse + "~", childprog1str);

                    search_textChild1 = progstr + "ShowStart" + projstr;
                    if (oldparentprogShowk.Contains(search_textChild1))  //do not keep this line in file
                    {
                        // n += newline + Environment.NewLine;   //add new line, after found target line
                        //Add new lines for show child and add new child tabs
                        newlineShow = "<button class=" + "\"" + progstr + "Showtablinks" + "\"" + " ";
                        newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowOpenTab(event, '" + progstr + "Show" + childprog1str + "', '" + childprog1str + "')" + "\"" + " ";
                        newlineShow = newlineShow + "id=" + "\"" + progstr + "Show" + projstr + "\"" + ">" + childprog1str + "</button>";
                        //newlineShow = newlineShow + ">" + childprog1str + "</button>";

                        nparentprogShowk += newlineShow + Environment.NewLine;   //add new line


                        newlineShow = "<button class=" + "\"" + progstr + "Showtablinks" + "\"" + " ";
                        //newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowOpenTab(event, '" + progstr + "Show" + childprog1str + "', '" + childprog1str + "')" + "\"" + " ";
                        newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowDelChild('" + childprog1str + "', '" + projstr + "')" + "\"" + " ";
                        newlineShow = newlineShow + "id=" + "\"" + progstr + "Show" + projstr + "Del" + "\"" + ">Del</button>";

                        nparentprogShowk += newlineShow + Environment.NewLine;   //add new line


                        childstr = nextchildstrindx;
                        newlineShow = "";
                        newlineShow = "<button class=" + "\"" + progstr + "Showtablinks" + "\"" + " ";
                        newlineShow = newlineShow + "onclick=" + "\"" + progstr + "ShowAddChild('" + progstr + "ShowStart" + "', '" + childstr + "')" + "\"" + " ";
                        newlineShow = newlineShow + "id=" + "\"" + progstr + "ShowStart" + childstr + "\"" + ">Add Child</button>";

                        nparentprogShowk += newlineShow + Environment.NewLine;   //add new line

                        oldparentprogShowk = "";   //blank out line, as not needed now
                    }

                    nparentprogShowk += oldparentprogShowk + Environment.NewLine;


                }
                srparentprogShowk.Close();
                System.IO.File.WriteAllText(FileNameparentprogShowk, nparentprogShowk);





                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progGrid.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progGrid.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                string progrealname = "";
                ConfigureColumn("Gen_Code/INNERGRID/progGrid.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/progGrid.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/progGrid.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/progGrid.cshtml", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "Grid.cshtml";
                string outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);

                string text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }

                System.IO.File.WriteAllText(outprogrealname, text);

                string destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Grid.cshtml";

                string outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------





                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/prog.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/prog.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/INNERGRID/prog.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/prog.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/prog.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/prog.cshtml", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + ".cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                text = text.Replace("~topprog~", topprog);
                text = text.Replace("~topinnerprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~childprog~", childchildprogstr);   //TOP CHILD CHILD table name - please check  
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + ".cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------






                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/prog.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/prog.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/INNERGRID/prog.js", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/prog.js", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/prog.js", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/prog.js", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + ".js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);

                text = text.Replace("~topprog~", topprog);
                text = text.Replace("~topinnerprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + childprog1str + ".js";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Scripts/" + childprog1str + ".js";
                //destfolderViews = "~/Scripts/" + childprog1str + ".js";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);






                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progResults.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progResults.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                ConfigureColumn("Gen_Code/INNERGRID/progResults.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/progResults.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/progResults.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/progResults.cshtml", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "Results.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);

                text = text.Replace("~tablemodel~", childprog1str);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~topprog~", topprog);
                text = text.Replace("~progtop~", progstr);   //TOP INNER GRID prog name
                text = text.Replace("~FPK_ID~", strpkid);
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Results.cshtml";


                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------

                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Views/" + childprog1str + "/" + childprog1str + "Results.cshtml";
                //destfolderViews = "~/Views/" + childprog1str + "/" + childprog1str + "Results.cshtml";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);








                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progResults.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progResults.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";
                ConfigureColumn("Gen_Code/INNERGRID/progResults.js", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/progResults.js", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/progResults.js", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/progResults.js", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "Results.js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~tablemodel~", childprog1str);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~topprog~", topprog);
                text = text.Replace("~progtop~", progstr);   //TOP INNER GRID prog name
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + childprog1str + "Results.js";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progController.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progController.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "Controller.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~tablemodel~", childprog1str);
                //text = text.Replace("~table~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Controllers/" + childprog1str + "Controller.cs";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Controllers/" + childprog1str + "Controller.cs";
                //destfolderViews = "~/Controllers/" + childprog1str + "Controller.cs";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);





                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progViewModel.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progViewModel.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "ViewModel.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                //text = text.Replace("~tablemodel~", childprog1str);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Models/" + childprog1str + "ViewModel.cs";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------

                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Models/" + childprog1str + "ViewModel.cs";
                //destfolderViews = "~/Models/" + childprog1str + "ViewModel.cs";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);






                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progBLL.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progBLL.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                ConfigureColumn("Gen_Code/INNERGRID/progBLL.cs", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/progBLL.cs", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/progBLL.cs", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/progBLL.cs", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "BLL.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                //text = text.Replace("~tablemodel~", childprog1str);
                text = text.Replace("~table~", childchildtablestr);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1type~", column1type);
                text = text.Replace("~column2type~", column2type);
                text = text.Replace("~column3type~", column3type);
                text = text.Replace("~column4type~", column4type);

                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "" + childprog1str + "BLL.cs";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appBLL/" + childprog1str + "BLL.cs";
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/" + childprog1str + "BLL.cs";
                //destfolderViews = "~/" + childprog1str + "BLL.cs";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/projMod.cz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/projMod.cs");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                ConfigureColumn("Gen_Code/INNERGRID/projMod.cs", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/projMod.cs", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/projMod.cs", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/projMod.cs", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "Mod.cs";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);
                //text = text.Replace("~tablemodel~", childprog1str);
                //text = text.Replace("~table~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~FPK_ID~", strpkid);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "" + childprog1str + "Mod.cs";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------

                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appMODEL/" + childprog1str + "Mod.cs";
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/" + childprog1str + "Mod.cs";
                //destfolderViews = "~/" + childprog1str + "Mod.cs";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progDetail.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progDetail.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "Detail.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~prog~", childprog1str);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Detail.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------

                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Views/" + childprog1str + "/" + childprog1str + "Detail.cshtml";
                //destfolderViews = "~/Views/" + childprog1str + "/" + childprog1str + "Detail.cshtml";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progShowDetail.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progShowDetail.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.cshtml", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.cshtml", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.cshtml", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.cshtml", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "ShowDetail.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~topprog~", topprog);
                text = text.Replace("~topinnerprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~childprog~", childchildprogstr);   //TOP CHILD CHILD table name - please check   ????? do in TOP CHILD CHILD
                text = text.Replace("~FPK_ID~", strpkid);


                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "ShowDetail.cshtml";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Views/" + childprog1str + "/" + childprog1str + "ShowDetail.cshtml";
                //destfolderViews = "~/Views/" + childprog1str + "/" + childprog1str + "ShowDetail.cshtml";
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progShowDetail.jz");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progShowDetail.js");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.js", "col1", column1type);
                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.js", "col2", column2type);
                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.js", "col3", column3type);
                ConfigureColumn("Gen_Code/INNERGRID/progShowDetail.js", "col4", column4type);

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "ShowDetail.js";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~topprog~", topprog);
                text = text.Replace("~topinnerprog~", progstr);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~column1~", child1column1str);
                if (column1type == "Char")
                {
                    text = text.Replace("~chartypecol1~", "");
                }
                if (column1type == "Integer")
                {
                    text = text.Replace("~inttypecol1~", "");
                }
                if (column1type == "Decimal")
                {
                    text = text.Replace("~dectypecol1~", "");
                }
                if (column1type == "Date")
                {
                    text = text.Replace("~datetypecol1~", "");
                }
                text = text.Replace("~column2~", child1column2str);
                if (column2type == "Char")
                {
                    text = text.Replace("~chartypecol2~", "");
                }
                if (column2type == "Integer")
                {
                    text = text.Replace("~inttypecol2~", "");
                }
                if (column2type == "Decimal")
                {
                    text = text.Replace("~dectypecol2~", "");
                }
                if (column2type == "Date")
                {
                    text = text.Replace("~datetypecol2~", "");
                }
                text = text.Replace("~column3~", child1column3str);
                if (column3type == "Char")
                {
                    text = text.Replace("~chartypecol3~", "");
                }
                if (column3type == "Integer")
                {
                    text = text.Replace("~inttypecol3~", "");
                }
                if (column3type == "Decimal")
                {
                    text = text.Replace("~dectypecol3~", "");
                }
                if (column3type == "Date")
                {
                    text = text.Replace("~datetypecol3~", "");
                }
                text = text.Replace("~column4~", child1column4str);
                if (column4type == "Char")
                {
                    text = text.Replace("~chartypecol4~", "");
                }
                if (column4type == "Integer")
                {
                    text = text.Replace("~inttypecol4~", "");
                }
                if (column4type == "Decimal")
                {
                    text = text.Replace("~dectypecol4~", "");
                }
                if (column4type == "Date")
                {
                    text = text.Replace("~datetypecol4~", "");
                }
                text = text.Replace("~childprog~", childchildprogstr);   //TOP CHILD CHILD table name - please check   ????? do in TOP CHILD CHILD
                text = text.Replace("~FPK_ID~", strpkid);


                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "wwwroot/js/" + childprog1str + "ShowDetail.js";

                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------

                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Scripts/" + childprog1str + "ShowDetail.js";
                //destfolderViews = "~/Scripts/" + childprog1str + "ShowDetail.js";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);







                inputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progShow.czhtml");
                outputDecrpt = Path.Combine(_Env.ContentRootPath, "Gen_Code/INNERGRID/progShow.cshtml");
                System.IO.File.Copy(inputDecrpt, outputDecrpt, true);

                progrealname = "";

                progrealname = "Gen_Code/INNERGRID/" + childprog1str + "Show.cshtml";
                outprogrealname = Path.Combine(_Env.ContentRootPath, progrealname);
                text = "";
                text = System.IO.File.ReadAllText(outputDecrpt);
                //text = text.Replace("~proj~", projstr);
                text = text.Replace("~topprog~", topprog);
                text = text.Replace("~prog~", childprog1str);
                text = text.Replace("~PK_ID~", strchild1pkid);
                text = text.Replace("~childprog1~", childchildprogstr);
                text = text.Replace("~childprog2~", childchildprogstr);

                System.IO.File.WriteAllText(outprogrealname, text);

                destfolderViews = "Views/" + childprog1str + "/" + childprog1str + "Show.cshtml";


                outputdestfolder = Path.Combine(_Env.ContentRootPath, destfolderViews);
                System.IO.File.Copy(outprogrealname, outputdestfolder, true);

                System.IO.File.Delete(outprogrealname);
                System.IO.File.Delete(outputDecrpt);
                //-------------------------------------------------------------------------------------------------


                //System.IO.File.WriteAllText(HttpContext.Server.MapPath(progrealname), text);
                //destfolderViews = "~/Gen_Code/" + projstr + "/appAPP/Views/" + childprog1str + "/" + childprog1str + "Show.cshtml";
                //destfolderViews = "~/Views/" + childprog1str + "/" + childprog1str + "Show.cshtml";

                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyFile(HttpContext.Server.MapPath(progrealname), HttpContext.Server.MapPath(destfolderViews), true);
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(HttpContext.Server.MapPath(progrealname));
                //new Microsoft.VisualBasic.Devices.Computer().FileSystem.DeleteFile(outputDecrpt);

                /*

                //--------------------------------------------------------------------------------------------------------------------
                //INNERGRID coding
                //--------------------------------------------------------------------------------------------------------------------
                */


            }
            catch (Exception)
            {


            }

        }




        //-------------------------------------------------------------------------
        //Add new lines to AHIM.csproj
        //-------------------------------------------------------------------------
        private void UpdateAHIMproj(string infile, string col, string newline)
        {

            MakeAHIMChange(infile, col, newline);
        }
        private void MakeAHIMChange(string infile, string col, string newline)
        {
            try
            {
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string search_text = col;   //Find line from where to Add new line
                string old;
                string n = "";
                StreamReader sr = System.IO.File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)   //read all lines
                {
                    if (old.Contains(search_text))
                    {
                        n += newline + Environment.NewLine;   //add new line, after found target line
                    }

                    n += old + Environment.NewLine;

                }
                sr.Close();
                System.IO.File.WriteAllText(FileName, n);

            }
            catch (Exception)
            {


            }

        }



        private void MakeAHIMRemove(string infile, string col, string newline)
        {
            try
            {
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string search_text = col;   //Find line from where to Add new line
                string old;
                string n = "";
                StreamReader sr = System.IO.File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)   //read all lines
                {
                    if (old.Contains(newline))
                    {
                    }
                    else
                    {
                        n += old + Environment.NewLine;
                    }

                }
                sr.Close();
                System.IO.File.WriteAllText(FileName, n);

            }
            catch (Exception)
            {


            }

        }




        //This function gets rid of all lines for a COLUMN 
        private void DelSiteRow(string infile)
        {
            try
            {
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string search_text = "~prog~";   //delete all lines with this text in it (ie ~prog~)
                string old;
                string n = "";
                StreamReader sr = System.IO.File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)
                {
                    if (!old.Contains(search_text))
                    {
                        n += old + Environment.NewLine;
                    }
                }
                sr.Close();
                System.IO.File.WriteAllText(FileName, n);

            }
            catch (Exception)
            {


            }

        }



        //===================================================================================================================
        //This function configures a COLUMN depending on type
        //===================================================================================================================
        private void ConfigureColumn(string infile, string col, string typee)
        {
            if (typee == "Char")
            {
                MakeCharColumn(infile, col);
            }
            //if (typee == "Number")
            //{
            //MakeNumColumn(infile, col);
            //}
            if (typee == "Date")
            {
                MakeDateColumn(infile, col);
            }
            if (typee == "Integer")
            {
                MakeIntColumn(infile, col);
            }
            if (typee == "Decimal")
            {
                MakeDecColumn(infile, col);
            }
        }

        //This function gets rid of all lines for a COLUMN that are NOT for a Date (ie ~chartype~, ~numtype~)
        private void MakeDateColumn(string infile, string col)
        {
            try
            {
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string search_text = "~chartype" + col + "~";   //delete all lines with this text in it (ie CHAR)
                string old;
                string n = "";
                StreamReader sr = System.IO.File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)
                {
                    if (!old.Contains(search_text))
                    {
                        n += old + Environment.NewLine;
                    }
                }
                sr.Close();
                System.IO.File.WriteAllText(FileName, n);


                string search_text2 = "~inttype" + col + "~";   //delete all lines with this text in it (ie INTEGER)
                string old2;
                string n2 = "";
                StreamReader sr2 = System.IO.File.OpenText(FileName);
                while ((old2 = sr2.ReadLine()) != null)
                {
                    if (!old2.Contains(search_text2))
                    {
                        n2 += old2 + Environment.NewLine;
                    }
                }
                sr2.Close();
                System.IO.File.WriteAllText(FileName, n2);


                string search_text3 = "~dectype" + col + "~";   //delete all lines with this text in it (ie DECIMAL)
                string old3;
                string n3 = "";
                StreamReader sr3 = System.IO.File.OpenText(FileName);
                while ((old3 = sr3.ReadLine()) != null)
                {
                    if (!old3.Contains(search_text3))
                    {
                        n3 += old3 + Environment.NewLine;
                    }
                }
                sr3.Close();
                System.IO.File.WriteAllText(FileName, n3);


            }
            catch (Exception)
            {


            }

        }

        //This function gets rid of all lines for a COLUMN that are NOT for a Char (ie ~datetype~, ~numtype~)
        private void MakeCharColumn(string infile, string col)
        {
            try
            {
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string search_text = "~datetype" + col + "~";   //delete all lines with this text in it (ie DATE)
                string old;
                string n = "";
                StreamReader sr = System.IO.File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)
                {
                    if (!old.Contains(search_text))
                    {
                        n += old + Environment.NewLine;
                    }
                }
                sr.Close();
                System.IO.File.WriteAllText(FileName, n);


                string search_text2 = "~inttype" + col + "~";   //delete all lines with this text in it (ie INTEGER)
                string old2;
                string n2 = "";
                StreamReader sr2 = System.IO.File.OpenText(FileName);
                while ((old2 = sr2.ReadLine()) != null)
                {
                    if (!old2.Contains(search_text2))
                    {
                        n2 += old2 + Environment.NewLine;
                    }
                }
                sr2.Close();
                System.IO.File.WriteAllText(FileName, n2);


                string search_text3 = "~dectype" + col + "~";   //delete all lines with this text in it (ie DECIMAL)
                string old3;
                string n3 = "";
                StreamReader sr3 = System.IO.File.OpenText(FileName);
                while ((old3 = sr3.ReadLine()) != null)
                {
                    if (!old3.Contains(search_text3))
                    {
                        n3 += old3 + Environment.NewLine;
                    }
                }
                sr3.Close();
                System.IO.File.WriteAllText(FileName, n3);


            }
            catch (Exception)
            {


            }

        }

        //This function gets rid of all lines for a COLUMN that are NOT for a Number (ie ~chartype~, ~datetype~, ~dectype~)
        private void MakeIntColumn(string infile, string col)
        {
            try
            {
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string search_text = "~chartype" + col + "~";   //delete all lines with this text in it (ie CHAR)
                string old;
                string n = "";
                StreamReader sr = System.IO.File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)
                {
                    if (!old.Contains(search_text))
                    {
                        n += old + Environment.NewLine;
                    }
                }
                sr.Close();
                System.IO.File.WriteAllText(FileName, n);


                string search_text2 = "~datetype" + col + "~";   //delete all lines with this text in it (ie DATE)
                string old2;
                string n2 = "";
                StreamReader sr2 = System.IO.File.OpenText(FileName);
                while ((old2 = sr2.ReadLine()) != null)
                {
                    if (!old2.Contains(search_text2))
                    {
                        n2 += old2 + Environment.NewLine;
                    }
                }
                sr2.Close();
                System.IO.File.WriteAllText(FileName, n2);


                string search_text3 = "~dectype" + col + "~";   //delete all lines with this text in it (ie DECIMAL)
                string old3;
                string n3 = "";
                StreamReader sr3 = System.IO.File.OpenText(FileName);
                while ((old3 = sr3.ReadLine()) != null)
                {
                    if (!old3.Contains(search_text3))
                    {
                        n3 += old3 + Environment.NewLine;
                    }
                }
                sr3.Close();
                System.IO.File.WriteAllText(FileName, n3);


            }
            catch (Exception)
            {


            }

        }

        //This function gets rid of all lines for a COLUMN that are NOT for a Number (ie ~chartype~, ~datetype~, ~inttype~)
        private void MakeDecColumn(string infile, string col)
        {
            try
            {
                //var FileName = HttpContext.Server.MapPath(infile);
                var FileName = Path.Combine(_Env.ContentRootPath, infile);

                string search_text = "~chartype" + col + "~";   //delete all lines with this text in it (ie CHAR)
                string old;
                string n = "";
                StreamReader sr = System.IO.File.OpenText(FileName);
                while ((old = sr.ReadLine()) != null)
                {
                    if (!old.Contains(search_text))
                    {
                        n += old + Environment.NewLine;
                    }
                }
                sr.Close();
                System.IO.File.WriteAllText(FileName, n);


                string search_text2 = "~datetype" + col + "~";   //delete all lines with this text in it (ie DATE)
                string old2;
                string n2 = "";
                StreamReader sr2 = System.IO.File.OpenText(FileName);
                while ((old2 = sr2.ReadLine()) != null)
                {
                    if (!old2.Contains(search_text2))
                    {
                        n2 += old2 + Environment.NewLine;
                    }
                }
                sr2.Close();
                System.IO.File.WriteAllText(FileName, n2);


                string search_text3 = "~inttype" + col + "~";   //delete all lines with this text in it (ie INTEGER)
                string old3;
                string n3 = "";
                StreamReader sr3 = System.IO.File.OpenText(FileName);
                while ((old3 = sr3.ReadLine()) != null)
                {
                    if (!old3.Contains(search_text3))
                    {
                        n3 += old3 + Environment.NewLine;
                    }
                }
                sr3.Close();
                System.IO.File.WriteAllText(FileName, n3);


            }
            catch (Exception)
            {


            }

        }





        private void Encrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }


        private void Decrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }




    }
}
