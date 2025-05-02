using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using practicarUNI.Juego;

namespace practicarUNI.Juego.padres
{
    public abstract class Personaje : IModificable
    {
        public bool JugadorDerrotado = false;
        public bool DefenderceDeEnemigo=false;

        public string Nombre{get;}
        protected double Jugador_VidaToatal{get; set;}
        protected double Jugador_VidaActual{get; set;}
        protected double Jugador_Daño{get; set;}
        protected double Jugador_Defensa{get; set;}
        public int Dinero_Jugador{get; set;} //esto es para que el jugador pueda tener dinero y comprar cosas en la tienda

        protected int Jugador_Nivel{get; set;}
        protected int Jugador_Progreso_Nivel{get; set;}
        public int Nivel => Jugador_Nivel;   //para que pueda ser accedido desde el inicio

        public Dictionary<string,Action<Enemigo>> Acciones{get; set;}= new Dictionary<string, Action<Enemigo>>(); //basicamente guarda un string(nombre del metodo) y recibe un enmigo para ejecutar un metodo
        public Dictionary<string,Action> AccionesSinObjetivo{get; set;} = new Dictionary<string, Action>();  //lo mismo que el otro diccionario pero este no requiere de un enemigo
        protected Dictionary<string, Action<object>> ModificadoresEstado = new Dictionary<string, Action<object>>(); //se utiliza para modificar atributos de los posibles eventos

        public Personaje(string nombre ,double VidaInicial, double DañoInicial, double DefensaInicial)
        {
            Nombre=nombre;    
            Jugador_VidaToatal = VidaInicial;
            Jugador_VidaActual = VidaInicial;
            Jugador_Daño = DañoInicial;
            Jugador_Defensa = DefensaInicial;
            Jugador_Nivel = 1;
            Jugador_Progreso_Nivel = 0;
    
            InicializarModificadoresEstado();
        }

        public virtual void MostrarStats()
        {
            Console.Write($"Sus estadisticas son: {Jugador_VidaToatal} puntos de vida, {Jugador_Daño} puntos de daño, {Jugador_Defensa} puntos de defensa,");
            Console.WriteLine($" Nivel: {Jugador_Nivel}");
        }
        protected virtual void EscaladoNivel_Jugador(int Jugador_Nivel)
        {
            Jugador_Daño+= Jugador_Nivel*(10);
            Jugador_VidaToatal+= Jugador_Nivel*(120);
            Jugador_Defensa+= Jugador_Nivel*(2);
        }
        public void SubirNivel(int Enemigo_ExpDrop)
        {
            int TopeEXP_Jugador=100+(Jugador_Nivel*25);

            Jugador_Progreso_Nivel+= Enemigo_ExpDrop;
            
            while(Jugador_Progreso_Nivel>=TopeEXP_Jugador)
            {
                Jugador_Progreso_Nivel-=TopeEXP_Jugador;
                Jugador_Nivel++;
                Console.WriteLine($"Subiste de Nivel! Nivel actual: {Jugador_Nivel}");
                EscaladoNivel_Jugador(Jugador_Nivel);
                TopeEXP_Jugador=100+(Jugador_Nivel*25);
            }
        }
        public void RecibirXP(int Enemigo_ExpDrop)
        {
            SubirNivel(Enemigo_ExpDrop);
        }
        
        protected abstract void Atacar(Enemigo enemigo);
        protected abstract void Defenderse();
        
        protected abstract void Curarse();
        public void RecibirDaño(double Enemigo_Daño, Enemigo enemigo)
        {
        double DañoMitigado= Enemigo_Daño;

            if(DefenderceDeEnemigo)
            {
                DañoMitigado*=0.7;
                DefenderceDeEnemigo = false;
                Console.WriteLine("Te preparas para mitigar parte del daño recibido");
            }

            double DañoFinal = Math.Max(DañoMitigado - Jugador_Defensa,0);
            Jugador_VidaActual -= DañoFinal;

            Console.WriteLine($"Recibiste {DañoFinal} de daño. Vida restante: {Jugador_VidaActual}");
            if (Jugador_VidaActual <= 0)
            {
                Jugador_VidaActual = 0;
                JugadorDerrotado = true;
                Console.WriteLine($"{Nombre} ha sido derrotado!");
            }
        }

        protected virtual void InicializarModificadoresEstado()
        {
            //aca se agregan los nuevos atributos 
            ModificadoresEstado["Salud"] = valor => 
            {
                try 
                {
                    Jugador_VidaActual += Convert.ToDouble(valor);
                }
                catch 
                {
                    Console.WriteLine("Error: Valor de salud inválido.");
                }
            };
            ModificadoresEstado["JugadorDerrotado"] = valor => 
            {
                try 
                {
                    JugadorDerrotado = Convert.ToBoolean(valor);
                }
                catch 
                {
                    Console.WriteLine("Error: Valor booleano inválido para JugadorDerrotado.");
                }
            };
            ModificadoresEstado["DefenderceDeEnemigo"] = valor => 
            {
                try 
                {
                    DefenderceDeEnemigo = Convert.ToBoolean(valor);
                }
                catch 
                {
                    Console.WriteLine("Error: Valor booleano inválido para DefenderceDeEnemigo.");
                }
            };
            ModificadoresEstado["Daño"] = valor => 
            {
                try 
                {
                    Jugador_Daño += Convert.ToDouble(valor);
                }
                catch 
                {
                    Console.WriteLine("Error: Valor de daño inválido.");
                }
            };
            ModificadoresEstado["Defensa"] = valor => 
            {
                try 
                {
                    Jugador_Defensa += Convert.ToDouble(valor);
                }
                catch 
                {
                    Console.WriteLine("Error: Valor de defensa inválido.");
                }
            };
            ModificadoresEstado["Experiencia"] = valor => 
            {
                try 
                {
                    int exp = Convert.ToInt32(valor);
                    RecibirXP(exp);
                }
                catch 
                {
                    Console.WriteLine("Error: Valor de experiencia inválido.");
                }
            };
        }
        public virtual void ModificarEstado(string atributo, object valor)
        {
            if (ModificadoresEstado.ContainsKey(atributo))
            {
                try
                {
                    ModificadoresEstado[atributo](valor);
                    Console.WriteLine($"{Nombre}: {atributo} modificado a {valor}.");
                    
                    
                    if (atributo == "Salud" && Jugador_VidaActual > Jugador_VidaToatal)
                        Jugador_VidaActual = Jugador_VidaToatal;
                        
                    
                    if (atributo == "Salud" && Jugador_VidaActual <= 0)
                    {
                        Jugador_VidaActual = 0;
                        JugadorDerrotado = true;
                        Console.WriteLine($"{Nombre} ha sido derrotado!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al modificar {atributo}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Advertencia: El atributo '{atributo}' no tiene un modificador definido en la clase Personaje.");
            }
        }

    }
}