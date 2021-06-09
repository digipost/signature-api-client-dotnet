# Running for the first time:
# Install Powershell with command ...
# brew install --cask powershell


function GenerateCode($XsdDirectory, $RootXsd, $OutDir)
{
    Write-Host("Starting to generate code from XSD ...")
    Write-Host("Xsd-directory: $XsdDirectory")
    Write-Host("Output-directory: $OutDir")

    Write-Host("Changing directory to $XsdDirectory ...")
    Set-Location $XsdDirectory

    Invoke-Expression "xsd $RootXsd /classes /outputdir:$OutDir"
}

function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

$CurrentScriptDir = Get-ScriptDirectory
$XsdToCodeDir = (Get-Item $CurrentScriptDir)

$CodeDir = "$XsdToCodeDir/Code"
$XsdPath = "$CurrentScriptDir/Xsd/"
$RootXsd = "direct-and-portal.xsd"

GenerateCode "$XsdPath" "$RootXsd" "$CodeDir"
