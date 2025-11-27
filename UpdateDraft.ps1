$ra = @(([Reflection.Assembly]::LoadWithPartialName("System.Xml") | Select -ExpandProperty "Location"))
(Add-Type -Path "UpdateDraft.cs" -PassThru -ReferencedAssemblies $ra)

$CLDR_path = "$([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop))/cldr/common"
$Out_path = "${PSScriptRoot}/draft.csv"
[UpdateDraft]::Generate($CLDR_path, $Out_path)

$Csv_path = "${PSScriptRoot}/currencies.csv"
$Json_path = "${PSScriptRoot}/currencies.json"
[UpdateDraft]::ConvertToJson($Csv_path, $Json_path)
