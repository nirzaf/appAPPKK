using appBLL;
using System.Collections.Generic;
using System.Reflection;


namespace appAPP.Models
{
    public class GrandChildAddViewModel : appMODEL.GrandChildAdd
    {
        public long CurrentPageNumber { get; set; }
        public long CurrentRowNumber { get; set; }
        public long PageSize { get; set; }
        public long TotalPages { get; set; }
        public long TotalRows { get; set; }
        public long PageRows { get; set; }
        public List<string> ReturnMessage { get; set; }
        public bool ReturnStatus { get; set; }
        public string SortBy { get; set; }
        public string SortAscendingDescending { get; set; }
        public long PreviframeWidth { get; set; }
        public long PreviframeHeight { get; set; }
        public string SortBy2 { get; set; }
        public string SortAscendingDescending2 { get; set; }
        public long PageSize2 { get; set; }
        public string download_token_value { get; set; }
        public string PDFFlag { get; set; }

        public void UpdateViewModel(object fromRecord, PropertyInfo[] fromFields)
        {
            UtilitiesBLL.SetProperties(fromFields, fromRecord, this);
        }

    }

}