NOTES
======

EDIT BundleConfig.cs and add all the Grids javascript files


1.  If NO CHILD EXISTS:-
     Change progTab.cshtml, see down the bottom. Put in hide progress bar line.
     Change progShowDetail.js, see ~prog~DetailDisplayRecord function and put in hide progress bar line.

---------------------------------------------------------------------------------------------------------------------------------------------

CHANGE THE SORT COLUMN and DESC or ASC - Starting Default
  1.  Change prog.cshtml    -        if (currentSortBy == "FifthCol" || currentSortBy == "")         //default Sort column, can change to another column
  2.  Change prog.js           -       $("#~prog~Formtopdiv [name=SortAscendingDescending]").val("ASC");    //change to Sort type default
                                                   $("#~prog~Formtopdiv [name=SortBy]").val("~prog~FourthCol");                //change default starting Sort Column 

  3.  Change progBLL.cs - (Get~prog~() function.   Change  sqlSorts.Append(" ORDER BY ~column4~ ");    to same as in Step 1.

  NOTE:-  Do not need to do Step 1 and Step 3. As initial sort column is now set in Step 2.
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------


 ADJUST HEIGHT of CHILD in TAB & DETAIL
 1. Change CHILD progResults.js   -   AdjustDivWidth~prog~() function
                     Change,  document.getElementById("idDiv~prog~TBody").style.height = "140px";  //must match ~prog~(SetPagingLinks)

 2. Change CHILD prog.js  -   progSetPagingLinks() function
                    Change, 140 - 25  (change 140)

 3.  When going into OpenDetail (not + ), can Change Parent detail page height via PARENT progShowDetail.js  (change height, in unison with CHILD height)
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  
CHECKBOX in DETAIL page
1.  Change projMod.cs, search for Private_Cover, and replace with real table column to be used.
     Also make sure what Type of field used (by default it is bit)

2.  Change progShowDetail.cshtml, search for Private_Cover, and replace with real table column to be used.

3.  Change progShowDetail.js, search for Private_Cover, and replace with real table column to be used.
     Also uncomment the lines necessary.

4.  Change progBLL.cs, search for Private_Cover, and replace with real table column to be used.
     Also uncomment the lines necessary, in Get~prog~Detail, Add~prog~, Update~prog~  functions.
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


RADIO BUTTONS in DETAIL page
1.  Change projMod.cs, search for Sex, and replace with real table column to be used.
     Also make sure what Type of field used (by default it is char)

2.  Change progShowDetail.cshtml, search for Sex, and replace with real table column to be used.

3.  Change progShowDetail.js, search for Sex, and replace with real table column to be used.
     Also uncomment the lines necessary.

4.  Change progBLL.cs, search for Sex, and replace with real table column to be used.
     Also uncomment the lines necessary, in Get~prog~Detail, Add~prog~, Update~prog~  functions.

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

NORMAL COMBO in DETAIL page
1.  Replace normcombopkid, normcombocol1 & normcombo -  with combo reference -  table/view, PKID, & column   - in  --->   progMod.cs.
     eg  Replace  normcombopkid   Asset_Operating_Type_ID    -   PKID
           Replace  normcombocol1   OperatorTypeName
           Replace  normcombo         OperatorType   -   table or view


2.  Replace normcombopkid, normcombocol1 & normcombo -  with combo reference -  table/view, PKID, & column   - in  --->   progBLL.cs.
     eg  Replace  normcombopkid   Asset_Operating_Type_ID    -   PKID
           Replace  normcombocol1   OperatorTypeName
           Replace  normcombo         OperatorType   -   table or view

     Change in Get~prog~Detail, SELECT clause with View to include added como table. May Need a VIEW created, to get referenced fields.
     Replace ~viewcombotable~ with VIEW created. 

     Also change in Add~prog~(),  the SQL to do INSERT and parameter. 
     Also change in Update~prog~(),  the SQL to do UPDATE and parameter.



3.  Replace normcombo  with combo reference table/view in --->  progController.cs.
      eg  Replace  normcombo         OperatorType   -   table or view



4.  Replace normcombopkid  & normcombocol1 with combo reference table/view PKID & column to be shown in --->  progShowDetail.js.
     eg  Replace  normcombopkid   Asset_Operating_Type_ID    -   PKID
           Replace  normcombocol1   OperatorTypeName
           Replace  normcombo         OperatorType   -   table or view
           Replace Worker_Type       OperatorTypeName
           Replace WorkerType         OperatorTypeName

     Uncomment line setTimeout(progDetailGoToServerAdd, 30)


5.  Replace normcombopkid  & normcombocol1 with combo reference table/view PKID & column to be shown in --->  progShowDetail.cshtml.
     eg  Replace  normcombopkid   Asset_Operating_Type_ID    -   PKID
           Replace  normcombo         OperatorType   -   table or view
           Replace Worker_Type       OperatorTypeName

 
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
DATE using DateTimeOffSet7  FORMAT

1.  Change code in ~prog~Mod.cs   (DateTimeOffSet format for date field)

2.  Change code in ~prog~BLL.cs   (DateTimeOffSet format for date field)
             .   ~prog~Search()
             .   ~prog~SearchPDF()
             .   Get~prog~Detail()
             .   Add~prog~()
             .   Update~prog~()

3.  ~prog~Results.cshtml   (DateTimeOffSet format for date field)



------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Vertical Scrolling (all pages)

prog_Layout.cshtml  -   has   -    overflow-y: scroll;    Optional on Body css





