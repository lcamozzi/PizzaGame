using System;

namespace PizzaGame
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // chiede i nomi dei giocatori
                string player1 = null, player2 = null;
                while (string.IsNullOrEmpty(player1?.Trim()))
                {
                    Console.Write("Nome del primo giocatore: ");
                    player1 = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(player2?.Trim()) || player2 == player1)
                {
                    Console.Write("Nome del secondo giocatore: ");
                    player2 = Console.ReadLine();
                }
                // chiede il numero max di pizze possibili
                // (eventualmente si potrebbe anche chiedere il numero minimo, comunque > PizzaGame.MinPizze)
                int maxPizze = 0;
                while (maxPizze < PizzaGame.MinPizze)
                {
                    Console.Write($"Numero max di pizze possibili (almeno {PizzaGame.MinPizze}): ");
                    var num = Console.ReadLine();
                    Int32.TryParse(num, out maxPizze);
                }
                                
                // istanzia il gioco
                var game = new PizzaGame(PizzaGame.MinPizze, maxPizze, player1, player2);
                Console.WriteLine($"\nIniziamo, buon divertimento!");
                Console.WriteLine($"Ci sono {game.PizzeIniziali} pizze");

                while (!game.IsFinito)
                {
                    // prossimo giocatore
                    var player = game.GetPlayer();
                    Console.WriteLine($"\n{player}, tocca a te!");
                    bool played = false;
                    do
                    {
                        // chiede le pizze che vuole mangiare
                        Console.Write($"  Quante pizze vuoi mangiare? ");
                        var k = Console.ReadKey();
                        byte.TryParse(k.KeyChar.ToString(), out byte p);
                        if (p == 0)
                        {
                            Console.WriteLine("\n  Inserisci un numero!");
                        }
                        else
                        {
                            // esegue la giocata
                            var result = game.SetPlayed(p);
                            if (result.Played)
                            {
                                played = true;
                                if (game.PizzeRimaste == 1)
                                    Console.Write($"\n\nRimane 1 pizza");
                                else if (game.PizzeRimaste > 1)
                                    Console.Write($"\n\nRimangono {game.PizzeRimaste} pizze");
                            }
                            else
                            {
                                Console.WriteLine($"\n  {result.Message}");
                            }
                        }
                    }
                    while (!played); // finchè il giocatore non ha giocato
                }

                // gioco finito
                Console.WriteLine($"\n\n{game.Esito}");                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErrore: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
