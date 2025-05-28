# MediCode - uproszczony system kliniki lekarskiej 

## Autor

Natalia Dybczak

## Kontakt

ndybczak@student.agh.edu.pl

## Opis projektu

MediCode to aplikacja internetowa stworzona w technologii **ASP.NET Core MVC** z dopełniającym **REST API**, która umożliwia zarządzanie lekarzami, pacjentami oraz wizytami w klinice lekarskiej. 

## Cel aplikacji

* Umożliwienie administratorowi zarządzanie kontami lekarzy
* Zapewnienie lekarzom możliwości zarządzania kontami oraz danymi zdrowotnymi pacjentów
* Umożliwienie lekarzom dodawania oraz odwoływania terminów wizyt, a także zapisywania pacjentów na wizyty
* Wyświetlanie statystyk dotyczących liczby wizyt oraz pacjentów dla każdego lekarza 

## Technologie

* ASP.NET Core MVC
* REST API (kontrolery API)
* Entity Framework Core (SQLite)
* Sesje użytkowników
* Hashowanie haseł (SHA256)
* Program konsolowy demonstracyjny (C#)

## Funkcjonalności

### Aplikacja webowa (MVC)

* Logowanie lekarzy (z autoryzacją po roli admina)
* Zarządzanie lekarzami (tylko administrator)
* Przegląd i edycja danych pacjentów
* Przegląd i edycja wizyt (widoczne tylko wizyty danego lekarza)
* Formularze dodawania chorób pacjentów
* Statystyki lekarza:
  * Łączna liczba wizyt
  * Liczba zrealizowanych wizyt
  * Liczba unikalnych pacjentów zrealizowanych wizyt

### REST API

* Endpoints do obsługi:

  * Pacjentów
  * Chorób pacjentów
  * Wizyt
* Uwierzytelnianie przez login + token (przesyłane w nagłówkach)
* Operacje GET / POST / PUT / DELETE dla powyższych encji

### Program konsolowy (REST client)

* Demonstracja REST API:

  * logowanie przez token
  * przegląd pacjentów i wizyt
  * dodawanie / edycja / usuwanie pacjentów, wizyt i chorób

## Tabele bazy danych

* **Lekarze** 
* **Pacjenci** 
* **Choroby** 
* **Wizyty** 

## Szczegóły techniczne

* Hasła są przechowywane jako SHA256
* Administrator tworzony jest przy pierwszym uruchomieniu aplikacji 
* Każdy lekarz posiada wygenerowany token
* W widokach ukryto możliwości edycji/dostępu dla nieautoryzowanych użytkowników

## Użytkowanie

* Po wejściu na stronę: możliwość logowania
* Po zalogowaniu jako **admin**:

  * Możliwość dodawania i usuwania lekarzy, a także przeglądania i edycji ich danych
  * Możliwość korzystania z funkcjonalności dostępnych dla każego lekarza 
* Po zalogowaniu jako **lekarz**:

  * Dostęp do pacjentów, ich chorób oraz wizyt
  * Możliwość dodawania i usuwania pacjentów, edycji ich danych oraz chorób 
  * Możliwość przeglądania wizyt zalogowanego lekarza, dodania nowej wizyty lub jej edycji
  * Statystyki dotyczące zrealizowanych wizyt

## Uwagi

* Tokeny są dostępne tylko dla administratora w zakładce Szczegóły dla każdego lekarza 

---

