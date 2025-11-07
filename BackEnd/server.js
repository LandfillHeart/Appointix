// ============================
// IMPORT DEI MODULI
// ============================

// Express: framework web per gestire le rotte e le richieste HTTP
const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
// Connessione al DB (definita in db.js)
const db = require('./db');
// mok per fallback
const fb = require('./fallbackApp');
// Bcrypt: libreria per cifrare e verificare le password
const bcrypt = require('bcrypt');


// ============================
// INIZIALIZZAZIONE APP
// ============================

// Crea un'app Express
const app = express();
const port = 3000;

app.use(cors({
  origin: '*', // Allow all origins for Unity
}));
// Middleware per interpretare il corpo delle richieste in formato JSON
// Es: body { "username": "Marco" }
app.use(express.json());


// Test endpoint for connection
app.get('/api/test', (req, res) => {
  res.json({ 
    message: 
      'Server is running!', 
    timestamp: 
      new Date().toISOString() });
});

// API endpoints for dottore
// GET all dottori
app.get('/api/dottori', async (req, res) => {
  try 
  {
    const [tasks] = await db.execute('SELECT * FROM dottore ORDER BY id DESC');
    res.json(tasks);
  } 
  catch (error) 
  {
    console.log('⚠️ MySQL not available, using fallback (dottori) :', error.message);
    // Fallback: restituisci task in memoria
    res.json(fb.dottori);
  }
});

// GET id dottori
app.get('/api/dottori/:id', async (req, res) => {
  try {
    const [tasks] = await db.execute('SELECT * FROM dottore WHERE id = ?', [req.params.id]);
    if (tasks.length === 0) {
      return res.status(404).json({ error: 'Dottore non trovato' });
    }
    res.json(tasks[0]);
  } catch (error) {
    res.status(500).json({ error: 'Errore nel recupero del dottore', details: error.message });
  }
});

// API endpoints for pazienti
// GET all pazienti
app.get('/api/pazienti', async (req, res) => {
  try 
  {
    const [tasks] = await db.execute('SELECT * FROM paziente ORDER BY id DESC');
    res.json(tasks);
  } 
  catch (error) 
  {
    console.log('⚠️ MySQL not available, using fallback [pazienti]:', error.message);
    // Fallback: restituisci task in memoria
    res.json(fb.pazienti);
  }
});

// GET id pazienti
app.get('/api/pazienti/:id', async (req, res) => {
  try {
    const [tasks] = await db.execute('SELECT * FROM paziente WHERE id = ?', [req.params.id]);
    if (tasks.length === 0) {
      return res.status(404).json({ error: 'Paziente non trovato' });
    }
    res.json(tasks[0]);
  } catch (error) {
    res.status(500).json({ error: 'Errore nel recupero del paziente', details: error.message });
  }
});

// API endpoints for prenotazioni
// GET all prenotazioni
app.get('/api/prenotazioni', async (req, res) => {
  try 
  {
    const [tasks] = await db.execute('SELECT * FROM prenotazione ORDER BY id DESC');
    res.json(tasks);
  } 
  catch (error) 
  {
    console.log('⚠️ MySQL not available, using fallback (prenotazioni):', error.message);
    // Fallback: restituisci task in memoria
    res.json(fb.prenotazioni);
  }
});

// GET id prenotazioni
app.get('/api/prenotazioni/:id', async (req, res) => {
  try {
    const [tasks] = await db.execute('SELECT * FROM prenotazione WHERE id = ?', [req.params.id]);
    if (tasks.length === 0) {
      return res.status(404).json({ error: 'Prenotazione non trovata' });
    }
    res.json(tasks[0]);
  } catch (error) {
    res.status(500).json({ error: 'Errore nel recupero della prenotazione', details: error.message });
  }
});

// GET id dottore prenotazioni
app.get('/api/prenotazioni/dottore/:id', async (req, res) => {
  try {
    const [tasks] = await db.execute('SELECT * FROM prenotazione WHERE idDottore = ?', [req.params.id]);
    if (tasks.length === 0) {
      return res.status(404).json({ error: 'Prenotazione non trovata' });
    }
    res.json(tasks[0]);
  } catch (error) {
    res.status(500).json({ error: 'Errore nel recupero della prenotazione', details: error.message });
  }
});

// GET id paziente prenotazioni
app.get('/api/prenotazioni/paziente/:id', async (req, res) => {
  try {
    const [tasks] = await db.execute('SELECT * FROM prenotazione WHERE idPaziente = ?', [req.params.id]);
    if (tasks.length === 0) {
      return res.status(404).json({ error: 'Prenotazione non trovata' });
    }
    res.json(tasks[0]);
  } catch (error) {
    res.status(500).json({ error: 'Errore nel recupero della prenotazione', details: error.message });
  }
});


app.post('/api/creaprenotazione', async (req, res) => {
  try {
    // ================================
    // 1️⃣ Estrazione dati dal body
    // ================================
    const { idPaziente, idDottore, inizioAppuntamento } = req.body;

    // Validazione minima
    if (!idPaziente || !idDottore) {
      return res.status(400).json({ message: 'idPaziente e idDottore sono obbligatori' });
    }

    // ================================
    // 2️⃣ Verifica esistenza paziente
    // ================================
    const [pazienteRows] = await db.query(
      'SELECT id FROM paziente WHERE id = ?',
      [idPaziente]
    );
    if (pazienteRows.length === 0) {
      return res.status(404).json({ message: '❌ Paziente non trovato' });
    }

    // ================================
    // 3️⃣ Verifica esistenza dottore
    // ================================
    const [dottoreRows] = await db.query(
      'SELECT id, durata FROM dottore WHERE id = ?',
      [idDottore]
    );
    if (dottoreRows.length === 0) {
      return res.status(404).json({ message: '❌ Dottore non trovato' });
    }

    // ================================
    // 4️⃣ Inserimento prenotazione
    // ================================
    // Se inizioAppuntamento non è fornito, usa il default CURRENT_TIMESTAMP del DB
    let query, params;
    if (inizioAppuntamento) {
      query = `
        INSERT INTO prenotazione (idPaziente, idDottore, inizioApp)
        VALUES (?, ?, ?)
      `;
      params = [idPaziente, idDottore, inizioAppuntamento];
    } else {
      query = `
        INSERT INTO prenotazione (idPaziente, idDottore)
        VALUES (?, ?)
      `;
      params = [idPaziente, idDottore];
    }

    const [result] = await db.query(query, params);

    // ================================
    // 5️⃣ Recupero prenotazione creata
    // ================================
    const [newApp] = await db.query(
      'SELECT * FROM prenotazione WHERE id = ?',
      [result.insertId]
    );

    // ================================
    // 6️⃣ Risposta OK
    // ================================
    res.status(201).json({
      message: '✅ Prenotazione creata con successo',
      prenotazione: newApp[0]
    });

  } catch (err) {
    console.error('❌ Errore nella creazione prenotazione:', err);
    res.status(500).json({
      message: 'Errore nel server durante la creazione della prenotazione',
      error: err.message
    });
  }
});

// ============================
// DELETE PRENOTAZIONE
// ============================
//
// Rimuove una prenotazione dal DB tramite ID
// Verifica prima che la prenotazione esista, altrimenti restituisce errore
// ============================
app.delete('/api/prenotazioni/:id', async (req, res) => {
  try {
    const prenotazioneId = req.params.id;

    // 1️⃣ Controllo esistenza
    const [check] = await db.query('SELECT * FROM prenotazione WHERE id = ?', [prenotazioneId]);
    if (check.length === 0) {
      return res.status(404).json({ message: '❌ Prenotazione non trovata' });
    }

    // 2️⃣ Eliminazione
    await db.query('DELETE FROM prenotazione WHERE id = ?', [prenotazioneId]);
    console.log(`🗑️ Prenotazione ID ${prenotazioneId} eliminata con successo.`);

    // 3️⃣ Risposta positiva
    res.json({ message: '✅ Prenotazione eliminata con successo', id: prenotazioneId });
  } catch (err) {
    console.error('❌ Errore durante l’eliminazione della prenotazione:', err);
    res.status(500).json({ message: 'Errore del server durante la cancellazione', error: err.message });
  }
});


// ============================
// ENDPOINT: REGISTRAZIONE
// ============================
//
// Inserisce un nuovo utente nel DB con password hashata.
// Accetta:
// - username
// - password (in chiaro, poi cifrata qui)
// - ruolo ("P" = Paziente, "D" = Dottore)
// - idPaziente o idDottore, a seconda del ruolo
// ============================
app.post('/api/register', async (req, res) => {
  try {
    // ===============================
    // 1️⃣ Estrazione dati dal body
    // ===============================
    const { username, password, ruolo, nome, cognome, email, telefono, specializzazione, citta } = req.body;

    // Validazione minima
    if (!username || !password || !ruolo || !nome || !cognome) {
      return res.status(400).json({ message: 'Campi obbligatori mancanti' });
    }

    // ===============================
    // 2️⃣ Cifratura sicura della password
    // ===============================
    const saltRounds = 12;
    const hashedPassword = await bcrypt.hash(password, saltRounds);

    // ===============================
    // 3️⃣ Creazione dinamica in base al ruolo
    // ===============================
    let idPaziente = null;
    let idDottore = null;

    if (ruolo === 'P') {
      // --- Inserisci nuovo paziente ---
      const [result] = await db.query(
        `INSERT INTO paziente (nome, cognome, email, telefono) VALUES (?, ?, ?, ?)`,
        [nome, cognome, email || null, telefono || null]
      );
      idPaziente = result.insertId;
      console.log(`🧍‍♂️ Paziente creato con ID: ${idPaziente}`);
    } 
    else if (ruolo === 'D') {
      // --- Inserisci nuovo dottore ---
      const [result] = await db.query(
        `INSERT INTO dottore (nome, cognome, specializzazione, email, telefono, citta)
         VALUES (?, ?, ?, ?, ?, ?)`,
        [nome, cognome, specializzazione || 'Generico', email || null, telefono || null, citta || null]
      );
      idDottore = result.insertId;
      console.log(`👨‍⚕️ Dottore creato con ID: ${idDottore}`);
    } 
    else {
      return res.status(400).json({ message: "Ruolo non valido. Usa 'P' o 'D'." });
    }

    // ===============================
    // 4️⃣ Inserimento utente nella tabella LOGIN
    // ===============================
    await db.query(
      `INSERT INTO login (username, password, ruolo, idPaziente, idDottore)
       VALUES (?, ?, ?, ?, ?)`,
      [username, hashedPassword, ruolo, idPaziente, idDottore]
    );

    console.log(`✅ Utente '${username}' registrato come ${ruolo === 'P' ? 'paziente' : 'dottore'}`);

    // ===============================
    // 5️⃣ Risposta positiva
    // ===============================
    res.status(201).json({ 
      message: '✅ Registrazione completata con successo', 
      idPaziente, 
      idDottore 
    });

  } catch (err) {
    console.error('❌ Errore nel register:', err);
    res.status(500).json({ message: 'Errore nel server', error: err.message });
  }
});



// ============================
// ENDPOINT: LOGIN
// ============================
//
// Verifica le credenziali di un utente.
// Confronta la password inviata con l’hash salvato nel DB.
// Restituisce info di base se il login ha successo.
// ============================

app.post('/api/login', async (req, res) => {
  try {
    // Estraggo username e password dal body
    const { username, password } = req.body;

    // Cerco l'utente nel DB tramite username
    const [rows] = await db.query(
      `SELECT * FROM login WHERE username = ?`,
      [username]
    );

    // Se non esiste → errore
    if (rows.length === 0) {
      return res.status(401).json({ message: 'Utente non trovato' });
    }

    const user = rows[0];

    // Confronto tra password inviata e hash salvato
    const match = await bcrypt.compare(password, user.password);

    // Se non coincidono → errore di autenticazione
    if (!match) {
      return res.status(401).json({ message: 'Password errata' });
    }

    // Login riuscito → invio dati minimi (ruolo e id collegato)
    res.json({
      message: '✅ Login riuscito',
      ruolo: user.ruolo,
      id: user.ruolo === 'D' ? user.idDottore : user.idPaziente
    });

  } catch (err) {
    console.error('❌ Errore nel login:', err);
    res.status(500).json({ message: 'Errore nel server', error: err.message });
  }
});


// ============================
// AVVIO DEL SERVER
// ============================
//
// Attiva il server Express in ascolto sulla porta 3000
// Accessibile su http://localhost:3000
// ============================

app.listen(port, () => {
  console.log('=================================================');
  console.log(`🚀 Server attivo su: http://localhost:${port}`);
  console.log('=================================================');
  console.log('📡 ENDPOINT DISPONIBILI:');
  
  console.log('--- DOTTORE ---');
  console.log('GET    /api/dottori               → Tutti i dottori');
  console.log('GET    /api/dottori/:id           → Dottore per ID');
  console.log('--- PAZIENTE ---');
  console.log('GET    /api/pazienti              → Tutti i pazienti');
  console.log('GET    /api/pazienti/:id          → Paziente per ID');

  console.log('--- PRENOTAZIONI ---');
  console.log('GET    /api/prenotazioni          → Tutte le prenotazioni');
  console.log('GET    /api/prenotazioni/:id      → Prenotazione per ID');
  console.log('GET    /api/prenotazioni/dottore/:id  → Prenotazioni per dottore');
  console.log('GET    /api/prenotazioni/paziente/:id → Prenotazioni per paziente');
  console.log('POST   /api/creaprenotazione      → Crea una nuova prenotazione');
  console.log('DELETE /api/prenotazioni/:id      → Elimina prenotazione per ID');

  console.log('--- AUTENTICAZIONE ---');
  console.log('POST   /api/register              → Crea utenza (P o D)');
  console.log('POST   /api/login                 → Esegui login utente');
  console.log('=================================================');
});
