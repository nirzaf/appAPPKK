using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;


namespace appMODEL
{

    public class ChildAdd
    {
        //public long Name_ID { get; set; }
        public long SECTOR_ID { get; set; }
        public long PHY_ID { get; set; }

        public string SECTOR_NAME { get; set; }

        public string SECTOR_TYPE { get; set; }

        public string SECTOR_DESC { get; set; }

        public DateTime SECTOR_BORN_DT { get; set; }

        public string GlobalSearchString { get; set; }

        public string SECTOR_NAME_Str { get; set; }
        public string SECTOR_TYPE_Str { get; set; }
        public string SECTOR_DESC_Str { get; set; }
        public string SECTOR_BORN_DT_Str { get; set; }


        public string Date_Bought_From { get; set; }
        public string Date_Bought_To { get; set; }
        public string Created_By { get; set; }

        public List<long> PK_IDD { get; set; }
        public List<string> CheckBoxx { get; set; }

        public string Logoimg { get; set; }

        public long Document_ID { get; set; }    //for Upload of a Document

        public int Private_Cover { get; set; }

        public string PHY_NAME { get; set; }
        public string PHY_DESC { get; set; }

        public string PHY_NOTE { get; set; }

        //next is for Parent-Child processing
        public long KNG_ID { get; set; }
        //public long SECTOR_ID { get; set; }
        public long SECTOR_TOPINNER_ID { get; set; }
        public long PARENT_SECTOR_TOPINNER_ID { get; set; }
        public long SECTOR_INNER_ID { get; set; }
        public long PARENT_SECTOR_INNER_ID { get; set; }
        public long SECTOR_COMBO_ID { get; set; }
        public long PARENT_SECTOR_COMBO_ID { get; set; }

        public long PROP_ID { get; set; }

        public string sectortype { get; set; }
        public string propnote { get; set; }

        public string ParentPKID { get; set; }
        public string ChildNumber { get; set; }
        public string ParentProgName { get; set; }
        public string ParentProgPKID { get; set; }
        public string TopProgName { get; set; }

        public string projname { get; set; }
        public string tablename { get; set; }
        public string progname { get; set; }
        public string primarykey { get; set; }
        public string column1 { get; set; }
        public string column1type { get; set; }
        public string column2 { get; set; }
        public string column2type { get; set; }
        public string column3 { get; set; }
        public string column3type { get; set; }
        public string column4 { get; set; }
        public string column4type { get; set; }
        public string column5 { get; set; }
        public string column5type { get; set; }
        public string column6 { get; set; }
        public string column6type { get; set; }

        public string child1clicked { get; set; }
        public string childprog1 { get; set; }  //also TOPINNERGRID table name
        public string child1primarykey { get; set; }
        public string child1column1 { get; set; }
        public string childcolumn1type { get; set; }
        public string child1column2 { get; set; }
        public string childcolumn2type { get; set; }
        public string child1column3 { get; set; }
        public string childcolumn3type { get; set; }
        public string child1column4 { get; set; }
        public string childcolumn4type { get; set; }

        public string childchildclicked { get; set; }
        public string childchildprog1 { get; set; }  //INNERGRID table name
        public string childchild1primarykey { get; set; }
        public string childchild1column1 { get; set; }
        public string childchildcolumn1type { get; set; }
        public string childchild1column2 { get; set; }
        public string childchildcolumn2type { get; set; }
        public string childchild1column3 { get; set; }
        public string childchildcolumn3type { get; set; }
        public string childchild1column4 { get; set; }
        public string childchildcolumn4type { get; set; }

        public string comboclicked { get; set; }
        public string comboprog { get; set; }  //TOPGRID Detail COMBO table name
        public string comboprimarykey { get; set; }
        public string combocolumn1 { get; set; }
        public string combocolumn1type { get; set; }
        public string combocolumn2 { get; set; }
        public string combocolumn2type { get; set; }

        public string aspxrazorcombo { get; set; }
        public string adoefcombo { get; set; }
        public string vscombo { get; set; }

        public string childprog2 { get; set; }  //also other TAB TOPCHILD table name

        public string dbname { get; set; }
        public string servername { get; set; }

        public string dbuser { get; set; }
        public string dbpass { get; set; }

        public string UserName { get; set; }
        public string webconfigcombo { get; set; }


    }






}
