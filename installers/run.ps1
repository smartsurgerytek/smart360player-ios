# Define paths and variables
$apkPath = "apks\smart360player-v0.3.0.apk"
$packageName = "com.smartsurgery.smart360player"
$legacyPackageName = "com.SmartSurgery.Smart360Player"
$remoteDataPath = "/sdcard/Android/data/$packageName/files/"
$localDataPath = "$HOME\AppData\LocalLow\SmartSurgery\smart360player\Data"
$localVideosPath = "$HOME\AppData\LocalLow\SmartSurgery\smart360player\Videos"
$activityPath = "$packageName/com.unity3d.player.UnityPlayerActivity"


# Function to check if a file exists
function Check-FileExists {
    param([string]$filePath)
    if (-Not (Test-Path $filePath)) {
        Write-Host "File not found: $filePath" -ForegroundColor Red
        exit 1
    }
}

# Function to check if ADB device is connected
function Check-DeviceConnected {
    Write-Host "Checking if the device is connected..."
    $adbDevices = & adb devices
    if (-not ($adbDevices -match "device$")) {
        Write-Host "No device connected or ADB not authorized." -ForegroundColor Red
        exit 1
    }
}

# Function to check if the app is already installed
function Check-AppInstalled {
    param([string]$packageName)
    Write-Host "Checking if $packageName is installed..."
    $appInstalled = & adb shell pm list packages | Select-String $packageName -CaseSensitive
    return $appInstalled
}

# Function to create directory on the device
function Create-RemoteDirectory {
    param([string]$directoryPath)
    Write-Host "Creating directory $directoryPath on device..."
    & adb shell mkdir -p $directoryPath
    if ($?) {
        Write-Host "Directory $directoryPath created successfully." -ForegroundColor Green
    } else {
        Write-Host "Failed to create directory $directoryPath." -ForegroundColor Red
        exit 1
    }
}
# Function to grant storage permissions to the app
function Grant-Permissions {
    param([string]$packageName)
    Write-Host "Granting READ_EXTERNAL_STORAGE and WRITE_EXTERNAL_STORAGE permissions to $packageName..."
    & adb shell pm grant $packageName android.permission.READ_EXTERNAL_STORAGE
    & adb shell pm grant $packageName android.permission.WRITE_EXTERNAL_STORAGE

    if ($?) {
        Write-Host "Permissions granted successfully." -ForegroundColor Green
    } else {
        Write-Host "Failed to grant permissions." -ForegroundColor Red
        exit 1
    }
}
# Function to push files to the device
function Push-Files {
    param([string]$source, [string]$destination)
    Write-Host "Pushing files from $source to $destination..."
    & adb push --sync $source $destination
    if ($?) {
        Write-Host "Files pushed successfully." -ForegroundColor Green
    } else {
        Write-Host "Failed to push files." -ForegroundColor Red
        exit 1
    }
}

# Function to launch the app
function Launch-App {
    param([string]$activity)
    Write-Host "Launching $activity..."
    & adb shell am start -n $activity
    if ($?) {
        Write-Host "$activity launched successfully." -ForegroundColor Green
    } else {
        Write-Host "Failed to launch $activity." -ForegroundColor Red
    }
}

function Stop-App {
    param([string]$packageName)
    Write-Host "Stopping $packageName..."
    & adb shell am force-stop $packageName
    if ($?) {
        Write-Host "$packageName Stop successfully." -ForegroundColor Green
    } else {
        Write-Host "Failed to Stop $packageName." -ForegroundColor Red
    }
}

# Step 1: Check if ADB device is connected
Check-DeviceConnected

# Step 2: Check if the APK file exists
Check-FileExists $apkPath

# Step 3: Uninstall the Legacy App
if ((Check-AppInstalled $legacyPackageName)){
    Write-Host "Uninstalling $legacyPackageName"
    & adb uninstall $legacyPackageName
    if ($?) {
        Write-Host "Package uninstall successfully." -ForegroundColor Green
    } else {
        Write-Host "Faied to uninstall package." -ForegroundColor Red
    }
}

# Step 4: Install the APK
if (-Not (Check-AppInstalled $packageName)) {
    Write-Host "Installing APK from $apkPath..."
    & adb install $apkPath
    if ($?) {
        Write-Host "APK installed successfully." -ForegroundColor Green
    } else {
        Write-Host "Failed to install APK." -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "$packageName is already installed." -ForegroundColor Green
}

# Step 5: Create the directory on the Quest 2
Create-RemoteDirectory $remoteDataPath

# Step 6: Check if local data and videos directories exist
Check-FileExists $localDataPath
Check-FileExists $localVideosPath

# Step 7: Push Files to Quest 2
Grant-Permissions $packageName
Push-Files $localDataPath $remoteDataPath
Push-Files $localVideosPath $remoteDataPath

# Step 8: Launch app in Quest 2
Launch-App $activityPath