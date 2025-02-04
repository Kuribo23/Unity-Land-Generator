mkdir %~dp0Apng

for /d %%s in (.\..\OUTPUTS\1\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -plays 0 %~dp0Apng/%%~nxs.apng
)
for /d %%s in (.\..\OUTPUTS\2\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -plays 0 %~dp0Apng/%%~nxs.apng
)
for /d %%s in (.\..\OUTPUTS\3\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -plays 0 %~dp0Apng/%%~nxs.apng
)
for /d %%s in (.\..\\OUTPUTS\4\*) do (
%~dp0ffmpeg_full/bin/ffmpeg -y -framerate 10 -i %%s/frames/frame%%02d.png -plays 0 %~dp0Apng/%%~nxs.apng
)