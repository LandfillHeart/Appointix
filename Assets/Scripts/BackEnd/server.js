// ============================
// IMPORT DEI MODULI
// ============================

// Express: framework web per gestire le rotte e le richieste HTTP
import express from 'express';

// Connessione al DB (definita in db.js)
import db from './db.js';

// Bcrypt: libreria per cifrare e verificare le password
import bcrypt from 'bcrypt';


// ============================
// INIZIALIZZAZIONE APP
// ============================

// Crea un'app Express
const app = express();

// Middleware per interpretare il corpo delle richieste in formato JSON
// Es: body { "username": "Marco" }
app.use(express.json());


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

app.post('/register', async (req, res) => {
  try {
    // Estraggo i dati inviati nel body della richiesta
    const { username, password, ruolo, idPaziente, idDottore } = req.body;

    // Controllo di validazione minima
    if (!username || !password || !ruolo) {
      return res.status(400).json({ message: 'Campi obbligatori mancanti' });
    }

    // Genera un hash sicuro della password (12 = livello di complessità)
    const saltRounds = 12;
    const hashedPassword = await bcrypt.hash(password, saltRounds);

    // Inserisce il nuovo utente nella tabella login
    // idPaziente e idDottore possono essere NULL (a seconda del ruolo)
    await db.query(
      `INSERT INTO login (username, password, ruolo, idPaziente, idDottore)
       VALUES (?, ?, ?, ?, ?)`,
      [username, hashedPassword, ruolo, idPaziente, idDottore]
    );

    // Risposta positiva
    res.status(201).json({ message: '✅ Registrazione completata con successo' });
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

app.post('/login', async (req, res) => {
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

app.listen(3000, () => {
  console.log('🚀 Server attivo su http://localhost:3000');
});
