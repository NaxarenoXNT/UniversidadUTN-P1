using System;
using MiProyecto.Ejercicios;
using MiProyecto.PuntosTP;
using practicarUNI.Juego;
using practicarUNI.Juego.padres;

namespace MiProyecto
{
    class Program
    {
        /*
        Console.WriteLine("Vamos a jugar Elije un juego:");
        Ejercicio juego = new Ejercicio();
        juego.BlackJack();

        Console.WriteLine("Vamos a resolver unos ejercicios!!!! elija uno:");
            PuntosTPs ejercicio=new PuntosTPs();
            ejercicio.Matrices1();


        ComienzoJuego jueguito= new ComienzoJuego();
            jueguito.Inicia();

        Console.WriteLine("Vamos a resolver unos ejercicios!!!! elija uno:");
            PuntosTPs ejercicio=new PuntosTPs();
            ejercicio.ejercicio12();


        */

        static void Main(string[] args)
        {

            ComienzoJuego jueguito= new ComienzoJuego();
            jueguito.Inicia();

            
        }


    }
}


/*
detecte varior errores:
primero: tengo que corregir el texto de seleccion de clases ya que se repiten los nombres    (corregido)
segundo: que el humano siempre se defiende no importa que haga                               (corregido)
tercero: no se si el turno del enemigo no se ejecuta con exito o simplemente no se muestra   (corregido parcialmente)
cuarto: tengo que rehacer la forma de tomar los turnos ya que siempre me pide que seleccion a un enemigo cuando no deberia de ser el caso   (corregido)
quinto: en caso de tener a muchos enemigos en el campo se rompe le fase de turnos de cada enemigo y creo que solo ejecuta un turno          (visto)
sexto: esto no es un error pero tengo que nerfear a los enemigos ya que estan re papota      (corregido)
septimo: los enemigos no mueren 
octavo: no termina la etapa de combate



Bueno realice cambios en la logica de acciones del enemigo para que ahora utilice una logica especifica y no aleatoriedad
ahora me toca corregir la seleccion de acciones del jugadror
me toca pasa lo que termine en el enemigo: Humano a los demas enemigos
tengo que corregir el diccionrio para que no pida enemigos otra ves


*/
        