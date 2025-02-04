mkdir %~dp0WebM

for /d %%s in (.\..\OUTPUTS\1\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -c:v libvpx-vp9 -pix_fmt yuva420p -lossless 1 %~dp0WebM/%%~nxs.webm
)
for /d %%s in (.\..\OUTPUTS\2\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -c:v libvpx-vp9 -pix_fmt yuva420p -lossless 1 %~dp0WebM/%%~nxs.webm
)
for /d %%s in (.\..\OUTPUTS\3\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -c:v libvpx-vp9 -pix_fmt yuva420p -lossless 1 %~dp0WebM/%%~nxs.webm
)
for /d %%s in (.\..\\OUTPUTS\4\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -c:v libvpx-vp9 -pix_fmt yuva420p -lossless 1 %~dp0WebM/%%~nxs.webm
)