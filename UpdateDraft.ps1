$CLDR_path = "$([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop))/cldr/common"

$Out_path = "${PSScriptRoot}/draft.csv"

$ra = @(([Reflection.Assembly]::LoadWithPartialName("System.Xml") | Select -ExpandProperty "Location"))
(Add-Type -Path "UpdateDraft.cs" -PassThru -ReferencedAssemblies $ra)
[UpdateDraft]::Generate($CLDR_path, $Out_path)
