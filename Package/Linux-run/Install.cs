namespace Package.Linux_run
{
    internal static class Install
    {
        public static string Desktop = @"[Desktop Entry]

Name=PopStudio

Exec=/usr/bin/PopStudio

Icon=/usr/share/icons/popstudio_icon.png

Terminal=false

Type=Application

X-Ubuntu-Touch=true

Categories=Development";

        public static string Script = @"#!/bin/bash
echo -e -n ""PopStudio Installer\n""
echo -e -n ""Version:3.9\n""
echo -e -n ""Author:YingFengTingYu\n""
echo -e -n ""Modify the data package, images, actions, particle effects and other contents of PopCap Games by using PopStudio!\n""
echo -e -n ""Maybe you need to enter your password while installing.\n""
echo -e -n ""Installing...\n""
lines=23
tail -n+${lines} $0 > ./PopStudio.tar.bz2
tar jxvf ./PopStudio.tar.bz2 -C
sudo chmod +x ./PopStudio/Main/*
sudo cp -f ./PopStudio/Main/* /usr/bin/
sudo cp -f ./PopStudio/Info/popstudio_icon.png /usr/share/icons/
sudo cp -f ./PopStudio/Info/popstudio.desktop /usr/share/applications/
if [ $? == 0 ];then
  echo -e -n ""Success\n""
else
  echo -e -n ""Fail\n""
fi
rm -rf ./PopStudio
rm -rf ./PopStudio.tar.bz2
exit 0
";
    }
}
