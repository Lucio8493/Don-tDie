#! /bin/sh

# Download Unity3D installer into the container
#  The below link will need to change depending on the version
#  Refer to https://unity3d.com/get-unity/download/archive and find the link pointed to by Mac "Unity Editor"
echo 'Downloading Unity 5.5.1 pkg:'
curl --retry 5 -o Unity.pkg https://netstorage.unity3d.com/unity/46dda1414e51/MacEditorInstaller/Unity-2017.2.0f3.pkg
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# In Unity 5 they split up build platform support into modules which are installed separately
# By default, only Mac OSX support is included in the original editor package; Windows, Linux, iOS, Android, and others are separate
# In this example we download Android support. Refer to http://unity.grimdork.net/ to see what form the URLs should take
echo 'Downloading Unity 5.5.1 Android Build Support pkg:'
curl --retry 5 -o Unity_android.pkg https://beta.unity3d.com/download/46dda1414e51/MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-2017.2.0f3.pkg
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# Run installer(s)
echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
echo 'Installing Unity_android.pkg'
sudo installer -dumplog -package Unity_android.pkg -target /
