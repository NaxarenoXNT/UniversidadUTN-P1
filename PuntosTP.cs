

using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace MiProyecto.PuntosTP
{
    public class PuntosTPs()
    {
        public void ejercicio5()
        {
            Console.WriteLine("Por favor ingrese sus valores: ");
            int num1= int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Por favor ingrese sus valores: ");
            int num2= int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Por favor ingrese sus valores: ");
            int num3= int.Parse(Console.ReadLine() ?? "0");

            int comienzo, final;
	        if(num1<num2)
	        {
	            comienzo=num1;
	            final=num2;
	        }
	        else
	        {
	            comienzo=num2;
	            final=num1;
	        }

            int contador=0;
            for(int i=comienzo; i<=final; i++)
            {
	            if(i % num3==0)
	            {
	            contador++;
	            Console.WriteLine(i);
	            }
            }
            if(contador==0)
            {
                Console.WriteLine($"No existen múltiplos de {num3} dentro de {num1} y {num2}");
            }
        }

        public void ejercicio6()
        {
            Console.WriteLine("Por favor ingrese el ancho de su rectangulo: ");
            int ancho= int.Parse(Console.ReadLine()??"0");
            Console.WriteLine("Por favor ingrese el largo de su rectangulo: ");
            int largo= int.Parse(Console.ReadLine()??"0");

            for(int i=0; i<largo; i++)
            {
                for(int j=0; j<ancho; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }

        public void ejercicio7()
        {
            Console.WriteLine("Por favor ingrese el ancho de su rectangulo: ");
            int ancho= int.Parse(Console.ReadLine()??"0");
            Console.WriteLine("Por favor ingrese el largo de su rectangulo: ");
            int largo= int.Parse(Console.ReadLine()??"0");

            for(int i=0; i<largo; i++)
            {
                for(int j=0; j<ancho; j++)
                {
                    if(i==0 || i==largo-1 || j==0 || j==ancho-1 )
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }

        }

        public void ejercicio8()
        {
            bool continuar=true;
            int contador=0;
            int total=0;
            int promedio=0;
            
            while(continuar)
            {
                Console.WriteLine("Ingrese un numero: ");
                int num=int.Parse(Console.ReadLine()??"0");
                contador++;

                if(num==0 && contador==0)
                {
                    Console.WriteLine("Su promedio es 0 con un total de 0 ingresos");
                    continuar=false;
                }
                else if(num!=0)
                {
                    total+=num;
                }
                else
                {
                    promedio=total/contador;
                    Console.WriteLine($"su promedio es de {promedio}, con un total de {contador-1} ingresos");
                    continuar=false;
                }
            }
        }

        public void ejercicio9()
        {
            List<int> numeros=new List<int>();
            bool seguir=true;

            do
            {
                Console.Write("Ingrese su numero: ");
                int num= int.Parse(Console.ReadLine()??"0");
                if(num==0)
                {
                    
                    
                    Console.WriteLine("Los numeros ingresados de mayor a menor son los siguiente: ");
                    Console.WriteLine(string.Join(",", numeros));
                    seguir=false;
                }
                else
                {
                    numeros.Add(num);
                    numeros.Sort();
                    numeros.Reverse();
                }
            }while(seguir);
                

                
        
        }

        public void ejercicio10()
        {
            Console.Write("Ingrese su numero: ");
            int num=int.Parse(Console.ReadLine()??"0");
            Console.Write("Ingrese un caracter: ");
            string caracter=Console.ReadLine()??"o";

            for(int i=0; i<=num; i++)
            {
                Console.Write(caracter);
            }
        }

        public void ejercicio11()
        {
            Console.Write("Ingrese su palabra: ");
            string palabra=Console.ReadLine()??"";
            int contador=0;

            foreach(char c in palabra)
            {
                contador++;
            }
            Console.WriteLine($"Su palabra tiene un total de {contador} letras");
        }

        public void ejercicio12()
        {
            Console.Write("Ingrese su palabra: ");
            string palabra=Console.ReadLine()??"";
            int contador=0;
            int cantidadPalabras=0;

            foreach(char c in palabra)
            {
                if(c==' ')
                {
                    cantidadPalabras++;
                }
                else if((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    contador++;
                }
                else if(contador==0)
                {
                    cantidadPalabras=-1;
                }
                
            }
            Console.WriteLine($"Lo que usted ingreso posee un total de {cantidadPalabras+1} palabras con un total de {contador} letras en total.");
        }
    }

}
