using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaGame
{
    class PizzaGame
    {
        public static readonly int MinPizze = 11;
        public static readonly int MaxPizzeGiocata = 3;
        
        private int _pizzeIniziali;
        private Queue<Pizza> _pizze;    // non proprio necessaria una coda di oggetti, ma giusto per abbellire
        
        private List<string> _players = new(); // già predisposto per gestire n giocatori
        private int? _currentPlayer;    // giocatore (0-based index) della giocata in corso
        private int? _prevPlayer;       // giocatore (0-based index) della giocata precedente
        private int _prevPizzeMangiate; // numero di pizze mangiate nella giocata precedente

        /// <summary>
        /// Costruttore che accetta un min/max pizze e l'elenco dei nomi dei giocatori
        /// </summary>
        /// <param name="minPizze">Numero minimo di pizze</param>
        /// <param name="maxPizze">Numero massimo di pizze</param>
        /// <param name="players">Nomi dei giocatori (almeno 2)</param>
        public PizzaGame(int minPizze, int maxPizze, params string[] players)
        {
            if (minPizze < MinPizze || maxPizze < MinPizze)
                throw new ArgumentException($"Il numero di pizze deve essere almeno {MinPizze}");
            if (minPizze > maxPizze)
                throw new ArgumentException("Il numero di pizze massimo non può essere minore di quello minimo");
            if (players.Length < 2)
                throw new ArgumentException("Ci devono essere almeno 2 giocatori");

            Random rand = new();
            // determina il numero di pizze e le aggiunge alla coda
            _pizzeIniziali = rand.Next(minPizze, maxPizze + 1);            
            _pizze = new Queue<Pizza>();
            for (int i = 0; i < _pizzeIniziali; i++)
            {
                _pizze.Enqueue(new Pizza());
            }
            // aggiunge i giocatori
            _players.AddRange(players);
        }

        public int PizzeIniziali => _pizzeIniziali;
        public int PizzeRimaste => _pizze.Count;

        /// <summary>
        /// Torna il nome del prossimo giocatore che deve fare la giocata
        /// </summary>
        public string GetPlayer()
        {
            if (IsFinito)
                throw new Exception("Il gioco è finito!");

            if (_prevPlayer == null)
            {
                // siamo all'inizio: il primo giocatore della lista
                _currentPlayer = 0;
            }
            else
            {
                // giocatore successivo
                _currentPlayer = _prevPlayer.Value + 1;
                if (_currentPlayer == _players.Count)
                    _currentPlayer = 0;

                if (_prevPizzeMangiate == 1 && PizzeRimaste == 1)
                {
                    // salto turno
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count)
                        _currentPlayer = 0;
                }                
            }

            return _players[_currentPlayer.Value];
        }        

        /// <summary>
        /// Esegue la giocata
        /// </summary>
        /// <param name="numPizze">Numero di pizze mangiate</param>
        /// <returns>Se la giocata è riuscita o meno, con l'eventuale messaggio di errore</returns>
        public (bool Played, string Message) SetPlayed(int numPizze)
        {
            if (IsFinito)
                throw new Exception("Il gioco è finito!");

            if (numPizze == 0)
                return (false, "Inserisci un numero di pizze da mangiare");
            if (numPizze > PizzeRimaste)
                return (false, "Non puoi mangiare più pizze di quelle rimanenti");
            if (numPizze > MaxPizzeGiocata)
                return (false, $"Non si possono mangiare più di {MaxPizzeGiocata} pizze alla volta");
            if (numPizze == _prevPizzeMangiate && _currentPlayer != _prevPlayer)
                return (false, $"Non puoi mangiare lo stesso numero di pizze mangiate prima dal tuo avversario");

            for (var i = 0; i < numPizze; i++)
                _pizze.Dequeue();

            // memorizza la giocata
            _prevPizzeMangiate = numPizze;
            _prevPlayer = _currentPlayer;

            return (true, null);
        }
        
        public bool IsFinito
        {
            get
            {
                return PizzeRimaste == 0;
            }
        }

        public string Esito
        {
            get
            {
                if (PizzeRimaste > 0)
                    return "Il gioco è ancora in corso!";
                else
                    return $"{_players[_currentPlayer.Value]} hai perso!";
            }
        }
    }

    class Pizza
    {
    }
}
