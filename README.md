Instrukcja uruchomienia
Postępuj zgodnie z poniższymi krokami, aby skonfigurować i uruchomić projekt ToDoListWPF.

Wymagania wstępne
Upewnij się, że masz zainstalowany Visual Studio na swoim komputerze.

Instalacja i konfiguracja
Sklonuj repozytorium za pomocą poniższego polecenia:

bash
Skopiuj kod
git clone https://github.com/oookiooo/ToDoListWPF.git
Po sklonowaniu repozytorium otwórz projekt w Visual Studio.

W Visual Studio kliknij prawym przyciskiem myszy na rozwiązanie "ToDoListWPF" i wybierz Właściwości.\

![image](https://github.com/user-attachments/assets/b298270d-58f8-4e6b-a575-07eeee5ae176)\


Skonfiguruj projekt tak, aby uruchamiał dwa projekty jednocześnie:
W oknie Właściwości wybierz opcję uruchamiania wielu projektów.
Skonfiguruj to zgodnie z przykładem na zrzucie ekranu.\

![image](https://github.com/user-attachments/assets/10f07f79-4e2f-4386-a62c-d9996a4f3b2c)\

Kliknij Zastosuj.
Opis działania programu\

![image](https://github.com/user-attachments/assets/38701d38-34a1-4e6d-82d1-f4f069e4ac41)\

![image](https://github.com/user-attachments/assets/7f3d2b58-d114-4e8a-bade-44a494ebaddd)\

Gdy zadanie kończy się za 30 minut (np. gdy aktualna godzina to 20:00, a zadanie ustawione jest na 20:20), w prawym dolnym rogu ekranu miga ikonka, informując o nadchodzącym zadaniu.
Miganie powiadomienia ustaje na minutę przed końcem zadania.
Jeśli status zadania zostanie ustawiony na "ukończone", miganie również przestaje.
