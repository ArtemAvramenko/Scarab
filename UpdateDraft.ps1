$ra = @(([Reflection.Assembly]::LoadWithPartialName("System.Xml") | Select -ExpandProperty "Location"))
(Add-Type -Path "UpdateDraft.cs" -PassThru -ReferencedAssemblies $ra)
[UpdateDraft]::Generate(
"$([System.Environment]::GetFolderPath([System.Environment+SpecialFolder]::Desktop))\CLDR_common",
"${PSScriptRoot}/draft.csv")
