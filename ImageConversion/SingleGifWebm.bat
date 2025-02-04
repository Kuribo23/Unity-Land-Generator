mkdir %~dp0Single

for /d %%s in (.\..\OUTPUTS\*) do (
%~dp0gifski_win/gifski --fps 10 -o %~dp0Single/%%~nxs.gif %%s/frame*.png
)
for /d %%s in (.\..\OUTPUTS\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frame%%02d.png -c:v libvpx-vp9 -pix_fmt yuva420p -lossless 1 %~dp0Single/%%~nxs.webm
)

pause