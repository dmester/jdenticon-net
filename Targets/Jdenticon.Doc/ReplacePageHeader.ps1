Param(
  [string]$htmlPath,
  [string]$headerPath,
  [string]$footerPath
)

$oldHeader = '<div class="pageHeader" id="PageHeader">Jdenticon-net documentation<form id="SearchForm" method="get" action="#" onsubmit="javascript:TransferToSearchPage(); return false;"><input id="SearchTextBox" type="text" maxlength="200" /><button id="SearchButton" type="submit"></button></form></div>';
$newHeader = Get-Content $headerPath

$oldFooter = '<div id="pageFooter" class="pageFooter"><p><a href="http://jdenticon.com/" target="_blank">Copyright</a></p> </div>';
$newFooter = Get-Content $footerPath
$newFooter = $newFooter.replace("<year>2000</year>", [DateTime]::Now.Year.ToString())

Get-ChildItem $htmlPath -Filter *.htm | 
Foreach-Object {
    $content = Get-Content $_.FullName
	$content = $content.replace($oldHeader, $newHeader)
	$content = $content.replace($oldFooter, $newFooter)
	$content = $content.replace('<link rel="shortcut icon" href="../icons/favicon.ico" />', "");
	$content = $content.replace('</title>', ' - Jdenticon for .NET</title>');

	Set-Content -Path $_.FullName -Value $content -Encoding UTF8
}
