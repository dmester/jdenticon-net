Param(
  [string]$htmlPath,
  [string]$headerPath
)

$oldHeader = '<div class="pageHeader" id="PageHeader">Jdenticon-net documentation<form id="SearchForm" method="get" action="#" onsubmit="javascript:TransferToSearchPage(); return false;"><input id="SearchTextBox" type="text" maxlength="200" /><button id="SearchButton" type="submit"></button></form></div>';
$newHeader = Get-Content $headerPath

Get-ChildItem $htmlPath -Filter *.htm | 
Foreach-Object {
    $content = Get-Content $_.FullName
	$content = $content.replace($oldHeader, $newHeader)

	Set-Content -Path $_.FullName -Value $content -Encoding UTF8
}
