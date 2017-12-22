#! /bin/sh

# Download dell'installer di Unity3D all'interno della macchina virtuale
# La versione scaricata e' la 2017.2.03
echo 'Downloading Unity 2017.2.0f3 pkg:'
curl --retry 5 -o Unity.pkg https://netstorage.unity3d.com/unity/46dda1414e51/MacEditorInstaller/Unity-2017.2.0f3.pkg
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# In Unity gli editor contengono esclusivamente le informazioni per costruire build sulla piattaforma dove opera l'editor gli altri moduli vanno aggiunti separatamente
# Nel caso in esempio vengono scaricati i file di supporto per effettuare la build che supporti sistemi Windows
echo 'Downloading Unity 2017.2.0f3 Windows Build Support pkg:'
curl --retry 5 -o Unity_win.pkg https://beta.unity3d.com/download/46dda1414e51/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.2.0f3.pkg
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# Avvio degli installer sulla macchina virtuale
echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
echo 'Installing Unity_win.pkg'
sudo installer -dumplog -package Unity_win.pkg -target /
