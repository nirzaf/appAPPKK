NOTES
---------

1.  If NO Child EXISTS:-
     Change progShowDetail.js, see ~prog~DetailDisplayRecord function and put in hide progress bar line.


 
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



