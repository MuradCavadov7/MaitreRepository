# MaitreRepository
# Maitre Kiosk API

Restoran kiosk sifariş prosesinin sadələşdirilmiş backend API-si: məhsullar,
sifarişlər, status axını, sifariş siyahısı və ən çox satılan məhsullar.

**Texnologiyalar:** C# / ASP.NET Core (.NET 10), EF Core 10, SQLite, AutoMapper, FluentValidation.

## Layihəni necə işə salmaq

Tələb: [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0). Başqa heç nə lazım deyil.

```bash
git clone [REPO-LINKI]
cd [REPO-QOVLUĞU]
dotnet run --project Maitre
```

İşə düşəndə tətbiq migration-ları avtomatik tətbiq edir: SQLite bazası
(`maitre.db`) yaranır və restoranlar seed olunur. Sonra Swagger-i açın:

- https://localhost:7134/swagger
- http://localhost:5251/swagger

Qeyd: `dotnet run` ilə brauzer avtomatik açılmaya bilər, ünvanı əl ilə daxil edin.

### Seed data
| RestaurantId | Ad |
|---|---|
| 1 | KFC |
| 2 | MC |
| 3 | PopEyes |

Restoranlar üçün endpoint yoxdur, yalnız seed data kimi mövcuddur.
Məhsullar seed olunmur, əvvəlcə `POST /api/Products?restaurantId=1` ilə məhsul yaradın.

## Endpoint-lər

`restaurantId` query parametri kimi göndərilir. [Route-ları Swagger-dəki real ünvanlarla yoxlayın.]

| # | Əməliyyat | Endpoint |
|---|---|---|
| 1 | Məhsul yaratmaq | `POST /api/Products?restaurantId=1` |
| 2 | Məhsulu yeniləmək (qiymət / satışda olması) | `PATCH /api/Products/{productId}?restaurantId=1` |
| 3 | Sifariş yaratmaq | `POST /api/Orders/orders?restaurantId=1` |
| 4 | Sifariş statusunu dəyişmək | `PATCH /api/Orders/orders/{orderId}/status?restaurantId=1` |
| 5 | Sifariş siyahısı (status filter + pagination) | `GET /api/Orders/orders?restaurantId=1&status=Pending&page=1&pageSize=10` |
| 6 | Ən çox satılan 5 məhsul | `GET /api/Orders/top-products?restaurantId=1` |

Əlavə (tələb olunmayan): məhsul siyahısı, məhsul silmək, sifariş silmək.

Sifariş yaratma nümunəsi:

```json
{ "orderItems": [ { "productId": 1, "quantity": 2 } ] }
```

Status dəyişmə nümunəsi:

```json
{ "status": "Preparing" }
```

## Arxitektura və əsas qərarlar

N-tier quruluş, asılılıqlar içəriyə doğrudur:

```
Maitre (API) -> Maitre.BL (servislər, DTO, validator, mapping) -> Maitre.DAL (EF Core, repository) -> Maitre.Core (entity, interfeys, exception)
```

- **N-tier, BaseEntity və repository pattern.** Controller-lərdə məntiq yoxdur,
  servislər biznes qaydalarını, repository-lər məlumat girişini daşıyır.
  Generic repository ümumi əməliyyatları edir, entity-yə xas sorğular
  (səhifələnmiş sifarişlər, top məhsullar) `OrderRepository` / `OrderItemRepository`-dədir.
- **Sifariş sətirində ad və qiymət snapshot-ı.** `OrderItem` sifariş anındakı
  `ProductName` və `UnitPrice`-ı saxlayır. Məhsulun qiyməti sonradan dəyişsə,
  köhnə sifarişlər və onların məbləği dəyişmir.
- **Məbləğ serverdə hesablanır.** Client yalnız məhsul id və say göndərir, qiymət
  göndərmir, ona görə qiyməti saxtalaşdırmaq mümkün deyil. Sifarişin cəmi
  yaradılanda hesablanıb saxlanılır.
- **Status axını keçid cədvəli ilə** (`Dictionary<OrderStatus, OrderStatus[]>`):
  icazə verilən keçidlər aydın görünür və asan dəyişdirilir.
- **FluentValidation** controller səviyyəsində avtomatik işləyir
  (SharpGrip AutoValidation), servislər yalnız biznes qaydaları ilə məşğuldur.
- **Mərkəzləşdirilmiş xəta idarəsi.** Custom exception-lar bir middleware-də HTTP
  koduna çevrilir: 400, 404, 409, gözlənilməz xəta üçün 500.
- **N+1 sorğu yoxdur.** Sifarişin məhsulları bir sorğu ilə yüklənir, siyahıda
  sətirlər `Include` ilə gəlir, Top 5 tək qruplaşdırılmış SQL sorğusudur.
- **Audit.** `CreatedAt` / `UpdatedAt` `SaveChanges` zamanı avtomatik doldurulur.
- **SQLite:** əlavə quraşdırma olmadan işə düşmək üçün. Enum-lar oxunaqlı olsun deyə string kimi saxlanılır.
- **Migration-lar başlanğıcda tətbiq olunur**, beləliklə layihə tək əmrlə açılır.
- **AutoMapper:** [versiyanı və lisenziya vəziyyətini yazın, məsələn "14.0.0" və ya "16.x, community lisenziyası"].

## Açıq məqamlar üçün qərarlar

1. **Satışda olmayan məhsul** sifarişə əlavə edilə bilməz (409 Conflict).
2. **Başqa restoranın məhsulu** sifarişə əlavə edilə bilməz; həmin restoran üçün "tapılmadı" kimi qaytarılır (404).
3. **Eyni məhsul bir sifarişdə təkrar gəlsə**, sətirlər birləşdirilir (say toplanır).
4. **Limitlər:** bir məhsul üzrə say 1-100; boş sifariş rədd edilir; qiymət > 0 və
   ən çox 2 onluq rəqəm olmalıdır.
5. **Yanlış status keçidi** 409 qaytarır. Ləğv yalnız `Pending` statusunda mümkündür.
   `Completed` və `Cancelled` son statuslardır.
6. **Sifariş siyahısı:** default `page=1`, `pageSize=10`, maksimum `pageSize=50`;
   ən yeni sifariş birinci. Yanlış səhifələmə dəyərləri 400 qaytarır.
7. **Top 5:** satılan ümumi saya görə; ləğv edilmiş sifarişlər sayılmır; bərabərlik
   olarsa məhsul adı üzrə əlifba sırası. Qruplaşdırma məhsul id-si ilə aparılır.
8. **Məhsul yeniləmə** qismən yeniləmədir: `price`, `isAvailable` və ya hər ikisi
   göndərilir. Heç biri göndərilməsə 400. Mövcud sifarişlər dəyişmir.
9. **Mövcud olmayan restoran id-si.** Restoranlar seed data olduğu üçün ayrıca
   yoxlanılmır: siyahılar boş qayıdır, mövcud olmayan məhsul/sifariş üzərində
   əməliyyat 404 verir. [Bu halı ayrıca həll etmisinizsə yazın.]
10. **Silmə:** sifarişdə istifadə olunmuş məhsul silinmir (409), əvəzinə satışdan
    çıxarılır. Yalnız ləğv edilmiş sifarişlər silinə bilər.

## Sərf olunan vaxt

Tapşırığa təxminən 6 saat sərf etdim, bu, tövsiyə olunan 3 saatdan çoxdur.
Əlavə vaxtın əsas səbəbləri:

- Bəzi texnologiya və pattern-ləri (n-tier arxitektura, generic repository,
  FluentValidation, AutoMapper, EF Core migration-ları) təkrar araşdırıb
  yadıma salmalı oldum.
- Quraşdırma zamanı rast gəldiyim xətaları (paket versiyalarının uyğunsuzluğu,
  AutoMapper-in DI inteqrasiyası, mapping ad uyğunsuzluqları) araşdırıb düzəltdim.
- İlk versiyalarda etdiyim səhvləri və dizayn qərarlarını yenidən nəzərdən keçirib
  düzəltdim ki, həll biznes qaydalarına düzgün uyğun gəlsin.

Sürətlə yarımçıq bir həll təqdim etmək əvəzinə, düzgün işləyən və başa düşdüyüm
bir həll təqdim etməyi üstün tutdum.

## Daha çox vaxtım olsaydı nəyi dəyişərdim

- **Paralellik (concurrency):** məhsul mövcudluq yoxlaması ilə sifarişin yazılması arasında
  satışdan çıxarıla bilər. Concurrency token və/və ya tranzaksiya əlavə edərdim.
- **Avtomatik testlər:** status keçid qaydaları və məbləğ hesablaması üçün unit testlər,
  API üçün `WebApplicationFactory` ilə integration testlər.
- **Pul tipi:** SQLite-da decimal mətn kimi saxlanılır; PostgreSQL-də `numeric(18,2)` istifadə edərdim.
- **Status dəyişikliyini** servisdəki cədvəl əvəzinə `Order` entity-sinin metodu kimi yazardım.
- **Məhsul siyahısına pagination** və xəta cavablarında ProblemDetails (RFC 7807).
- **Docker / docker-compose** və PostgreSQL seçimi.
- **Structured logging** və health check.
- **Status tarixçəsi cədvəli:** kim, nə vaxt statusu dəyişib.

