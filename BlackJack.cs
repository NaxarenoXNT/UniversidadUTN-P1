

namespace MiProyecto.Ejercicios
{
    public class Ejercicio
    {
        public void TurnoJugador(string eleccion, int cantidadTotal, bool dealer)
        {
            Random CartaAleatoria= new Random();
            bool TurnoDealer=true;

            while(eleccion=="si")
            {
                int CartaRandom= CartaAleatoria.Next(1,11);

                Console.WriteLine("Quiere tomar una carta?: Si/No");
                eleccion=Console.ReadLine()??"no".ToLower();
                if(eleccion=="si")
                {
                    cantidadTotal+=CartaRandom;
                    Console.WriteLine(cantidadTotal);

                    if(cantidadTotal>21)
                    {
                        Console.WriteLine($"Lamentablemente usted perdio la suma de sus cartas es: {cantidadTotal}");
                    }
                    else if(cantidadTotal==21)
                    {
                        Console.WriteLine("Usted a ganado!!!!");
                    }
                    else if(eleccion=="no" && !dealer)
                    {
                        Console.WriteLine($"Usted gana teniendo {cantidadTotal}");
                    }

                }
                else
                {
                    Console.WriteLine("Ahora le toca al dealer...");
                    DealerDeCartas(TurnoDealer, cantidadTotal);
                }
                    
                
            }
        }
        public void DealerDeCartas(bool dealer, int cantidadTotal)
        {
            int cantidaDealer=0;
            Random CartaDealer=new Random();

            while(dealer)
            {
                Console.WriteLine("El Dealer esta sacando una carta....");
                int carta= CartaDealer.Next(1,11);
                cantidaDealer+=carta;
                Console.WriteLine(cantidaDealer);

                if(cantidaDealer>21)
                {
                    Console.WriteLine($"El Dealer de cartas saco {cantidaDealer}");
                    Console.WriteLine($"Usted gana teniendo: {cantidadTotal}");
                    dealer=false;
                }
                else if(cantidaDealer==21)
                {
                    Console.WriteLine("El Dealer Gana ");

                }
            }

        }

        public void BlackJack()
        {
            Console.WriteLine("Vamos a jugar al BlackJack!!!!");
            Console.WriteLine("Pero primero lo primero.... Estas dispuesto a jugar? si/no");
            string eleccionJuego=Console.ReadLine()??"no".ToLower();
            int cantidadTotal=0;
            bool dealer=true;

            if(eleccionJuego=="si")
            {
                TurnoJugador(eleccionJuego ,cantidadTotal, dealer);
            }
            else
            {
                Console.WriteLine("Pues bueno, no jugaremos...");
            }
        }
    }
}