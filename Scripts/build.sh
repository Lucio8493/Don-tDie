#! /bin/sh

# NOTE the command args below make the assumption that your Unity project folder is
#  a subdirectory of the repo root directory, e.g. for this repo "unity-ci-test" 
#  the project folder is "UnityProject". If this is not true then adjust the 
#  -projectPath argument to point to the right location.

## Make the builds
# Recall from install.sh that a separate module was needed for Windows build support
echo "Attempting build of ${UNITYCI_PROJECT_NAME} for Android"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-nographics \
	-silent-crashes \
	-logFile $(pwd)/unity.log \
	-projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
	-buildAndroidPlayer "$(pwd)/Build/android/${UNITYCI_PROJECT_NAME}.apk" \
	-quit

rc1=$?
echo "Build logs (Android)"
cat $(pwd)/unity.log

echo "Attempting build of ${UNITYCI_PROJECT_NAME} for OSX"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-nographics \
	-silent-crashes \
	-logFile $(pwd)/unity.log \
	-projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
	-buildOSXUniversalPlayer "$(pwd)/Build/osx/${UNITYCI_PROJECT_NAME}.app" \
	-quit

rc2=$?
echo "Build logs (OSX)"
cat $(pwd)/unity.log

#Remeber to add exit $(($rc1|$rc2))
exit $(($rc1|$rc2))
