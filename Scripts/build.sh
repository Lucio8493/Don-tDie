#! /bin/sh

# Nota i comandi che seguono prevedono che la cartella del progetto e' una sottodirectory della directory del repository
# I comandi servono ad avviare e a fermare l'editor di unity in modalita' batch

# Avvio dei tests presenti nell'editor
echo "Running tests for ${UNITYCI_PROJECT_NAME}"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
	-batchmode \
	-nographics \
	-runTests \
	-testPlatform playmode \
	-testResults "$(pwd)/${UNITYCI_PROJECT_NAME}"/test.xml \
	-testFilter "$(pwd)/${UNITYCI_PROJECT_NAME}/Library/ScriptAssemblies/Assembly-CSharp.dll" \

rc0=$?
echo "Unit test logs"
cat "$(pwd)/${UNITYCI_PROJECT_NAME}"/test.xml
# Uscita in caso di fallimento dei tests
if [ $rc0 -ne 0 ]; then { echo "Failed unit tests"; exit $rc0; } fi


# Avvio della fase di build

# Avvio del processo di build per la creazione di uno standalone in grado di operare su sistemi Windows
echo "Attempting build of ${UNITYCI_PROJECT_NAME} for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-nographics \
	-silent-crashes \
	-logFile $(pwd)/unity.log \
	-projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
	-buildWindowsPlayer "$(pwd)/Build/windows/${UNITYCI_PROJECT_NAME}.exe" \
	-quit

rc1=$?
echo "Build logs (Windows)"
cat $(pwd)/unity.log


# Avvio del processo di build per la creazione di uno standalone in grado di operare su sistemi OSX
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

# In uscita controlla che i processi siano terminati senza errori
exit $(($rc1|$rc2))
