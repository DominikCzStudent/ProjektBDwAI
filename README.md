# System rezerwacji hoteli - dokumentacja projektu
---
## 1. Opis projektu
Aplikacja System rezerwacji hoteli jest webową aplikacją typu CRUD, stworzoną w technologii ASP.NET Core MVC, zaprojektowaną zgodnie z wzorcem Model–View–Controller. Umożliwia zarządzanie hotelami, pokojami oraz rezerwacjami, z uwzględnieniem autoryzacji użytkowników, ról, walidacji danych oraz utrwalania danych w relacyjnej bazie danych.

Projekt bazy danych został zaprojektowany na poziomie logicznym i zaimplementowany w podejściu Code First z użyciem Entity Framework Core.
## 2. Wymagania systemowe
Do uruchomienia projektu wymagane są:
- .NET SDK 8.0
- SQL Server
- Visual Studio 2026
- Przeglądarka internetowa
## 3. Instalacja i uruchomienie projektu
Aby pobrać projekt, należy wpisać:
```
git clone https://github.com/DominikCzStudent/ProjektBDwAI.git
```
lub pobrać archiwum ZIP z GitHuba. W przypadku pobrania archiwum, należy je rozpakować, a następne uruchomić plik Projekt.slnx.

Aplikacja korzysta z SQL Servera. Łańcuch połączenia znajduje się w pliku appsettings.json:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=HotelReservationDb;Trusted_Connection=True;TrustServerCertificate=True"
},
```
W razie potrzeby należy dostosować nazwę serwera SQL.

Projekt nie wymaga ręcznego tworzenia bazy danych. W Konsoli Menadżera Pakietów w Visual Studio należy wpisać:
```
Update-Database
```
Spowoduje to automatyczne utworzenie bazy danych, wykonanie wszystkich migracji, a także utworzenie tabel aplikacyjnych oraz Identity. Migracje zostały przygotowane wcześniej i znajdują się w repozytorium projektu.

Żeby uruchomić aplikację, należy nacisnąć klawisz F5 lub zieloną strzałkę w Visual Studio.
## 4. Konta testowe i role użytkowników
Podczas pierwszego uruchomienia aplikacji automatycznie tworzony jest użytkownik administratora:
- Login: admin
- Hasło: Admin123!
- Rola: Admin

Administrator ma dostęp do:
- zarządzania hotelami,
- zarządzania pokojami,
- przeglądania wszystkich rezerwacji,
- edycji i usuwania danych.

Zwykły użytkownik może:
- zarejestrować się samodzielnie,
- logować się do systemu,
- przeglądać dostępne rezerwacje,
- tworzyć własne rezerwacje,
- przeglądać szczegóły swoich rezerwacji.

Konto administratora tworzone jest automatycznie podczas pierwszego uruchomienia aplikacji (seed danych).
## 5. Opis funkcjonalności aplikacji
Rezerwacje:
- tworzenie nowej rezerwacji (formularz z walidacją),
- wybór hotelu oraz pokoju,
- określenie dat pobytu,
- przeglądanie szczegółów rezerwacji,
- edycja i usuwanie rezerwacji (zgodnie z uprawnieniami).

Hotele (tylko administrator):
- dodawanie hoteli,
- edycja danych hotelu,
- usuwanie hoteli,
- przegląd listy hoteli.

Pokoje (tylko administrator):
- dodawanie pokoi do hoteli,
- określenie numeru, pojemności i ceny za noc,
- edycja i usuwanie pokoi,
- przegląd listy pokoi.
## 6. Walidacja danych:
Każdy formularz w aplikacji zawiera walidację:
- po stronie serwera (Data Annotations),
- po stronie klienta (jQuery Validation).

Przykłady:
- pola wymagane (Required),
- ograniczenia długości,
- poprawność zakresów dat,
- komunikaty walidacyjne w języku polskim.
## 7. Model danych
Aplikacja zawiera 4 encje, powiązane relacjami:
- Hotel
- Room
- Reservation
- ApplicationUser (Identity)
Relacje:
- Hotel → Rooms (1:N)
- Room → Reservations (1:N)
- ApplicationUser → Reservations (1:N)

Encja ApplicationUser jest realizowana z użyciem ASP.NET Core Identity, co skutkuje wygenerowaniem kilku tabel technicznych w bazie danych (AspNetUsers, AspNetRoles, itd.), jednak logicznie stanowi jedną encję użytkownika.
## 8. API (CRUD)
Aplikacja zawiera REST API CRUD odnoszące się do głównej encji systemu – rezerwacji. API zostało zaimplementowane jako kontroler typu ApiController, niezależny od warstwy widoków MVC. API jest dostępne po uruchomieniu aplikacji i wymaga uwierzytelnienia użytkownika (ASP.NET Core Identity). Dane są przesyłane i zwracane w formacie JSON z wykorzystaniem obiektu DTO (`ReservationDto`), co oddziela model API od modelu bazy danych.

Endpoint API:
```
api/ReservationsApi
```
## 9. Architektura projektu
Projekt został wykonany zgodnie z wzorcem MVC (Model–View–Controller):
- Model – klasy encji, walidacja, relacje,
- View – widoki Razor (.cshtml),
- Controller – logika aplikacji, obsługa żądań.
Dodatkowo w projekcie zastosowane zostały:
- Entity Framework Core (Code First),
- ASP.NET Core Identity (autoryzacja i role),
- Bootstrap (interfejs użytkownika).

Projekt został podzielony na warstwy zgodnie z zasadami MVC, co ułatwia rozwój, testowanie oraz utrzymanie aplikacji.
---
Autor: Dominik Czechowicz s171932
