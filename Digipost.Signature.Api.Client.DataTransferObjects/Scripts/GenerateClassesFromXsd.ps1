Write-Host "-> RUNNING THE FIRST TIME:"
Write-Host "   Set-ExecutionPolicy Bypass"
Write-Host "- If on Vm-Ware host and script on Z: drive:"
Write-Host "   net use Z: `"\\vmware-host\Shared Folders`""

function Run-Xsd($Xsd, $OutDir)
{
    Write-Host("XSD: $Xsd")
    Write-Host("OUT: $OutDir")
    
    $CurrentDirectory = (Get-Item -Path ".\" -Verbose).FullName
    Set-Location "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools"    
    Invoke-Expression ".\Xsd.exe $Xsd /classes /out:$OutDir"
    Set-Location $CurrentDirectory
}

function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

$ScriptsDir = Get-ScriptDirectory
$RootDir =  (Get-Item $ScriptsDir).Parent.FullName
$XsdDir = "$RootDir\Xsd"

$DirectXsd = "$XsdDir\direct.xsd"
$PortalXsd = "$XsdDir\portal.xsd"

Run-Xsd $PortalXsd "$RootDir\Portal"
Run-Xsd $DirectXsd "$RootDir\Direct"


Read-Host "Enter to quit"