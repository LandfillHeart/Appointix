# Appointix

![Node.js](https://img.shields.io/badge/Node.js-v22.20-green?logo=node.js)
![MySQL](https://img.shields.io/badge/MySQL-v8.0.36-blue?logo=mysql)
![Unity](https://img.shields.io/badge/Unity-v6.2.9-black?logo=unity)
![License](https://img.shields.io/badge/License-MIT-green)

Applicazione sviluppata da :
- **Ray Kimbler**
- **Fabio D’Alessandro**
- **Michele Gabriele Matera** 
come **progetto finale del Academy *C# – FormaTemp***.

---

## Descrizione

**Appointix** è un sistema per la gestione di **appuntamenti medici** tra pazienti e dottori.  
L’app offre due interfacce principali:

- **Dottore:** visualizzazione e gestione degli appuntamenti, impostazione disponibilità, annullamento prenotazioni.
- **Paziente:** ricerca dottori per specializzazione/città, visualizzazione disponibilità, prenotazione appuntamenti.

> **Tip:** Una **REST API** (REpresentational State Transfer) è un’interfaccia che permette a client e server di comunicare tramite HTTP con operazioni standard (`GET`, `POST`, `PUT`, `DELETE`) usando dati in **JSON**.

---

## Architettura

| Componente | Stack / Ruolo |
|------------|---------------|
| **Frontend** | Unity (C#), UI interattiva, gestione login, registrazione e prenotazioni |
| **Backend** | Node.js + Express, esposizione API REST, logica business e trigger DB |
| **Database** | MySQL 8.0, gestione tabelle relazionate, trigger per automazione |

> **Tip:** I **trigger MySQL** sono script automatici che eseguono azioni quando un evento (es. `INSERT`) si verifica nel database.

---

## Ruoli e funzionalità

### Dottore
- Visualizza tutti gli appuntamenti prenotati dai pazienti.
- Imposta **giorni e orari disponibili**.
- Definisce la **durata media degli appuntamenti**.
- Può annullare prenotazioni future.

### Paziente
- Ricerca dottori per **città**, **specializzazione** o **disponibilità**.
- Visualizza orari liberi.
- Prenota o annulla appuntamenti futuri.

---

## Database (AppOintIX)

Tabelle principali:

| Tabella | Descrizione |
|---------|-------------|
| `dottore` | Anagrafica e disponibilità dei medici |
| `paziente` | Dati personali dei pazienti |
| `prenotazione` | Registro appuntamenti con orario inizio/fine |
| `login` | Gestione credenziali (hashate) e ruoli |

Trigger principali:

- `trg_prenotazione_fineApp` → calcola automaticamente la **fine appuntamento**.
- `trg_login_ruolo_check` → verifica coerenza **ruolo → foreign key**.

> **Tip:** **bcrypt** è una libreria per cifrare password in modo sicuro (hash + salt).

---

## Tecnologie

- **Frontend:** Unity (C#)
- **Backend:** Node.js, Express, bcrypt, mysql2, cors, body-parser
- **Database:** MySQL 8.0

---

## Setup rapido

1. **Clona il repo**
```bash
git clone https://github.com/LandfillHeart/Appointix.git
cd appointix
```

2. **Installa dipendenze Node.js**
```bash
npm install
```

3. **Configura il database**
- Avvia MySQL
- Esegui lo script db.sql per creare tabelle e trigger.

4. **Avvia il server**
- Entra nella cartella backend e lancia, dopo aver cambiato le credenziali in  **db.js**:
```bash
npm start
```

5. **Servizio REST**
- Avvia PostMan
- Copia gli esempi Json dal file: [BackEnd/esempi.txt](BackEnd/esempi.txt)

6. **TIPS**
- REST API: Interfaccia per comunicare tra client e server tramite HTTP standard.
- Trigger MySQL: Automatizzano operazioni complesse lato database (es. calcolo fine appuntamento).
- bcrypt: Protegge password con hash + salt, impossibile da decifrare facilmente.

7. **Licenza**
- [LICENSE](LICENSE)