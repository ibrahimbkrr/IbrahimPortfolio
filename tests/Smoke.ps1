# PowerShell 7. Tests use real controllers/managers with isolated memory repositories.
# No SQL Server data is read or changed. Build Release outputs before running.
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$artifacts = Join-Path $root '.artifacts'
New-Item -ItemType Directory -Force $artifacts | Out-Null
$apiPort = 55180
$uiPort = 55181
$key = [Guid]::NewGuid().ToString('N')
$password = [Guid]::NewGuid().ToString('N')
$settings = @{
    ASPNETCORE_ENVIRONMENT='Development'; ASPNETCORE_URLS="http://127.0.0.1:$apiPort"
    ApiSettings__AdminApiKey=$key; ApiSettings__BaseUrl="http://127.0.0.1:$apiPort/"
    AdminUser__Username='smoke-admin'; AdminUser__Password=$password
    Logging__EventLog__LogLevel__Default='None'
}
$previous = @{}
$processes = @()
$script:checks = 0
function Assert($condition, $name) {
    if (!$condition) { throw "FAIL: $name" }
    $script:checks++
    Write-Output "PASS: $name"
}
function New-Client($url) {
    $handler = [Net.Http.HttpClientHandler]::new()
    $handler.AllowAutoRedirect = $false
    $client = [Net.Http.HttpClient]::new($handler)
    $client.BaseAddress = [Uri]$url
    $client.Timeout = [TimeSpan]::FromSeconds(30)
    return $client
}
function Send($client, $path, $method='GET', $data=$null) {
    $request = [Net.Http.HttpRequestMessage]::new([Net.Http.HttpMethod]::new($method), $path)
    if ($null -ne $data) {
        $values = [Collections.Generic.Dictionary[string,string]]::new()
        foreach ($entry in $data.GetEnumerator()) { $values[$entry.Key] = [string]$entry.Value }
        $request.Content = [Net.Http.FormUrlEncodedContent]::new($values)
    }
    try {
        $response = $client.SendAsync($request).GetAwaiter().GetResult()
        try { return @{ Status=[int]$response.StatusCode; Body=$response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); Location=[string]$response.Headers.Location } }
        finally { $response.Dispose() }
    } finally { $request.Dispose() }
}
function Token($page) {
    $match = [regex]::Match($page.Body, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"')
    if (!$match.Success) { throw 'Anti-forgery token missing' }
    return [Net.WebUtility]::HtmlDecode($match.Groups[1].Value)
}
function Api-List($name) { return ((Send $api "api/$name").Body | ConvertFrom-Json) }
try {
    foreach ($entry in $settings.GetEnumerator()) {
        $previous[$entry.Key] = [Environment]::GetEnvironmentVariable($entry.Key)
        [Environment]::SetEnvironmentVariable($entry.Key, $entry.Value)
    }
    $apiDll = Join-Path $root 'tests/IbrahimPortfolio.Checks/bin/Release/net8.0/IbrahimPortfolio.Checks.dll'
    $apiProcess = Start-Process dotnet -ArgumentList @('"' + $apiDll + '"','--serve-api') -WindowStyle Hidden -PassThru -RedirectStandardOutput "$artifacts/smoke-api.log" -RedirectStandardError "$artifacts/smoke-api-error.log"
    $processes += $apiProcess
    $env:ASPNETCORE_URLS = "http://127.0.0.1:$uiPort"
    $uiRoot = Join-Path $root 'IbrahimPortfolio.WebUI'
    $uiProcess = Start-Process dotnet -WorkingDirectory $uiRoot -ArgumentList @('bin/Release/net8.0/IbrahimPortfolio.WebUI.dll') -WindowStyle Hidden -PassThru -RedirectStandardOutput "$artifacts/smoke-ui.log" -RedirectStandardError "$artifacts/smoke-ui-error.log"
    $processes += $uiProcess
    $web = New-Client "http://127.0.0.1:$uiPort/"
    $api = New-Client "http://127.0.0.1:$apiPort/"
    $anonymousApi = New-Client "http://127.0.0.1:$apiPort/"
    $api.DefaultRequestHeaders.Add('X-Admin-Api-Key', $key)
    for ($attempt=0; $attempt -lt 40; $attempt++) {
        try { $login = Send $web '/Admin/Login'; $ready = Send $api 'api/About'; break } catch { Start-Sleep -Milliseconds 250 }
    }
    Assert ($login.Status -eq 200) 'Login page is anonymous'
    Assert ((Send $web '/Admin/Dashboard').Status -eq 302) 'Dashboard requires authentication'
    Assert ((Send $anonymousApi 'api/Message').Status -eq 401) 'Messages are private at API'
    Assert ((Send $anonymousApi 'api/Skill/1' 'DELETE').Status -eq 401) 'Anonymous API delete blocked'
    Assert ((Send $web '/Admin/Login' 'POST' @{ Username='smoke-admin'; Password=$password }).Status -eq 400) 'Login requires CSRF token'
    $bad = Send $web '/Admin/Login' 'POST' @{ Username='smoke-admin'; Password='invalid'; __RequestVerificationToken=(Token $login) }
    Assert ($bad.Status -eq 200 -and $bad.Body.Contains('validation-summary-errors')) 'Invalid password rejected'
    $good = Send $web '/Admin/Login' 'POST' @{ Username='smoke-admin'; Password=$password; RememberMe='true'; __RequestVerificationToken=(Token $bad) }
    Assert ($good.Status -eq 302 -and $good.Location -match '/Admin/Dashboard') 'Login redirects to dashboard'
    $dashboard = Send $web '/Admin/Dashboard'
    Assert ($dashboard.Status -eq 200) 'Authenticated dashboard renders'
    Assert ($dashboard.Body -notmatch 'asp-(action|controller|for)=') 'Admin tag helpers render'
    foreach ($page in 'About','Experience','Education','Skill','Project','Certificate','SocialMedia','Message') {
        Assert ((Send $web "/Admin/$page").Status -eq 200) "$page sidebar route"
    }
    $public = Send $web '/'
    Assert ($public.Status -eq 200) 'Public home renders empty lists'
    Assert ($public.Body -notmatch 'asp-(action|controller|for)=') 'Public tag helpers render'
    $invalid = Send $web '/Admin/Skill/Create' 'POST' @{ Name='Test'; Level=101; __RequestVerificationToken=(Token $dashboard) }
    Assert ($invalid.Status -eq 200 -and @(Api-List 'Skill').Count -eq 0) 'Invalid form does not write API'
    $cases = @(
        @{ Name='Experience'; Api='Experience'; Field='CompanyName'; Data=@{ CompanyName='ExperienceSmoke'; Position='Developer'; Description='Description'; StartDate='2024-01-01'; IsCurrent='true' } },
        @{ Name='Education'; Api='Education'; Field='SchoolName'; Data=@{ SchoolName='EducationSmoke'; Department='Software'; Degree='Bachelor'; Description='Description'; StartDate='2020-01-01' } },
        @{ Name='Skill'; Api='Skill'; Field='Name'; Data=@{ Name='SkillSmoke'; Level=70 } },
        @{ Name='Project'; Api='Projects'; Field='Name'; Data=@{ Name='ProjectSmoke'; Description='Description'; Technologies='C#'; GithubUrl='https://example.com/' } },
        @{ Name='Certificate'; Api='Certificate'; Field='Name'; Data=@{ Name='CertificateSmoke'; Issuer='Issuer'; IssueDate='2024-01-01' } },
        @{ Name='SocialMedia'; Api='SocialMedia'; Field='Name'; Data=@{ Name='SocialSmoke'; Url='https://example.com/'; Icon='fab fa-github'; DisplayOrder=1; IsActive='true' } }
    )
    foreach ($case in $cases) {
        $form = Send $web "/Admin/$($case.Name)/Create"
        Assert ($form.Status -eq 200) "$($case.Name) create form"
        foreach ($input in [regex]::Matches($form.Body, '<input\b[^>]*type="date"[^>]*>')) {
            Assert ($input.Value -notmatch 'data-val-range') "$($case.Name) date has no numeric range validator"
        }
        $data = $case.Data.Clone(); $data.__RequestVerificationToken = Token $form
        Assert ((Send $web "/Admin/$($case.Name)/Create" 'POST' $data).Status -eq 302) "$($case.Name) create POST"
        $record = @(Api-List $case.Api)[0]
        Assert ($record.Id -gt 0) "$($case.Name) persisted through manager"
        $edit = Send $web "/Admin/$($case.Name)/Update/$($record.Id)"
        Assert ($edit.Status -eq 200) "$($case.Name) update form"
        foreach ($input in [regex]::Matches($edit.Body, '<input\b[^>]*type="date"[^>]*>')) {
            Assert ($input.Value -notmatch 'data-val-range') "$($case.Name) update date has no numeric range validator"
        }
        $data.Id = $record.Id; $data[$case.Field] = "$($case.Name)Updated"; $data.__RequestVerificationToken = Token $edit
        Assert ((Send $web "/Admin/$($case.Name)/Update" 'POST' $data).Status -eq 302) "$($case.Name) update POST"
        Assert ((Send $web '/').Body.Contains("$($case.Name)Updated")) "$($case.Name) public reflects update"
        Assert ((Send $web "/Admin/$($case.Name)/Delete/$($record.Id)").Status -eq 405) "$($case.Name) GET delete blocked"
        Assert ((Send $web "/Admin/$($case.Name)/Delete/$($record.Id)" 'POST' @{}).Status -eq 400) "$($case.Name) delete CSRF required"
        Assert ((Send $web "/Admin/$($case.Name)/Delete/$($record.Id)" 'POST' @{ __RequestVerificationToken=(Token $edit) }).Status -eq 302) "$($case.Name) delete POST"
        Assert (@(Api-List $case.Api).Count -eq 0) "$($case.Name) deleted"
        Assert ((Send $web "/Admin/$($case.Name)/Update/999").Status -eq 404) "$($case.Name) missing record page"
    }
    $about = Send $web '/Admin/About/Update/1'
    Assert ((Send $web '/Admin/About/Update' 'POST' @{ Id=1; FirstName='AboutUpdated'; LastName='Test'; Title='Developer'; Description='Description'; Email='test@example.com'; __RequestVerificationToken=(Token $about) }).Status -eq 302) 'About update'
    Assert ((Send $web '/').Body.Contains('AboutUpdated')) 'About public update'
    $contactPage = Send $web '/'
    $contact = @{ Name='ContactSender'; Email='sender@example.com'; Subject='ContactSubject'; MessageContent='OriginalMessage'; __RequestVerificationToken=(Token $contactPage) }
    Assert ((Send $web '/Home/SendMessage' 'POST' $contact).Status -eq 302) 'Public contact submission'
    $message = @(Api-List 'Message')[0]
    Assert (!$message.IsRead -and $message.CreatedAt) 'New message unread and dated'
    $dashboard = Send $web '/Admin/Dashboard'
    Assert ($dashboard.Body.Contains('ContactSubject') -and $dashboard.Body.Contains('table-warning')) 'Dashboard shows unread message'
    Assert ((Send $web "/Admin/Message/Open/$($message.Id)" 'POST' @{ __RequestVerificationToken=(Token $dashboard) }).Status -eq 302) 'Open message marks read'
    $read = @(Api-List 'Message')[0]
    Assert ($read.IsRead -and $read.MessageContent -eq $message.MessageContent -and $read.CreatedAt -eq $message.CreatedAt) 'Read preserves message content/date'
    $detail = Send $web "/Admin/Message/Detail/$($message.Id)"
    Assert ($detail.Status -eq 200 -and $detail.Body.Contains('OriginalMessage')) 'Message detail renders'
    Assert ((Send $web "/Admin/Message/Delete/$($message.Id)" 'POST' @{ __RequestVerificationToken=(Token $detail) }).Status -eq 302) 'Message delete'
    Assert (@(Api-List 'Message').Count -eq 0) 'Message removed'
    Stop-Process -Id $apiProcess.Id
    Assert ((Send $web '/Admin/Dashboard').Status -eq 200) 'Dashboard survives API offline'
    $offline = Send $web '/'
    Assert ($offline.Status -eq 200 -and $offline.Body.Contains('alert-warning')) 'Public home survives API offline with warning'
    Assert ((Send $web '/Admin/Login/Logout' 'POST' @{ __RequestVerificationToken=(Token $detail) }).Status -eq 302) 'Logout clears cookie'
    Assert ((Send $web '/Admin/Dashboard').Status -eq 302) 'Admin inaccessible after logout'
    Assert ((Send $web '/missing-page').Status -eq 404) 'Friendly 404'
    Write-Output "$script:checks HTTP checks passed. SQL persistence was not tested."
} finally {
    foreach ($process in $processes) { if (!$process.HasExited) { Stop-Process -Id $process.Id -ErrorAction SilentlyContinue } }
    foreach ($entry in $previous.GetEnumerator()) { [Environment]::SetEnvironmentVariable($entry.Key, $entry.Value) }
    foreach ($client in $web,$api,$anonymousApi) { if ($client) { $client.Dispose() } }
}

