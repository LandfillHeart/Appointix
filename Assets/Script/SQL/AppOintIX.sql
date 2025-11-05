-- === CREAZIONE DEL DATABASE PRINCIPALE ===
-- Crea il database solo se non esiste già, con codifica UTF-8 completa (utf8mb4)
-- e collation general-case-insensitive (supporta accenti e caratteri speciali)
CREATE DATABASE IF NOT EXISTS AppOintIX
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;

-- Seleziona il database su cui lavorare
USE AppOintIX;


-- === CREAZIONE TABELLA DOTTORE ===
-- Contiene le informazioni anagrafiche e di disponibilità di ogni dottore
CREATE TABLE IF NOT EXISTS dottore (
    id INT AUTO_INCREMENT PRIMARY KEY,           -- Identificativo univoco del dottore
    nome VARCHAR(50) NOT NULL,                   -- Nome del dottore
    cognome VARCHAR(50) NOT NULL,                -- Cognome del dottore
    specializzazione VARCHAR(100) NOT NULL,      -- Specializzazione medica (es. Cardiologo)
    email VARCHAR(50) DEFAULT "replay@appointix.com",  -- Email predefinita se non specificata
    telefono VARCHAR(20) DEFAULT "081 5434 60",        -- Numero di telefono standard
    citta VARCHAR(50),                           -- Città di lavoro o sede
    durata INT DEFAULT 30,                       -- Durata media dell'appuntamento (in minuti)
    giorniDisponibili VARCHAR(255) DEFAULT '',   -- Giorni in cui il dottore è disponibile (es. 'Lun,Mar,Mer')
    orarioInizio TIME DEFAULT '09:00:00',        -- Orario d'inizio disponibilità giornaliera
    orarioFine TIME DEFAULT '18:00:00'           -- Orario di fine disponibilità giornaliera
) ENGINE=InnoDB;                                 -- Usa InnoDB per supportare vincoli, transazioni e relazioni


-- === CREAZIONE TABELLA PAZIENTE ===
-- Contiene i dati anagrafici dei pazienti
CREATE TABLE IF NOT EXISTS paziente (
    id INT AUTO_INCREMENT PRIMARY KEY,           -- Identificativo univoco del paziente
    nome VARCHAR(50) NOT NULL,                   -- Nome del paziente
    cognome VARCHAR(50) NOT NULL,                -- Cognome del paziente
    email VARCHAR(50) UNIQUE,                    -- Email univoca (evita duplicati)
    telefono VARCHAR(20)                         -- Numero di telefono
) ENGINE=InnoDB;                                 -- Anche qui InnoDB per integrità referenziale


-- === CREAZIONE TABELLA PRENOTAZIONE ===
-- Registra ogni appuntamento tra paziente e dottore
CREATE TABLE IF NOT EXISTS prenotazione (
    id INT AUTO_INCREMENT PRIMARY KEY,           -- Identificativo univoco della prenotazione
    idPaziente INT NOT NULL,                     -- Chiave esterna verso la tabella paziente
    idDottore INT NOT NULL,                      -- Chiave esterna verso la tabella dottore
    inizioApp TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Orario di inizio dell'appuntamento
    fineApp TIMESTAMP DEFAULT NULL,              -- Orario di fine (calcolato dal trigger)
    
    -- Definizione dei vincoli di integrità referenziale
    CONSTRAINT fk_paziente FOREIGN KEY (idPaziente) REFERENCES paziente(id),
    CONSTRAINT fk_dottore FOREIGN KEY (idDottore) REFERENCES dottore(id)
) ENGINE=InnoDB;                                 -- Necessario per supportare foreign key e trigger


-- === TRIGGER AUTOMATICO: CALCOLO FINE APPUNTAMENTO ===
-- Scatta prima dell'inserimento di una nuova prenotazione
-- Calcola automaticamente il campo 'fineApp' in base alla durata del dottore
-- Se l'ID del dottore non è valido, blocca l'inserimento con un errore
DELIMITER //
CREATE TRIGGER trg_prenotazione_fineApp
BEFORE INSERT ON prenotazione
FOR EACH ROW
BEGIN
    DECLARE durataMin INT;  -- Variabile temporanea per salvare la durata del dottore

    -- Recupera la durata (in minuti) dalla tabella dottore in base all'ID
    SELECT durata INTO durataMin
    FROM dottore
    WHERE id = NEW.idDottore;

    -- Se non trova la durata (o il dottore non esiste), interrompe l'operazione
    IF durataMin IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Dottore non trovato o durata non definita';
    END IF;

    -- Calcola l'orario di fine appuntamento aggiungendo la durata al timestamp iniziale
    SET NEW.fineApp = NEW.inizioApp + INTERVAL durataMin MINUTE;
END//
DELIMITER ;