cd MapDicer\bin
REM ^ goes to MapDicer\MapDicer\bin
md Debug
md Release
cd Debug
mklink /D Assets C:\Users\Jatlivecom\GitHub\MapDicer\MapDicer\Assets
cd ..
cd Release
mklink /D Assets C:\Users\Jatlivecom\GitHub\MapDicer\MapDicer\Assets
