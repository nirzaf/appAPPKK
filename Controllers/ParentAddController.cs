//using System.Media;
using Microsoft.AspNetCore.Mvc;

namespace appAPP.Controllers
{

    //[Authorize(Roles = "Admin")]
    //[Authorize(Users = "kalvin,umesh")]
    //[Authorize]
    public class ParentAddController : Controller
    {

        /// <summary>
        /// Initial start point of displaying the Grid
        /// </summary>
        public ActionResult DisplayParentAdd()
        {
            Models.ParentAddViewModel ThisViewModel = new Models.ParentAddViewModel();

            ThisViewModel.TotalPages = 0;
            ThisViewModel.TotalRows = 0;
            ThisViewModel.CurrentPageNumber = 0;
            //ThisViewModel.SortAscendingDescending = "DESC";  //default init setting, can change to ASC. NOT NEEDED now

            ViewData.Model = ThisViewModel;

            return View("ParentAddGrid");   //must match View name
        }

        /// <summary>
        /// Display Detail Page
        /// </summary>
        //[Authorize]
        public ActionResult ParentAddDetail()
        {

            //Names130BLL ThisBLL = new Names130BLL();
            Models.ParentAddViewModel ThisViewModel = new Models.ParentAddViewModel();

            ViewData.Model = ThisViewModel;

            return View();  //goes to Detail View by default
        }



    }
}
