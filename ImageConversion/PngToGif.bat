for /d %%s in (.\..\OUTPUTS\1\*) do (
%~dp0gifski_win/gifski --fps 10 -o %%s/%%~nxs.gif %%s/frames/frame*.png
)
for /d %%s in (.\..\OUTPUTS\2\*) do (
%~dp0gifski_win/gifski --fps 10 -o %%s/%%~nxs.gif %%s/frames/frame*.png
)
for /d %%s in (.\..\OUTPUTS\3\*) do (
%~dp0gifski_win/gifski --fps 10 -o %%s/%%~nxs.gif %%s/frames/frame*.png
)
for /d %%s in (.\..\\OUTPUTS\4\*) do (
%~dp0gifski_win/gifski --fps 10 -o %%s/%%~nxs.gif %%s/frames/frame*.png
)