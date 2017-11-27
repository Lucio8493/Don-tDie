#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# This link changes from time to time. I haven't found a reliable hosted installer package for doing regular
# installs like this. You will probably need to grab a current link from: http://unity3d.com/get-unity/download/archive
echo 'Downloading from https://beta.unity3d.com/download/46dda1414e51/MacEditorInstaller/Unity-2017.2.0f3.pkg: '
curl -o Unity.pkg https://beta.unity3d.com/download/46dda1414e51/MacEditorInstaller/Unity-2017.2.0f3.pkg

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /