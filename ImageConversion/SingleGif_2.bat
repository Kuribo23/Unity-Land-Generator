mkdir %~dp0Gif

for /d %%s in (.\..\OUTPUTS\*) do (
%~dp0gifski_win/gifski --fps 10 -o %~dp0Gif/%%~nxs.gif %%s/frame*.png
)