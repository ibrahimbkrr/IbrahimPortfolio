# IbrahimPortfolio

.NET 8 ile geliştirilen kişisel portfolio ve içerik yönetim uygulaması. Public site Start Bootstrap Resume, yönetim alanı SB Admin tasarımını kullanır. WebUI verileri typed HttpClient servisleriyle Web API üzerinden alır.

## Kullanılan Teknolojiler

.NET 8, ASP.NET Core MVC ve Web API, Entity Framework Core 8, SQL Server, AutoMapper, Generic Repository, Dependency Injection, Typed HttpClient, Cookie Authentication, Razor Views, ViewComponents ve Bootstrap. Public arayüz Start Bootstrap Resume, admin arayüz SB Admin üzerine kuruludur.

## Mimari

| Proje | Sorumluluk |
| --- | --- |
| IbrahimPortfolio.Entity | About, Experience, Education, Skill, Project, Certificate, SocialMedia, Message entity'leri |
| IbrahimPortfolio.DataAccess | SQL Server, EF Core 8, AppDbContext, Generic Repository ve migration'lar |
| IbrahimPortfolio.Dto | İstek/yanıt DTO'ları, DataAnnotations ve URL/tarih doğrulaması |
| IbrahimPortfolio.Business | Service interface'leri, Manager sınıfları ve AutoMapper mapping'leri |
| IbrahimPortfolio.WebApi | CRUD endpointleri, doğrulama, hata yanıtları ve yönetim API anahtarı kontrolü |
| IbrahimPortfolio.WebUI | ASP.NET Core MVC, Admin Area, Cookie Authentication, ViewComponents ve typed HttpClient |

Veri akışı: WebUI → WebApi → Business → DataAccess → SQL Server. Entity ve DTO sınırları korunur.

## Özellikler

- Hakkımda, deneyim, eğitim, yetenek, proje ve sertifikalar API'den dinamik görüntülenir.
- Aktif sosyal medya hesapları sıralanır; CV, proje ve sertifika bağlantıları desteklenir.
- İletişim formu sunucuda doğrulanır. Yeni mesajın tarihi sunucuda atanır, `IsRead = false` olur.
- Admin: Hakkımda güncelleme; deneyim, eğitim, yetenek, proje, sertifika ve sosyal medya CRUD işlemleri.
- Mesaj listesi, detay, okundu işaretleme ve silme. Görüntüle düğmesi CSRF korumalı POST ile okundu yapıp detay sayfasına yönlendirir. Doğrudan GET detay isteği veri değiştirmez.
- Dashboard paralel API çağrılarıyla sayaçları ve son beş mesajı yükler; alınamayan sayaçlar `—` olarak gösterilir.
- Boş listeler, API kesintisi, başarısız işlemler ve 404 durumları için kullanıcı mesajları bulunur.

## Public Portfolio

About, Experience, Education, Skill, Project, Certificate ve SocialMedia verileri API üzerinden alınır. Hakkımda, deneyim, eğitim, yetenek, proje ve sertifika bölümleri ViewComponents ile oluşturulur. Contact formu ziyaretçilerin mesajlarını API'ye gönderir. Admin panelindeki değişiklikler sayfa yeniden açıldığında public içeriğe yansır.

## Admin Panel

Dashboard proje/yetenek/sertifika sayılarını, okunmamış mesaj sayısını ve son beş mesajı gösterir. About ekranında profil güncellenir; Experience, Education, Skill, Project, Certificate ve SocialMedia ekranlarında ekleme, düzenleme ve silme yapılır. Messages ekranı mesaj listesi, detay, okundu işaretleme ve silmeyi sunar. Yönetim alanı Cookie Authentication ve Admin rolüyle korunur.

## Kurulum

Gereksinimler: .NET 8 SDK, SQL Server/SQL Server Express ve EF Core 8 CLI aracı. Smoke testi için Windows ve PowerShell 7 kullanılır. Komutları solution kökünden çalıştırın.

1. Repository'yi klonlayın. Aşağıdaki `GITHUB_KULLANICI_ADI` alanını repository sahibinin GitHub kullanıcı adıyla değiştirin.

```powershell
git clone https://github.com/GITHUB_KULLANICI_ADI/IbrahimPortfolio.git
cd IbrahimPortfolio
```

2. Bağımlılıkları geri yükleyip Release derlemesi alın.

```powershell
dotnet restore IbrahimPortfolio.sln --disable-parallel -m:1
dotnet build IbrahimPortfolio.sln --no-restore -c Release -m:1
```

Çalışan API/WebUI derleme dosyalarını kilitliyorsa ilgili geliştirme oturumunu durdurun veya `-c Release` ile ayrı çıktıda derleyin.

### 3. SQL Server bağlantısı

Makineye özel connection string repository'de tutulmaz. SQL Server instance adını kendi kurulumunuza göre değiştirip Development ayarını WebApi User Secrets'a kaydedin:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\SQLEXPRESS;Database=IbrahimPortfolioDb;Trusted_Connection=True;TrustServerCertificate=True;" --project IbrahimPortfolio.WebApi
```

Bu örnek Windows Authentication kullanır. SQL kullanıcı adı/parolası gereken ortamlarda gerçek bağlantıyı yalnızca User Secrets veya güvenli environment configuration ile sağlayın. `TrustServerCertificate=True` yerel geliştirme içindir; production TLS ayarlarını ortamınıza göre yapılandırın.

### 4. Admin hesabı ve API anahtarı

WebUI ve WebApi proje dosyalarında `UserSecretsId` tanımlıdır; `dotnet user-secrets init` çalıştırmanız gerekmez. Kullanıcı adı örnektir; parola ve anahtar placeholder'larını kendi güçlü değerlerinizle değiştirin.

```powershell
dotnet user-secrets set "AdminUser:Username" "admin" --project IbrahimPortfolio.WebUI
dotnet user-secrets set "AdminUser:Password" "YOUR_SECURE_PASSWORD" --project IbrahimPortfolio.WebUI

# Aynı uzun, rastgele anahtarı iki projeye de yazın; aşağıdakini gerçek anahtarınızla değiştirin.
dotnet user-secrets set "ApiSettings:AdminApiKey" "YOUR_RANDOM_SHARED_API_KEY" --project IbrahimPortfolio.WebUI
dotnet user-secrets set "ApiSettings:AdminApiKey" "YOUR_RANDOM_SHARED_API_KEY" --project IbrahimPortfolio.WebApi

```

Örnek parolaları/anahtarları kullanmayın. Gerçek değerleri source code veya appsettings dosyalarına yazmayın. User Secrets yalnızca Development içindir ve şifreli bir production secret store değildir. Önceki kaynak kodundaki parolayı kullandıysanız değiştirin.

## Database — 5. Migration uygulama

Mevcut migration'lar `IbrahimPortfolio.DataAccess/Migrations` altındadır. Bu iyileştirmeler entity/DB şemasını değiştirmez; yeni migration gerekmez. İlk kurulumda veya mevcut migration'ları uygulamak için:

```powershell
# Yüklü değilse:
dotnet tool install --global dotnet-ef --version 8.0.16
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project IbrahimPortfolio.DataAccess --startup-project IbrahimPortfolio.WebApi
```

Veritabanı adı `IbrahimPortfolioDb`'dir. Migration örnek portfolio kaydı eklemez. Boş kurulumda ilk Hakkımda kaydını `POST /api/About` ile, `X-Admin-Api-Key` başlığını kullanarak ekleyin; ardından admin panelinden güncelleyebilirsiniz. Diğer içerikler panelden eklenebilir.

## Projeyi Çalıştırma — 6–7. API ve WebUI

İki ayrı terminalde:

```powershell
dotnet run --project IbrahimPortfolio.WebApi --launch-profile http
dotnet run --project IbrahimPortfolio.WebUI --launch-profile http
```

- API: `http://localhost:5000`, Development Swagger: `/swagger`.
- WebUI: `http://localhost:5273`, admin giriş: `/Admin/Login`, dashboard: `/Admin/Dashboard`.
- Development API adresi WebUI `appsettings.Development.json` içindeki `ApiSettings:BaseUrl` üzerinden ayarlanır.
- HTTPS profilleri de mevcuttur; iki uygulamanın adreslerini uyumlu yapılandırın.

Bu adresler mevcut `Properties/launchSettings.json` dosyalarındaki `http` profillerine aittir. `https` profilleri WebApi için `https://localhost:7147`, WebUI için `https://localhost:7244` adreslerini de açar. Portları değiştirirseniz WebUI `ApiSettings:BaseUrl` değerini de yeni API adresine göre güncelleyin. Başlatma logundaki `Now listening on` adresi çalışmakta olan uygulamanın adresidir.

## Güvenlik

Admin controller'ları `[Authorize(Roles = "Admin")]` kullanan `AdminBaseController` üzerinden korunur. Login anonymous erişime açıktır. Cookie HttpOnly'dir, iki saat geçerlidir ve sliding expiration kullanır. Production'da Secure cookie ve HTTPS API adresi zorunludur. Logout POST ile cookie'yi siler; admin yanıtları cache edilmez.

WebUI POST işlemleri anti-forgery doğrulaması kullanır. Silme ve okundu işlemleri GET ile çalışmaz. DTO doğrulaması WebUI ve API'de uygulanır; opsiyonel alanlar nullable kalır. Bağlantılarda HTTP/HTTPS, görsel/CV alanlarında ayrıca `/` ile başlayan yerel yollar kabul edilir. Yeni sekme bağlantıları `noopener noreferrer` kullanır.

API'nin yazma işlemleri ve mesaj okuma endpointleri `X-Admin-Api-Key` ister. Anahtar yoksa erişim reddedilir. WebUI bu anahtarı yalnızca giriş yapmış adminin sunucu tarafındaki isteklerine ekler; tarayıcıya göndermez. Public portfolio GET istekleri ve iletişim formunun `POST /api/Message` endpointi anonymous kalır.

Production ayarlarını güvenli configuration/environment üzerinden sağlayın:

| Değişken | Uygulama |
| --- | --- |
| `AdminUser__Username`, `AdminUser__Password` | WebUI |
| `ApiSettings__BaseUrl` | WebUI, HTTPS API adresi |
| `ApiSettings__AdminApiKey` | WebUI ve WebApi, aynı değer |
| `ConnectionStrings__DefaultConnection` | WebApi |

## API

`api/About`, `api/Experience`, `api/Education`, `api/Skill`, `api/Projects`, `api/Certificate`, `api/SocialMedia`: GET liste, GET `{id}`, POST oluşturma, PUT güncelleme, DELETE `{id}`. Güncelleme DTO'su ID içerir. Bulunamayan kayıtlar 404, geçersiz DTO'lar 400 döner.

`api/Message`: POST iletişim mesajı; korumalı GET liste/`{id}`, PUT `{id}/read`, DELETE `{id}`. Genel mesaj güncelleme endpointi kaldırılmıştır; okundu işlemi gönderen, içerik ve tarihi değiştiremez.

## Kontrol

Yeni test paketi eklemeden çalışan kontroller:

```powershell
dotnet restore tests/IbrahimPortfolio.Checks --disable-parallel -m:1
dotnet build tests/IbrahimPortfolio.Checks --no-restore -c Release -m:1
dotnet tests/IbrahimPortfolio.Checks/bin/Release/net8.0/IbrahimPortfolio.Checks.dll
pwsh -File tests/Smoke.ps1
```

Kod kontrolleri mapping, CRUD manager'ları, DTO doğrulaması, API yetkileri ve hata yönetimini sınar. HTTP kontrolleri gerçek WebUI'ı ve gerçek API controller/manager'larını, ayrı bellek repository'leriyle çalıştırır; SQL verilerinizi değiştirmez. Testler 55180 ve 55181 portlarını kullanır, geçici test parolaları/anahtarları üretir ve bitince kendi süreçlerini durdurur. Loglar Git dışında tutulan `.artifacts` altındadır.

İsteğe bağlı gerçek SQL mesaj testi (eklenen test kaydı aynı transaction içinde silinir ve transaction geri alınır; mevcut kayıtlara dokunulmaz):

```powershell
$env:PORTFOLIO_TEST_SQL = "Server=.\SQLEXPRESS;Database=IbrahimPortfolioDb;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet tests/IbrahimPortfolio.Checks/bin/Release/net8.0/IbrahimPortfolio.Checks.dll --check-sql
Remove-Item Env:PORTFOLIO_TEST_SQL
```

Giriş yapmadan admin URL'lerini, hatalı/doğru login'i, logout'u; tüm Create/Update/Delete formlarını; public sayfaya yansıyan içerikleri ve iletişim → okunmamış sayaç → görüntüle → okundu akışını kontrol edin. API'yi durdurduğunuzda sayfa açılmalı ve uyarı gösterilmelidir. Veritabanına bağlı uçtan uca sonuçlar için yerel SQL Server bağlantınız çalışır durumda olmalıdır.

## Ekran Görüntüleri

Public portfolio, admin dashboard ve yönetim ekranlarının görüntüleri daha sonra bu bölüme eklenecektir.

## Repository Düzeni

Kaynak kod, proje dosyaları, migration'lar, test kaynakları ve gerekli `wwwroot` tema dosyaları repository'de bulunur. Derleme/test çıktıları, IDE önbellekleri, loglar, `.env` dosyaları, yerel configuration ve sertifika/private key dosyaları Git dışında tutulur. Tema ve üçüncü taraf kütüphanelerdeki mevcut lisans/copyright bildirimleri korunur.
