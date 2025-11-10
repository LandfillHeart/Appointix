// db.js
// Importiamo la versione "promise-based" di mysql2.
// Questo modulo espone API che già ritornano Promise, così possiamo usare async/await
const mysql = require('mysql2/promise');

/*
  Creiamo un connection pool. Il pool mantiene un insieme di connessioni riusabili
  verso MySQL, evitando l'overhead di aprire/chiudere una connessione per ogni query.

  Opzioni utili (alcune impostate qui, altre potresti voler aggiungere):
    - host: indirizzo del DB (localhost o IP)
    - user: utente MySQL
    - password: password dell'utente (se è vuota, metti '' e NON null)
    - database: nome esatto del database (case-sensitive su Linux/macOS)
    - waitForConnections: se true il pool attende quando non ci sono connessioni libere
    - connectionLimit: numero massimo di connessioni nel pool
    - queueLimit: massimo query in coda (0 = unlimited)
    - namedPlaceholders: true permette query con :name (opzionale)
*/
const db = mysql.createPool({
  host: 'localhost',
  user: 'root',
  password: 'root',
  //password: 'fondutaLover13',   // SE la password è vuota, usa stringa vuota '' — non usare null
  database: 'AppOintIX',
  waitForConnections: true,
  connectionLimit: 10,   // valore ragionevole per dev; in produzione dimensionalo
  queueLimit: 0
});

// Esportiamo l'oggetto pool (db) in modo che gli altri file possano importarlo con require('./db')
module.exports = db;