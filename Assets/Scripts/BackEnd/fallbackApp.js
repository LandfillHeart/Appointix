const fallbackApp = {
  dottori: [
    { id: 1, nome: "Luca", cognome: "Verdi", specializzazione: "Cardiologo" },
    { id: 2, nome: "Chiara", cognome: "Rossi", specializzazione: "Dermatologa" },
    { id: 3, nome: "Andrea", cognome: "Bianchi", specializzazione: "Neurologo" }
  ],
  pazienti: [
    { id: 1, nome: "Fabio", cognome: "Di Marco", email: "fabio@test.com" },
    { id: 2, nome: "Sara", cognome: "Conti", email: "sara@test.com" }
  ],
  prenotazioni: [
    { id: 1, idPaziente: 1, idDottore: 2, inizioApp: "2025-11-05 10:00:00", fineApp: "2025-11-05 10:20:00" }
  ]
};


module.exports = fallbackApp;