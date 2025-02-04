for /d %%s in (.\..\OUTPUTS\*) do (
%~dp0gifski_win/gifski --fps 10 -o %%s/%%~nxs.gif %%s/frame*.png
)