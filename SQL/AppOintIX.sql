-- ============================================================
-- === CREAZIONE DEL DATABASE PRINCIPALE ======================
-- ============================================================

-- Crea il database solo se non esiste già, con codifica UTF-8 completa (utf8mb4)
-- e collation case-insensitive, per supportare accenti, emoji e caratteri speciali.
CREATE DATABASE IF NOT EXISTS AppOintIX
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;

-- Seleziona il database su cui lavorare
USE AppOintIX;


-- ============================================================
-- === CREAZIONE TABELLA DOTTORE ==============================
-- ============================================================

-- Contiene le informazioni anagrafiche e di disponibilità di ogni dottore.
CREATE TABLE IF NOT EXISTS dottore (
    id INT AUTO_INCREMENT PRIMARY KEY,           -- Identificativo univoco del dottore
    nome VARCHAR(50) NOT NULL,                   -- Nome del dottore
    cognome VARCHAR(50) NOT NULL,                -- Cognome del dottore
    specializzazione VARCHAR(100) NOT NULL DEFAULT "Base",      -- Specializzazione medica (es. Cardiologo)
    email VARCHAR(50) UNIQUE NOT NULL,  -- Email OBBLIGATORIA
    telefono VARCHAR(20) DEFAULT "081 5434 60",        -- Numero di telefono di default
    citta VARCHAR(50),                           -- Città di lavoro o sede del medico
    durata INT DEFAULT 30,                       -- Durata media di un appuntamento (in minuti)
    giorniDisponibili VARCHAR(255) DEFAULT "Lun,Mar,Mer,Gio,Ven",   -- Giorni disponibili (es. 'Lun,Mar,Mer')
    orarioInizio TIME DEFAULT '09:00:00',        -- Inizio orario di lavoro
    orarioFine TIME DEFAULT '18:00:00'           -- Fine orario di lavoro
) ENGINE=InnoDB;                                 -- InnoDB: supporta transazioni e foreign key


-- ============================================================
-- === CREAZIONE TABELLA PAZIENTE =============================
-- ============================================================

-- Contiene i dati anagrafici di ogni paziente.
CREATE TABLE IF NOT EXISTS paziente (
    id INT AUTO_INCREMENT PRIMARY KEY,           -- Identificativo univoco del paziente
    nome VARCHAR(50) NOT NULL,                   -- Nome del paziente
    cognome VARCHAR(50) NOT NULL,                -- Cognome del paziente
    email VARCHAR(50) UNIQUE NOT NULL,                    -- Email univoca per login/contatti
    telefono VARCHAR(20)                         -- Numero di telefono del paziente
) ENGINE=InnoDB;                                 -- InnoDB: integrità referenziale garantita


-- ============================================================
-- === CREAZIONE TABELLA PRENOTAZIONE =========================
-- ============================================================

-- Registra ogni appuntamento tra paziente e dottore, con orario di inizio e fine.
CREATE TABLE IF NOT EXISTS prenotazione (
    id INT AUTO_INCREMENT PRIMARY KEY,              -- Identificativo univoco della prenotazione
    idPaziente INT NOT NULL,                        -- Riferimento al paziente
    idDottore INT NOT NULL,                         -- Riferimento al dottore
    inizioApp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- Orario di inizio dell’appuntamento
    fineApp TIMESTAMP DEFAULT NULL,                 -- Orario di fine (calcolato dal trigger)
    
    -- Vincoli di integrità per mantenere coerenza tra tabelle
    CONSTRAINT fk_paziente FOREIGN KEY (idPaziente) REFERENCES paziente(id),
    CONSTRAINT fk_dottore FOREIGN KEY (idDottore) REFERENCES dottore(id)
) ENGINE=InnoDB;                                    -- Supporto per trigger e relazioni


-- ============================================================
-- === CREAZIONE TABELLA LOGIN ================================
-- ============================================================

-- Gestisce l’autenticazione di pazienti e dottori.
-- Le password vengono salvate come hash (es. bcrypt o argon2id) generati dal backend.
CREATE TABLE IF NOT EXISTS login (
    id INT AUTO_INCREMENT PRIMARY KEY,              -- Identificativo univoco del record login
    username VARCHAR(50) UNIQUE NOT NULL,           -- Nome utente (univoco) EMAIL DEL PAZIENTE O DOTTORE
    password VARCHAR(255) NOT NULL,                 -- Hash della password (NON in chiaro)
    ruolo ENUM('P', 'D') NOT NULL,                  -- 'P' = Paziente, 'D' = Dottore
    idPaziente INT DEFAULT NULL,                    -- FK per paziente (se ruolo = 'P')
    idDottore INT DEFAULT NULL,                     -- FK per dottore (se ruolo = 'D')
    
    -- Vincoli referenziali per associare il login all’entità corretta
    FOREIGN KEY (idPaziente) REFERENCES paziente(id),
    FOREIGN KEY (idDottore) REFERENCES dottore(id)
) ENGINE=InnoDB;                                    -- Necessario per mantenere relazioni coerenti


-- ============================================================
-- === TRIGGER: CALCOLO AUTOMATICO FINE APPUNTAMENTO ==========
-- ============================================================

-- Questo trigger calcola automaticamente l’orario di fine appuntamento
-- in base alla durata definita per il dottore associato.
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

    -- Se non trova la durata (o il dottore non esiste), interrompe l’operazione
    IF durataMin IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Dottore non trovato o durata non definita';
    END IF;

    -- Calcola l’orario di fine appuntamento aggiungendo la durata al timestamp iniziale
    SET NEW.fineApp = NEW.inizioApp + INTERVAL durataMin MINUTE;
END//
DELIMITER ;


-- ============================================================
-- === TRIGGER: VALIDAZIONE RUOLO IN LOGIN ====================
-- ============================================================

-- Verifica che il ruolo corrisponda all’entità corretta:
-- se 'D' serve un idDottore, se 'P' serve un idPaziente.
DELIMITER //
CREATE TRIGGER trg_login_ruolo_check
BEFORE INSERT ON login
FOR EACH ROW
BEGIN
    -- Controllo coerenza tra ruolo e foreign key
    IF NEW.ruolo = 'D' AND NEW.idDottore IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Ruolo D richiede un idDottore valido';
    ELSEIF NEW.ruolo = 'P' AND NEW.idPaziente IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Ruolo P richiede un idPaziente valido';
    END IF;
END//
DELIMITER ;

-- === PROVA  DOTTORE ===
SELECT * FROM dottore ORDER BY id DESC;

-- === PROVA PAZIENTE ===
SELECT * FROM paziente ORDER BY id DESC;


-- === PROVA PRENOTAZIONE ===
SELECT * FROM prenotazione ORDER BY id DESC;

-- === PROVA LOGIN ===
SELECT * FROM login ORDER BY id DESC;
